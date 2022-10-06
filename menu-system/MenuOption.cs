using System;

namespace menu_system {
	public abstract class MenuOption {
		public string Name;
		public abstract void Activate();
		protected MenuOption(string name) {
			Name = name;
		}

		protected MenuOption() {
			throw new NotImplementedException();
		}
	}
	
	public class OptionWithAction: MenuOption {
		private readonly Action _action;
			
		public OptionWithAction(string name, Action action) : base(name) {
			_action = action;
		}

		public override void Activate() {
			_action.Invoke();
		}
	}
		
	public class OptionWithSubMenu: MenuOption {
		private readonly Menu _submenu;
			
		public OptionWithSubMenu(string name, Menu submenu) : base(name) {
			_submenu = submenu;
		}
			
		public override void Activate() {
			_submenu.Run();
		}
	}
}
