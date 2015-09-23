using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Win32;
using Orion.Zeta.Methods.Dev;
using Orion.Zeta.Methods.Dev.Shared;
using Orion.Zeta.Methods.Dev.Shared.Helpers;
using Orion.Zeta.Methods.Dev.Shared.Implementations;

namespace Orion.Zeta.Methods.WebSearch {
	public class WebSearchMethod : ISearchMethodAsync {
		private readonly Regex _regex;
		private Icon _defaultBrowserIcon;
		private string _defaultBrowser;

		public WebSearchMethod() {
			_regex = new Regex("^@.+$");
		}

		public bool IsMatching(string expression) {
			return this._regex.IsMatch(expression);
		}

		public void Initialisation() {
			_defaultBrowser = GetSystemDefaultBrowser();
			_defaultBrowserIcon = IconHelper.GetIcon(_defaultBrowser);
		}

		public IEnumerable<IItem> Search(string expression) {
			return new List<IItem> {
				new Item {
					Value = expression,
					Rank = 0,
					DisplayName = expression,
					Execute = new Execute {
						Program = HttpUtility.UrlEncode("https://www.google.com/search?q=" + expression)
					},
					Icon = _defaultBrowserIcon
				}
			};
		}
		private string GetSystemDefaultBrowser() {
			string name = string.Empty;
			RegistryKey regKey = null;

			try {
				//set the registry key we want to open
				regKey = Registry.ClassesRoot.OpenSubKey("HTTP\\shell\\open\\command", false);

				//get rid of the enclosing quotes
				name = regKey.GetValue(null).ToString().ToLower().Replace("" + (char) 34, "");

				//check to see if the value ends with .exe (this way we can remove any command line arguments)
				if (!name.EndsWith("exe"))
					//get rid of all command line arguments (anything after the .exe must go)
					name = name.Substring(0, name.LastIndexOf(".exe") + 4);

			}
			catch (Exception ex) {
				name = string.Format("ERROR: An exception of type: {0} occurred in method: {1} in the following module: {2}",
					ex.GetType(), ex.TargetSite, this.GetType());
			}
			finally {
				//check and see if the key is still open, if so
				//then close it
				if (regKey != null)
					regKey.Close();
			}
			//return the value
			return name;
		}

		public void RefreshCache() {
			// nothing to do
		}

		public Task InitialisationAsync() {
			return Task.Run(() => Initialisation());
		}

		public Task<IEnumerable<IItem>> SearchAsync(string expression) {
			return Task.Run(() => Search(expression));
		}
	}
}