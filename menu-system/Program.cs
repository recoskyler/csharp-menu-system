using System;
using System.Collections.Generic;

namespace menu_system {
	public class Program {
		private static int Sum(int a, int b) => a + b;

		public static void Main() {
			// This is just a test program, showing the features of the library

			int someVal = -1;
			
			MenuOptionWithStringSelector strSelector = new MenuOptionWithStringSelector("Some multi option",
				new List<string> {"Basic", "Medium", "Advanced"}, s => { });
			
			MenuOptionWithNumberSelector numSelector = new MenuOptionWithNumberSelector("Some multi num option",
				1,
				10,
				(int index, int value) => { someVal = value; });
			
			List<MenuOption> subMenuOptions = new List<MenuOption>()
			{
				new MenuOptionWithAction("Sum 1 and 2", () => { Console.WriteLine(Sum(1, 2));
					Console.ReadKey();
				}),
				new MenuOptionWithAction("Print YO", () => { Console.WriteLine("YO");
					Console.ReadKey();
				}),
				strSelector,
				numSelector,
			};
			
			Menu subMenu = new Menu(subMenuOptions, false, "the sub menu", "This is a sub menu");
			
			List<MenuOption> menuOptions = new List<MenuOption>()
			{
				new MenuOptionWithAction("Sum 5 and 2", () => { Console.WriteLine(Sum(5, 2));
					Console.ReadKey();
				}),
				new MenuOptionWithAction("Print HEY", () => { Console.WriteLine("HEY");
					Console.ReadKey();
				}),
				new MenuOptionWithSubMenu("Go to sub menu", subMenu)
			};

			Menu menu = new Menu(menuOptions, true, "the main menu", "This is a simple menu system", true);
			
			menu.Run();
			
			Console.WriteLine("");
			Console.WriteLine(strSelector.CurrentOption());
			Console.WriteLine(someVal);
		}
	}
}
