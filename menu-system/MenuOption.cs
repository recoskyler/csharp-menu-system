using System;
using System.Collections.Generic;

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
		private readonly Dictionary<string, Action> _options;
		private readonly List<string> _keys;
		private int Selection { get; set; }
			
		public MenuOptionWithStringSelector(string name, Dictionary<string, Action> options,
			ConsoleColor? foregroundColor = null,
			ConsoleColor? backgroundColor = null) : base(name,
			foregroundColor,
			backgroundColor) {
			if (options.Count < 2) throw new Exception("Must have at least 2 options");
			
			_options = options;
			_keys = new List<string>(_options.Keys);
			Selection = 0;
		}

		public override void NextOption() {
			if (++Selection >= _options.Count) Selection = 0;
		}
		
		public override void PreviousOption() {
			if (--Selection < 0) Selection = _options.Count - 1;
		}

		public override string CurrentOption() {
			return _keys[Selection];
		}
			
		public override void Activate() {
			_options[_keys[Selection]].Invoke();
		}
	}
	
	public class MenuOptionWithNumberSelector: MenuOption {
		private readonly Action<int> _onChange;
		private readonly int _min;
		private readonly int _max;
		private int Selection { get; set; }
			
		public MenuOptionWithNumberSelector(string name, int min, int max, Action<int> onChange,
			ConsoleColor? foregroundColor = null,
			ConsoleColor? backgroundColor = null) : base(name,
			foregroundColor,
			backgroundColor) {
			if (min >= max || max - min < 2) throw new Exception("Must have at least 2 options");
			
			_onChange = onChange;
			_min = min;
			_max = max;
			Selection = 0;
		}

		public override void NextOption() {
			if (++Selection > _max) Selection = _min;
			_onChange(Selection);
		}
		
		public override void PreviousOption() {
			if (--Selection < _min) Selection = _max;
			_onChange(Selection);
		}

		public override string CurrentOption() {
			return Selection.ToString();
		}
			
		public override void Activate() {
			_onChange(Selection);
		}
	}
}
