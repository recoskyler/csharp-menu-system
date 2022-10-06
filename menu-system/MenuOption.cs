using System;
using System.Collections.Generic;

namespace menu_system {
	public abstract class MenuOption {
		public readonly string Name;
		public abstract void Activate();
		public virtual void NextOption() {}
		public virtual void PreviousOption() {}

		public virtual string CurrentOption() {
			return Name;
		}
		
		protected MenuOption(string name) {
			Name = name;
		}

		protected MenuOption() {
			throw new NotImplementedException();
		}
	}
	
	public class MenuOptionWithAction: MenuOption {
		private readonly Action _action;
			
		public MenuOptionWithAction(string name, Action action) : base(name) {
			_action = action;
		}

		public override void Activate() {
			_action.Invoke();
		}
	}
		
	public class MenuOptionWithSubMenu: MenuOption {
		private readonly Menu _submenu;
			
		public MenuOptionWithSubMenu(string name, Menu submenu) : base(name) {
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
			
		public MenuOptionWithStringSelector(string name, Dictionary<string, Action> options) : base(name) {
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
}
