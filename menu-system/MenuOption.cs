using System;

namespace menu_system {
	public abstract class MenuOption {
		public abstract void Activate();
	}
	
	public class WithAction: MenuOption {
		private readonly Action _action;
			
		WithAction(Action action) {
			_action = action;
		}

		public override void Activate() {
			_action.Invoke();
		}
	}
		
	public class WithSubMenu: MenuOption {
		private readonly Menu _submenu;
			
		WithSubMenu(Menu submenu) {
			_submenu = submenu;
		}
			
		public override void Activate() {
			_submenu.Run();
		}
	}
}
