using System;
using System.Collections.Generic;

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
		
		public Menu(Dictionary<char, MenuOption> options,
			bool isMainMenu = false,
			string title = "",
			string subTitle = "",
			bool exitPrompt = true,
			string exitPromptText = "Are you sure you want to exit?",
			string exitPromptTruthy = "Yes",
			string exitPromptFalsy = "No",
			string backButtonText = "Back",
			string exitButtonText = "Exit",
			string listPoint = "~",
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

		private void Render(int selection) {
			List<char> keys = new List<char>(Options.Keys);
			
			// Console.BackgroundColor = ConsoleColor.Black;
			// Console.ForegroundColor = ConsoleColor.White;
			
			Console.Clear();
			Console.CursorVisible = false;
			
			if (_title != "") Console.WriteLine(_title.ToUpper());
			if (_subTitle != "") Console.WriteLine(_subTitle);
			
			Console.WriteLine("");
			Console.WriteLine("  KEY  OPTION");
			Console.WriteLine("");

			for (int i = 0; i < Options.Keys.Count; i++) {
				// if (i == selection) {
				// 	Console.BackgroundColor = ConsoleColor.Gray;
				// 	Console.ForegroundColor = ConsoleColor.White;
				// } else {
				// 	Console.BackgroundColor = ConsoleColor.Black;
				// 	Console.ForegroundColor = ConsoleColor.White;
				// }
				
				Console.Write(i == selection ? _selectionPoint : _listPoint);
				Console.Write("  {0} - {1}", keys[i], Options[keys[i]].Name);

				if (Options[keys[i]].GetType() == typeof(MenuOptionWithStringSelector)) {
					Console.Write("  |  < {0} >", Options[keys[i]].CurrentOption());
				}
				
				Console.WriteLine("");
			}

			// if (selection >= Options.Count) {
			// 	Console.BackgroundColor = ConsoleColor.Gray;
			// 	Console.ForegroundColor = ConsoleColor.White;
			// } else {
			// 	Console.BackgroundColor = ConsoleColor.Black;
			// 	Console.ForegroundColor = ConsoleColor.White;
			// }
			
			Console.Write(selection >= Options.Count ? _selectionPoint : _listPoint);
			Console.WriteLine("  {0} - {1}", _isMainMenu ? "e" : "b", _isMainMenu ? "Exit" : "Back");
			
			// Console.BackgroundColor = ConsoleColor.Black;
			// Console.ForegroundColor = ConsoleColor.White;
			
			Console.WriteLine("");

			if (selection < Options.Count && Options[keys[selection]].GetType() == typeof(MenuOptionWithStringSelector)) {
				Console.Write("LEFT/RIGHT : Change | ");	
			}
			
			Console.Write("UP/DOWN : Move | ENTER : Select | ");
			Console.Write(_isMainMenu ? "E/ESC : Exit" : "B/BACKSPACE : Back");
		}

		public void Stop() => _isRendering = false;

		public void Run() {
			if (_isRendering) return;

			_isRendering = true;

			int selection = 0;
			List<char> keys = new List<char>(Options.Keys);
			ConsoleKey key;
			char keyChar;

			do {
				Render(selection);
				
				while (!Console.KeyAvailable)
				{
					// Console.Write(".");
				}

				// Key is available - read it
				ConsoleKeyInfo kInfo = Console.ReadKey(false);
				
				key = kInfo.Key;
				keyChar = kInfo.KeyChar;

				switch (key) {
					case ConsoleKey.Enter:
						if (selection == Options.Count) {
							_isRendering = false;
							break;
						}
						
						Options[keys[selection]].Activate();
						
						break;
					
					case ConsoleKey.B:
						if (_isMainMenu) break;
						_isRendering = false;
						break;

					case ConsoleKey.Backspace:
						if (_isMainMenu) break;
						_isRendering = false;
						break;
					
					case ConsoleKey.E:
						if (!_isMainMenu) break;
						_isRendering = false;
						break;
					
					case ConsoleKey.UpArrow:
						if (--selection < 0) selection = Options.Count;
						break;
					
					case ConsoleKey.DownArrow:
						if (++selection > Options.Count) selection = 0;
						break;
					
					case ConsoleKey.LeftArrow:
						if (selection == Options.Count) break;
						
						if (Options[keys[selection]].GetType() == typeof(MenuOptionWithStringSelector)) {
							Options[keys[selection]].PreviousOption();
						}
						
						break;
					
					case ConsoleKey.RightArrow:
						if (selection == Options.Count) break;
						
						if (Options[keys[selection]].GetType() == typeof(MenuOptionWithStringSelector)) {
							Options[keys[selection]].NextOption();
						}
						
						break;

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
