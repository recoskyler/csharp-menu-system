using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace menu_system {
	public class Menu {
		private Dictionary<char, MenuOption> Options { get; set; }

		private string _title;
		private string _subTitle;
		private bool _exitPrompt;
		private string _backButtonText;
		private string _exitButtonText;
		private string _listPoint;
		private string _selectionPoint;
		private string _exitPromptText;
		private string _exitPromptTruthy;
		private string _exitPromptFalsy;
		private bool _isRendering;
		private bool _isMainMenu;
		
		Menu(Dictionary<char, MenuOption> options,
			bool isMainMenu = false,
			string title = "",
			string subTitle = "",
			bool exitPrompt = true,
			string exitPromptText = "Are you sure you want to exit?",
			string exitPromptTruthy = "Yes",
			string exitPromptFalsy = "No",
			string backButtonText = "Back",
			string exitButtonText = "Exit",
			string listPoint = "-",
			string selectionPoint = ">"
		) {
			if ((options.ContainsKey('e') && isMainMenu) || (options.ContainsKey('b') && !isMainMenu)) {
				throw new Exception("You cannot add an option with this character");
			}
			
			Options = options;
			_listPoint = listPoint;
			_selectionPoint = selectionPoint;
			_isRendering = false;
			_backButtonText = backButtonText;
			_exitButtonText = exitButtonText;
			_exitPrompt = exitPrompt;
			_exitPromptText = exitPromptText;
			_exitPromptTruthy = exitPromptTruthy;
			_exitPromptFalsy = exitPromptFalsy;
			_title = title;
			_subTitle = subTitle;
			_isMainMenu = isMainMenu;
		}

		public void AddOption(char key, MenuOption option) {
			if ((key == 'e' && _isMainMenu) || (key == 'b' && !_isMainMenu)) {
				throw new Exception("You cannot add an option with this character");
			}
			
			Options.Add(key, option);
		}
		
		public void RemoveOption(char key) {
			if ((key == 'e' && _isMainMenu) || (key == 'b' && !_isMainMenu)) {
				throw new Exception("You cannot remove the option with this character");
			}
			
			Options.Remove(key);
		}

		private void Render() {
			
		}

		public void Run() {
			if (_isRendering) return;

			_isRendering = true;

			int selection = 0;
			List<char> keys = new List<char>(Options.Keys);
			ConsoleKey key;
			char keyChar;


			do {
				while (!Console.KeyAvailable)
				{
					Console.Write(".");
				}

				// Key is available - read it
				ConsoleKeyInfo kInfo = Console.ReadKey(false);
				
				key = kInfo.Key;
				keyChar = kInfo.KeyChar;

				switch (key) {
					case ConsoleKey.Enter:
						if (selection == Options.Count) return;
						
						Options[keys[selection]].Activate();
						
						break;
					
					case ConsoleKey.Backspace:
						if (_isMainMenu) break;
						return;
					
					case ConsoleKey.E:
						if (!_isMainMenu) break;
						return;
					
					default:
						if (Options.ContainsKey(keyChar)) {
							Options[keyChar].Activate();
						}

						break;
				}
				
				if (key == ConsoleKey.NumPad1) {
					Console.WriteLine(ConsoleKey.NumPad1.ToString());
				} else if (key == ConsoleKey.NumPad2) {
					Console.WriteLine(ConsoleKey.NumPad1.ToString());
				}

			} while (key != ConsoleKey.Escape && _isRendering);

			_isRendering = false;
		}
	}
}
