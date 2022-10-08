using System;
using System.Collections.Generic;

namespace menu_system {
	public class Menu {
		private Dictionary<char, MenuOption> Options { get; set; }
		private int Selection { get; set; }

		private readonly string _title;
		private readonly string _subTitle;
		private readonly string _backButtonText;
		private readonly string _exitButtonText;
		private readonly string _listPoint;
		private readonly string _selectionPoint;
		private readonly string _exitPromptText;
		private readonly string _exitPromptTruthy;
		private readonly string _exitPromptFalsy;
		
		private readonly bool _exitPrompt;
		private readonly bool _isMainMenu;
		
		private bool _isRendering;
		
		public Menu(Dictionary<char, MenuOption> options,
			bool isMainMenu = false,
			string title = "MENU",
			string subTitle = "Please choose an option",
			bool exitPrompt = false,
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
			Selection = 0;
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
			List<char> keys = new List<char>(Options.Keys);
			
			Console.BackgroundColor = ConsoleColor.Black;
			
			Console.Clear();
			Console.CursorVisible = false;
			
			// Title and subtitle
			
			// Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.White;
			
			if (_title != "") Console.WriteLine(_title.ToUpper());
			if (_subTitle != "") Console.WriteLine(_subTitle);
			
			// Key/Option hint
			
			// Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.Gray;
			
			Console.WriteLine("");
			Console.WriteLine("  KEY  OPTION");
			Console.WriteLine("");
			
			// Options
			
			Console.ForegroundColor = ConsoleColor.White;

			for (int i = 0; i < Options.Keys.Count; i++) {
				if (i == Selection) {
					// Console.BackgroundColor = ConsoleColor.Gray;
					Console.ForegroundColor = ConsoleColor.Cyan;
				} else {
					// Console.BackgroundColor = ConsoleColor.Black;
					Console.ForegroundColor = ConsoleColor.White;
				}
				
				Console.Write(i == Selection ? _selectionPoint : _listPoint);
				Console.Write("  {0} - {1}", keys[i], Options[keys[i]].Name);

				// Extra Selector stuff
				
				Console.ForegroundColor = ConsoleColor.Yellow;
				
				if (Options[keys[i]].GetType() == typeof(MenuOptionWithStringSelector)
				    || Options[keys[i]].GetType() == typeof(MenuOptionWithNumberSelector)) {
					Console.ForegroundColor = ConsoleColor.White;
					Console.Write("  |  ");
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.Write("< {0} >", Options[keys[i]].CurrentOption());
				}
				
				Console.WriteLine("");
			}
			
			// Exit/Back option (if the menu is not the exit prompt: isMainMenu = false and exitPromt = true)

			if (_isMainMenu || !_exitPrompt) {
				if (Selection >= Options.Count) {
					// 	Console.BackgroundColor = ConsoleColor.Gray;
					Console.ForegroundColor = ConsoleColor.Red;
				} else {
					// 	Console.BackgroundColor = ConsoleColor.Black;
					Console.ForegroundColor = ConsoleColor.DarkRed;
				}
				
				Console.Write(Selection >= Options.Count ? _selectionPoint : _listPoint);
				Console.WriteLine("  {0} - {1}", _isMainMenu ? "e" : "b", _isMainMenu ? _exitButtonText : _backButtonText);
			}
			// Selector Hints/Usage
			
			// Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.Gray;
			
			Console.WriteLine("");
			
			if (Selection < Options.Count && (Options[keys[Selection]].GetType() == typeof(MenuOptionWithStringSelector)
			    || Options[keys[Selection]].GetType() == typeof(MenuOptionWithNumberSelector))) {
				Console.Write("LEFT/RIGHT : Change | ");	
			}
			
			// Menu Hints/Usage
			
			Console.Write("UP/DOWN : Move | ENTER : Select | ");
			Console.Write(_isMainMenu ? "E/ESC : Exit" : "B/BACKSPACE : Back");
		}

		public void NextOption() {
			Selection++;
			
			if (Selection > Options.Count - 1 && !_isMainMenu && _exitPrompt) Selection = 0;
			if (Selection > Options.Count) Selection = 0;
		}
		
		public void PreviousOption() {
			Selection--;
			
			if (Selection < 0 && !_isMainMenu && _exitPrompt) Selection = Options.Count - 1;
			if (Selection < 0) Selection = Options.Count;
		}

		public void Stop() => _isRendering = false;

		public void Run() {
			if (_isRendering) return;

			_isRendering = true;

			List<char> keys = new List<char>(Options.Keys);
			ConsoleKey key;
			char keyChar;

			do {
				Render();
				
				while (!Console.KeyAvailable) {
					// Console.Write(".");
				}

				// Key is available - read it
				ConsoleKeyInfo kInfo = Console.ReadKey(false);
				
				key = kInfo.Key;
				keyChar = kInfo.KeyChar;

				switch (key) {
					case ConsoleKey.Enter:
						if (Selection == Options.Count) {
							_isRendering = false;
							break;
						}
						
						Options[keys[Selection]].Activate();
						
						break;
					
					case ConsoleKey.B:
						if (!_isMainMenu && !_exitPrompt) _isRendering = false;

							if (Options.ContainsKey(keyChar)) Options[keyChar].Activate();

						break;

					case ConsoleKey.Backspace:
						if (_isMainMenu || _exitPrompt) break;

						_isRendering = false;
						break;
					
					case ConsoleKey.E:
						if (_isMainMenu) _isRendering = false;
						if (Options.ContainsKey(keyChar)) Options[keyChar].Activate();

						break;
					
					case ConsoleKey.UpArrow:
						PreviousOption();
						break;
					
					case ConsoleKey.DownArrow:
						NextOption();
						break;
					
					case ConsoleKey.LeftArrow:
						if (Selection == Options.Count) break;
						
						if (Options[keys[Selection]].GetType() == typeof(MenuOptionWithStringSelector)
						|| Options[keys[Selection]].GetType() == typeof(MenuOptionWithNumberSelector)) {
							Options[keys[Selection]].PreviousOption();
						}
						
						break;
					
					case ConsoleKey.RightArrow:
						if (Selection == Options.Count) break;
						
						if (Options[keys[Selection]].GetType() == typeof(MenuOptionWithStringSelector)
						|| Options[keys[Selection]].GetType() == typeof(MenuOptionWithNumberSelector)) {
							Options[keys[Selection]].NextOption();
						}
						
						break;

					default:
						if (Options.ContainsKey(keyChar)) {
							Selection = keys.LastIndexOf(keyChar);
							Options[keyChar].Activate();
						}

						break;
				}

				// Exit prompt
				
				if (_exitPrompt && _isMainMenu && !_isRendering) {
					Menu exitPrompt = new Menu(new Dictionary<char, MenuOption>(), false, _title, _exitPromptText, true);

					exitPrompt.Options = new Dictionary<char, MenuOption>() {
						{ 'y', new MenuOptionWithAction(_exitPromptTruthy, () => { exitPrompt.Stop(); }) },
						{ 'n', new MenuOptionWithAction(_exitPromptFalsy, () => {
							_isRendering = true; exitPrompt.Stop(); }) },
					};
					
					exitPrompt.Run();
				}
			} while (key != ConsoleKey.Escape && _isRendering);

			_isRendering = false;
		}
	}
}
