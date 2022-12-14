# C# Menu System

A simple, Object-Oriented, customizable menu system library.

## State

🤷‍♂ **I guess it's ok**

The minimum viable state has been reached. You can use it, but expect a few bugs.

## Features

- Background and foreground color customization of almost everything
- Menus, submenus, and menu options
- Menu option with action. Invokes an `Action`.
- Menu option with submenu. Runs another `Menu`. This menu can not have `isMainMenu` set to `true`.
- Menu option with string selector. A menu with multiple string options, which scrolls sideways. An `onChange` `Action<string>` gets invoked each time the selection changes.
- Menu option with number range selector. A menu with multiple integer options which scrolls sideways. An `onChange` `Action<int>` gets invoked each time the selection changes.

## Usage

The `.dll` can be found in the [Releases](https://github.com/recoskyler/csharp-menu-system/releases) section.

For a full implementation example, see [Program.cs](./menu-system/Program.cs).

```csharp
MenuOptionWithStringSelector strSelector = new MenuOptionWithStringSelector("Some multi option",
    new List<string> {"Basic", "Medium", "Advanced"}, s => { }, 'm');

MenuOptionWithNumberSelector numSelector = new MenuOptionWithNumberSelector("Some multi num option",
    1, // Start of the range
    10, // Count of the range, so it will have 10 elements
    (int a) => { someVal = a; },
    2); // Steps. The list will be 1, 3, 5, 7, 9...

List<MenuOption> subMenuOptions = new List<MenuOption>()
{
    new MenuOptionWithAction("Sum 1 and 2", () => { 
            Console.WriteLine(Sum(1, 2));
            Console.ReadKey();
        }),
    new MenuOptionWithAction("Print YO", () => {
            Console.WriteLine("YO");
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
```

## [License](./LICENSE)

The license can be found [here](./LICENSE)

## About

By Adil Atalay Hamamcıoğlu

_Made this for the C# lecture assignment. Hopefully will remember to reuse it._
