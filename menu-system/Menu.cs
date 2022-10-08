using System;
using System.Collections.Generic;

namespace menu_system {
	public class Menu {
		private int _selection;
		public int Selection {
			get => _selection;
			set 
			{
				if (value >= 0 && value < Options.Count) {
					_selection = value;
				} else {
					throw new Exception("_selection out of index");
				}
			}
		}

		private Dictionary<char, MenuOption> Options { get; set; }

		private ConsoleColor ForegroundColor { get; set; }
		private ConsoleColor BackgroundColor { get; set; }
		private ConsoleColor TitleForegroundColor { get; set; }
		private ConsoleColor TitleBackgroundColor { get; set; }
		private ConsoleColor SubtitleForegroundColor { get; set; }
		private ConsoleColor SubtitleBackgroundColor { get; set; }
		private ConsoleColor SelectionForegroundColor { get; set; }
		private ConsoleColor SelectionBackgroundColor { get; set; }
		private ConsoleColor BackAndExitForegroundColor { get; set; }
		private ConsoleColor BackAndExitBackgroundColor { get; set; }
		private ConsoleColor HintForegroundColor { get; set; }
		private ConsoleColor HintBackgroundColor { get; set; }
		private ConsoleColor ActiveForegroundColor { get; set; }
		private ConsoleColor ActiveBackgroundColor { get; set; }
		
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
			string selectionPoint = ">",
			ConsoleColor foregroundColor = ConsoleColor.White,
			ConsoleColor backgroundColor = ConsoleColor.Black,
			ConsoleColor titleForegroundColor = ConsoleColor.Black,
			ConsoleColor titleBackgroundColor = ConsoleColor.White,
			ConsoleColor subtitleForegroundColor = ConsoleColor.Black,
			ConsoleColor subtitleBackgroundColor = ConsoleColor.Gray,
			ConsoleColor selectionForegroundColor = ConsoleColor.Cyan,
			ConsoleColor selectionBackgroundColor = ConsoleColor.Black,
			ConsoleColor backAndExitForegroundColor = ConsoleColor.DarkRed,
			ConsoleColor backAndExitBackgroundColor = ConsoleColor.Black,
			ConsoleColor hintForegroundColor = ConsoleColor.DarkGray,
			ConsoleColor hintBackgroundColor = ConsoleColor.Black,
			ConsoleColor activeForegroundColor = ConsoleColor.Yellow,
			ConsoleColor activeBackgroundColor = ConsoleColor.Black
		) {
			if ((options.ContainsKey('e') && isMainMenu) || (options.ContainsKey('b') && !isMainMenu)) {
				throw new Exception("You cannot add an option with this character");
			}
			
			Options = options;
			ActiveForegroundColor = activeForegroundColor;
			ActiveBackgroundColor = activeBackgroundColor;
			ForegroundColor = foregroundColor;
			BackgroundColor = backgroundColor;
			TitleForegroundColor = titleForegroundColor;
			TitleBackgroundColor = titleBackgroundColor;
			SubtitleForegroundColor = subtitleForegroundColor;
			SubtitleBackgroundColor = subtitleBackgroundColor;
			SelectionForegroundColor = selectionForegroundColor;
			SelectionBackgroundColor = selectionBackgroundColor;
			BackAndExitForegroundColor = backAndExitForegroundColor;
			BackAndExitBackgroundColor = backAndExitBackgroundColor;
			HintForegroundColor = hintForegroundColor;
			HintBackgroundColor = hintBackgroundColor;
			_selection = 0;
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
			
			Console.BackgroundColor = BackgroundColor;
			
			Console.Clear();
			Console.CursorVisible = false;
			
			// Title and subtitle
			
			Console.BackgroundColor = TitleBackgroundColor;
			Console.ForegroundColor = TitleForegroundColor;
			
			if (_title != "") Console.WriteLine(_title.ToUpper());
			
			Console.BackgroundColor = SubtitleBackgroundColor;
			Console.ForegroundColor = SubtitleForegroundColor;
			
			if (_subTitle != "") Console.WriteLine(_subTitle);
			
			// Key/Option hint
			
			Console.BackgroundColor = HintBackgroundColor;
			Console.ForegroundColor = HintForegroundColor;
			
			Console.WriteLine("");
			Console.WriteLine("  KEY  OPTION");
			Console.WriteLine("");
			
			// Options
			
			Console.ForegroundColor = ConsoleColor.White;

			for (int i = 0; i < Options.Keys.Count; i++) {
				if (i == _selection) {
					Console.BackgroundColor = SelectionBackgroundColor;
					Console.ForegroundColor = SelectionForegroundColor;
				} else {
					Console.BackgroundColor = Options[keys[i]].BackgroundColor ?? BackgroundColor;
					Console.ForegroundColor = Options[keys[i]].ForegroundColor ?? ForegroundColor;
				}
				
				Console.Write(i == _selection ? _selectionPoint : _listPoint);
				Console.Write("  {0} - {1}", keys[i], Options[keys[i]].Name);

				// Extra Selector stuff
				
				Console.ForegroundColor = ConsoleColor.Yellow;
				
				if (Options[keys[i]].GetType() == typeof(MenuOptionWithStringSelector)
				    || Options[keys[i]].GetType() == typeof(MenuOptionWithNumberSelector)) {
					Console.BackgroundColor = SelectionBackgroundColor;
					Console.ForegroundColor = ForegroundColor;
					
					Console.Write("  |  ");

					if (i == _selection) {
						Console.BackgroundColor = ActiveBackgroundColor;
						Console.ForegroundColor = ActiveForegroundColor;
					}
					
					Console.Write("< {0} >", Options[keys[i]].CurrentOption());
				}
				
				Console.WriteLine("");
			}
			
			// Exit/Back option (if the menu is not the exit prompt: isMainMenu = false and exitPromt = true)

			if (_isMainMenu || !_exitPrompt) {
				if (_selection >= Options.Count) {
					Console.BackgroundColor = ConsoleColor.Black;
					Console.ForegroundColor = ConsoleColor.Red;
				} else {
					Console.BackgroundColor = BackAndExitBackgroundColor;
					Console.ForegroundColor = BackAndExitForegroundColor;
				}
				
				Console.Write(_selection >= Options.Count ? _selectionPoint : _listPoint);
				Console.WriteLine("  {0} - {1}", _isMainMenu ? "e" : "b", _isMainMenu ? _exitButtonText : _backButtonText);
			}
			
			// Selector Hints/Usage
			
			Console.BackgroundColor = HintBackgroundColor;
			Console.ForegroundColor = HintForegroundColor;
			
			Console.WriteLine("");
			
			if (_selection < Options.Count && (Options[keys[_selection]].GetType() == typeof(MenuOptionWithStringSelector)
			    || Options[keys[_selection]].GetType() == typeof(MenuOptionWithNumberSelector))) {
				Console.Write("LEFT/RIGHT : Change | ");	
			}
			
			// Menu Hints/Usage
			
			Console.Write("UP/DOWN : Move | ENTER : Select | ");
			Console.Write(_isMainMenu ? "E/ESC : Exit" : "B/BACKSPACE : Back");
		}

