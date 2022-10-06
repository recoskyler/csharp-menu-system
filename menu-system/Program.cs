using System;
using System.Collections.Generic;

namespace menu_system {
	public class Program {
		private static int Sum(int a, int b) => a + b;

		public static void Main() {
			Dictionary<char, MenuOption> subMenuOptions = new Dictionary<char, MenuOption>()
			{
				{'1', new OptionWithAction("Sum 1 and 2", () => { Console.WriteLine(Sum(1, 2));
					Console.ReadKey();
				})},
				{'2', new OptionWithAction("Print YO", () => { Console.WriteLine("YO");
					Console.ReadKey();
				})}
			};
			
			Menu subMenu = new Menu(subMenuOptions, false, "the sub menu", "This is a sub menu");
			
			Dictionary<char, MenuOption> menuOptions = new Dictionary<char, MenuOption>()
			{
				{'a', new OptionWithAction("Sum 5 and 2", () => { Console.WriteLine(Sum(5, 2));
					Console.ReadKey();
				})},
				{'b', new OptionWithAction("Print HEY", () => { Console.WriteLine("HEY");
					Console.ReadKey();
				})},
				{'c', new OptionWithSubMenu("Go to sub menu", subMenu)}
			};

			Menu menu = new Menu(menuOptions, true, "the main menu", "This is a simple menu system");
			
			menu.Run();
		}
	}
}
