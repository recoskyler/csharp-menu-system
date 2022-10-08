using System;
using System.Collections.Generic;
using System.Data;

namespace menu_system {
	public abstract class MenuOption {
		public readonly string Name;
		
		public ConsoleColor? ForegroundColor { get; set; }
		public ConsoleColor? BackgroundColor { get; set; }
		
		public abstract void Activate();
		
		public virtual void NextOption() {}
		
		public virtual void PreviousOption() {}

		public virtual string CurrentOption() {
			return Name;
		}
		
		protected MenuOption(string name, ConsoleColor? foregroundColor, ConsoleColor? backgroundColor) {
			Name = name;
			ForegroundColor = foregroundColor;
			BackgroundColor = backgroundColor;
		}

		protected MenuOption(ConsoleColor? foregroundColor, ConsoleColor? backgroundColor) {
			ForegroundColor = foregroundColor;
			BackgroundColor = backgroundColor;
			throw new NotImplementedException();
		}
	}
	
	public class MenuOptionWithAction: MenuOption {
		private readonly Action _action;
			
		public MenuOptionWithAction(
			string name,
			Action action,
			ConsoleColor? foregroundColor = null,
			ConsoleColor? backgroundColor = null) : base(name,
			foregroundColor,
			backgroundColor) {
			_action = action;
		}

		public override void Activate() {
			_action.Invoke();
		}
	}
		
	public class MenuOptionWithSubMenu: MenuOption {
		private readonly Menu _submenu;
			
		public MenuOptionWithSubMenu(string name, Menu submenu,
			ConsoleColor? foregroundColor = null,
			ConsoleColor? backgroundColor = null) : base(name,
			foregroundColor,
			backgroundColor) {
			_submenu = submenu;
		}
			
		public override void Activate() {
			_submenu.Run();
		}
	}
	
	public class MenuOptionWithStringSelector: MenuOption {
		private readonly List<string> _options;
		private readonly Action<string> _onChange;
		private int _selection;

		public int Selection {
			get => _selection;
			set {
				if (value >= 0 && value < _options.Count) {
					_selection = value;
				} else {
					throw new Exception("Selection out of index");
				}
			}
		}

		public MenuOptionWithStringSelector(string name, List<string> options, Action<string> onChange,
			ConsoleColor? foregroundColor = null,
			ConsoleColor? backgroundColor = null) : base(name,
			foregroundColor,
			backgroundColor) {
			if (options.Count < 2) throw new Exception("Must have at least 2 options");

			_onChange = onChange;
			_options = options;
			_selection = 0;
		}

		public override void NextOption() {
			if (++_selection >= _options.Count) _selection = 0;
			_onChange.Invoke(CurrentOption());
		}
		
		public override void PreviousOption() {
			if (--_selection < 0) _selection = _options.Count - 1;
			_onChange.Invoke(CurrentOption());
		}

		public override string CurrentOption() {
			return _options[_selection];
		}
			
		public override void Activate() {
			_onChange.Invoke(CurrentOption());
		}
	}
	
	public class MenuOptionWithNumberSelector: MenuOption {
		private readonly Action<int> _onChange;
		private readonly int _min;
		private readonly int _max;
		private int _selection;

		public int Selection {
			get => _selection;
			set {
				if (value >= _min && value <= _max) {
					_selection = value;
				} else {
					throw new Exception("Selection out of index");
				}
			}
		}
			
		public MenuOptionWithNumberSelector(string name, int min, int max, Action<int> onChange,
			ConsoleColor? foregroundColor = null,
			ConsoleColor? backgroundColor = null) : base(name,
			foregroundColor,
			backgroundColor) {
			if (min >= max || max - min < 2) throw new Exception("Must have at least 2 sorted options");
			
			_onChange = onChange;
			_min = min;
			_max = max;
			_selection = 0;
		}

		public override void NextOption() {
			if (++_selection > _max) _selection = _min;
			_onChange(_selection);
		}
		
		public override void PreviousOption() {
			if (--_selection < _min) _selection = _max;
			_onChange(_selection);
		}

		public override string CurrentOption() {
			return _selection.ToString();
		}
			
		public override void Activate() {
			_onChange(_selection);
		}
	}
}