		public void NextOption() {
			_selection++;
			
			if (_selection > Options.Count - 1 && !_isMainMenu && _exitPrompt) _selection = 0;
			if (_selection > Options.Count) _selection = 0;
		}
		
		public void PreviousOption() {
			_selection--;
			
			if (_selection < 0 && !_isMainMenu && _exitPrompt) _selection = Options.Count - 1;
			if (_selection < 0) _selection = Options.Count;
		}

		public void Stop() => _isRendering = false;

		public void Run() {
			if (_isRendering) return;

			ConsoleColor backgroundColor = Console.BackgroundColor;
			ConsoleColor foregroundColor = Console.ForegroundColor;

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
						if (_selection == Options.Count) {
							_isRendering = false;
							break;
						}
						
						Options[keys[_selection]].Activate();
						
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
						if (_selection == Options.Count) break;
						
						if (Options[keys[_selection]].GetType() == typeof(MenuOptionWithStringSelector)
						|| Options[keys[_selection]].GetType() == typeof(MenuOptionWithNumberSelector)) {
							Options[keys[_selection]].PreviousOption();
						}
						
						break;
					
					case ConsoleKey.RightArrow:
						if (_selection == Options.Count) break;
						
						if (Options[keys[_selection]].GetType() == typeof(MenuOptionWithStringSelector)
						|| Options[keys[_selection]].GetType() == typeof(MenuOptionWithNumberSelector)) {
							Options[keys[_selection]].NextOption();
						}
						
						break;

					default:
						if (Options.ContainsKey(keyChar)) {
							_selection = keys.LastIndexOf(keyChar);
							Options[keyChar].Activate();
						}

						break;
				}

				// Exit prompt
				
				if (_exitPrompt && _isMainMenu && !_isRendering) {
					// Just render another menu
					
					// Setting the isMainMenu to false, and exitPrompt to true is a way to say that this
					// Menu is the exit prompt itself. So it will not have a back/exit button/option.
					
					Menu exitPrompt = new Menu(
						new Dictionary<char, MenuOption>(), 
						false, 
						_title, 
						_exitPromptText, 
						true
						);

					exitPrompt.Options = new Dictionary<char, MenuOption>() {
						{ 'y', new MenuOptionWithAction(_exitPromptTruthy, () => { exitPrompt.Stop(); }) },
						{ 'n', new MenuOptionWithAction(_exitPromptFalsy, () => {
							_isRendering = true; exitPrompt.Stop(); }) },
					};

					exitPrompt._selection = 1;
					
					exitPrompt.Run();
				}
			} while (key != ConsoleKey.Escape && _isRendering);

			_isRendering = false;
			
			// Clear the console
			
			Console.BackgroundColor = backgroundColor;
			Console.ForegroundColor = foregroundColor;
			
			Console.Clear();
		}
	}
}
