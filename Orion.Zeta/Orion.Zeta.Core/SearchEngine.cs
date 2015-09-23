using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Orion.Zeta.Methods.Dev;
using Orion.Zeta.Methods.Dev.Shared;

namespace Orion.Zeta.Core {
	public class SearchEngine : ISearchEngine {
		private readonly IList<ISearchMethod> _searchMethods;
		private readonly IList<ISearchMethodAsync> _searchMethodsAsync;
		private int? _autoRefreshCache;
		private Timer _refreshCacheTimer;

		public SearchEngine() {
			_searchMethods = new List<ISearchMethod>();
			_searchMethodsAsync = new List<ISearchMethodAsync>();
		}

		public void RegisterMethod(ISearchMethod method) {
			_searchMethods.Add(method);
			method.Initialisation();
		}

		public void RegisterMethod(ISearchMethodAsync method) {
			_searchMethodsAsync.Add(method);
			method.InitialisationAsync();
		}

		public async Task<IEnumerable<IItem>> Search(string expression) {
			var items = new List<IItem>();
			IList<Task<IEnumerable<IItem>>> tasks = new List<Task<IEnumerable<IItem>>>();
			
			foreach (var searchMethod in _searchMethods.Where(sm => sm.IsMatching(expression))) {
				items = searchMethod.Search(expression).Concat(items).ToList();
			}
			foreach (var searchMethodAsync in _searchMethodsAsync.Where(sma => sma.IsMatching(expression))) {
				tasks.Add(searchMethodAsync.SearchAsync(expression));
			}
			if (!tasks.Any()) 
				return items;
			await Task.WhenAll(tasks);
			foreach (var task in tasks) {
				items = task.Result.Concat(items).ToList();
			}
			return items;
		}

	    public void RefreshCache() {
	        foreach (var searchMethod in _searchMethods) {
	            searchMethod.RefreshCache();
	        }

	        foreach (var searchMethodAsync in _searchMethodsAsync) {
	            searchMethodAsync.RefreshCache();
	        }
	    }

		public int? AutoRefreshCache {
			get { return _autoRefreshCache; }
			set {
				if (value.HasValue) {
					ActivateAutoRefreshCache(value.Value);
				}
				else {
					DeactivateAutoRefreshCache();
				}
				_autoRefreshCache = value;
			}
		}

		private void ActivateAutoRefreshCache(int interval) {
			if (_refreshCacheTimer != null) {
				_refreshCacheTimer.Interval = interval;
			}
			else {
				_refreshCacheTimer = new Timer(interval);
				_refreshCacheTimer.AutoReset = true;
				_refreshCacheTimer.Elapsed += (sender, args) => {
					RefreshCache();
				};
				_refreshCacheTimer.Start();
			}
		}
		private void DeactivateAutoRefreshCache() {
			if (_refreshCacheTimer == null) {
				return;
			}
			_refreshCacheTimer.Stop();
			_refreshCacheTimer.Dispose();
			_refreshCacheTimer = null;
		}

		public void UnRegister(ISearchMethod searchMethod) {
	        if (_searchMethods.Contains(searchMethod)) {
	            _searchMethods.Remove(searchMethod);
	        }
	        else if (_searchMethodsAsync.Contains(searchMethod)) {
	            _searchMethodsAsync.Remove(searchMethod as ISearchMethodAsync);
	        }
	    }
	}
}