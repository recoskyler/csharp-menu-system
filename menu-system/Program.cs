using System;
using System.Collections.Generic;

namespace menu_system {
	public class Program {
		private static int Sum(int a, int b) => a + b;

		public static void Main() {
			MenuOptionWithStringSelector strSelector = new MenuOptionWithStringSelector("Some multi option",
				new Dictionary<string, Action>() {
					{ "Basic", () => { } },
					{ "Medium", () => { } },
					{ "Advanced", () => { } }
				});
			
			Dictionary<char, MenuOption> subMenuOptions = new Dictionary<char, MenuOption>()
			{
				{'1', new MenuOptionWithAction("Sum 1 and 2", () => { Console.WriteLine(Sum(1, 2));
					Console.ReadKey();
				})},
				{'2', new MenuOptionWithAction("Print YO", () => { Console.WriteLine("YO");
					Console.ReadKey();
				})},
				{'3', strSelector}
			};
			
			Menu subMenu = new Menu(subMenuOptions, false, "the sub menu", "This is a sub menu");
			
			Dictionary<char, MenuOption> menuOptions = new Dictionary<char, MenuOption>()
			{
				{'a', new MenuOptionWithAction("Sum 5 and 2", () => { Console.WriteLine(Sum(5, 2));
					Console.ReadKey();
				})},
				{'b', new MenuOptionWithAction("Print HEY", () => { Console.WriteLine("HEY");
					Console.ReadKey();
				})},
				{'c', new MenuOptionWithSubMenu("Go to sub menu", subMenu)}
			};

			Menu menu = new Menu(menuOptions, true, "the main menu", "This is a simple menu system");
			
			menu.Run();
			
			Console.WriteLine("");
			Console.WriteLine(strSelector.CurrentOption());
		}
	}
}
