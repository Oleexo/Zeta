using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Orion.Zeta.Core;
using Orion.Zeta.Persistence;
using Orion.Zeta.Persistence.LocalStorage;
using Orion.Zeta.Services;
using Orion.Zeta.Settings;
using Orion.Zeta.Settings.Containers;
using Orion.Zeta.ViewModels;

namespace Orion.Zeta {
	public class Zeta : IModifiableGeneralSetting {
		public class SettingWindowChangedEventArgs {
			public SettingWindowChangedEventArgs(WindowStatus status) {
				Status = status;
			}

			public enum WindowStatus {
				Close,
				Open
			}
			public WindowStatus Status { get; private set; }
		}

		#region Singleton
		public static Zeta Instance;
		#endregion

		#region Properties
		#endregion
		#region Events
		public event EventHandler<SettingWindowChangedEventArgs> SettingWindowChanged;
		#endregion
		#region Fields
		private readonly App _app;
		private readonly Logger _logger;
		private MainWindow _mainWindow;
		private MainViewModel _mainViewModel;
		private Task _initialisationSearchEngineTask;
		#region Services

		private readonly Lazy<ISettingsService> _settingsService;
		private readonly ISettingRepository _settingRepository;
		private readonly Lazy<ISearchMethodService> _methodService;
		private readonly Lazy<ISearchEngine> _searchEngine;
		private ISettingsService SettingsService => _settingsService.Value;
		private ISearchMethodService SearchMethodService => _methodService.Value;
		public ISearchEngine SearchEngine => _searchEngine.Value;
		#endregion
		#endregion

		public Zeta(App app) {
			_app = app;
			_logger = new Logger();
			Instance = this;
			_settingRepository = new SettingRepository();
			_searchEngine = new Lazy<ISearchEngine>(() => new SearchEngine());
			_settingsService = new Lazy<ISettingsService>(() => new SettingsService(_settingRepository));
			_methodService = new Lazy<ISearchMethodService>(() => new SearchMethodService(new SearchMethodPool(SearchEngine), new MefSearchMethodLoader()));
		}

		public void WakeUpApplication() {
			_mainWindow.WakeUpApplication();
		}

		public async void OpenSettingWindow() {
			await _initialisationSearchEngineTask;
			SettingWindowChanged?.Invoke(this, new SettingWindowChangedEventArgs(SettingWindowChangedEventArgs.WindowStatus.Open));
			WakeUpApplication();
			var settingWindow = new SettingWindow(SettingsService, SearchMethodService);
			settingWindow.Closed += async (sender, args) => {
				SearchMethodService.ManageMethodsBySetting(SettingsService);
				SettingWindowChanged?.Invoke(this, new SettingWindowChangedEventArgs(SettingWindowChangedEventArgs.WindowStatus.Close));
				await SettingsService.SaveChangesAsync();
			};
			settingWindow.Show();
		}

		public Task InitialisationSearchEngineAsync() {
			_initialisationSearchEngineTask = Task.Run(() => InitialisationSearchEngine());
			return _initialisationSearchEngineTask;
		}

		private void InitialisationSearchEngine() {
			try {
				SettingsService.RegisterGlobal(new GeneralSettingContainer(new ApplicationSettingService(SettingsService), this));
				SettingsService.RegisterGlobal(new StyleSettingContainer(new ApplicationSettingService(SettingsService), _mainViewModel));
				SearchMethodService.RegisterSearchMethods(SettingsService);
				SearchMethodService.RegisterSettings(SettingsService);
			}
			catch (Exception e) {
				_logger.LogError("Initialisation", e);
			}
		}

		public void Start() {
			_mainWindow = new MainWindow();
			_mainViewModel = _mainWindow.DataContext as MainViewModel;
		}

		#region IModifiableGeneralSetting
		private bool _startOnBoot;
		public bool StartOnBoot {
			get { return _startOnBoot; }
			set {
				_startOnBoot = value;
				ToogleStartOnBoot(value);
			}
		}

		private void ToogleStartOnBoot(bool value) {
			var startupPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
			var exePath = Assembly.GetExecutingAssembly().Location;
			var shortcutName = "Zeta";
			var completePath = Path.Combine(startupPath, shortcutName + ".lnk");
			if (value) {
				if (!File.Exists(completePath))
					Shortcut.Create(shortcutName, startupPath, exePath);
			}
			else {
				if (File.Exists(completePath))
					File.Delete(completePath);
			}
		}

		public void EnabledAutoRefresh(int interval) {
			SearchEngine.AutoRefreshCache = interval*60000;
		}

		public void DisabledAutoRefresh() {
			SearchEngine.AutoRefreshCache = null;
		}
		#endregion
	}

	public interface IModifiableGeneralSetting {
		bool StartOnBoot { get; set; }
		void EnabledAutoRefresh(int interval);
		void DisabledAutoRefresh();
	}
}