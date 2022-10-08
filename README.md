# C# Menu System

A simple, Object-Oriented, customizable menu system library.

## Usage

For a full implementation example, see [Program.cs](./menu-system/Program.cs).

```csharp
MenuOptionWithStringSelector strSelector = new MenuOptionWithStringSelector("Some multi option",
    new List<string> {"Basic", "Medium", "Advanced"}, s => { });

MenuOptionWithNumberSelector numSelector = new MenuOptionWithNumberSelector("Some multi num option",
    1,
    10,
    (int a) => { someVal = a; });

Dictionary<char, MenuOption> subMenuOptions = new Dictionary<char, MenuOption>()
{
    {'1', new MenuOptionWithAction("Sum 1 and 2", () => { Console.WriteLine(Sum(1, 2));
        Console.ReadKey();
    })},
    {'2', new MenuOptionWithAction("Print YO", () => { Console.WriteLine("YO");
        Console.ReadKey();
    })},
    {'3', strSelector},
    {'4', numSelector},
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

Menu menu = new Menu(menuOptions, true, "the main menu", "This is a simple menu system", true);

menu.Run();
```

## State

🤷‍♂ **I guess it's ok**

The minimum viable state has been reached. You can use it, but there might be a few bugs.

## [License](./LICENSE)

The license can be found [here](./LICENSE)

## About

By Adil Atalay Hamamcıoğlu

_Made this for the C# lecture assignment. Hopefully will remember to reuse it._
