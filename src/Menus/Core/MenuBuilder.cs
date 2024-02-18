using Loly.src.Variables.Class;
using System.Drawing;
using Console = Colorful.Console;

namespace Loly.src.Menus.Core;

internal class MenuBuilder
{
    private readonly int _drawMenuColumnPos;
    private readonly int _drawMenuRowPos;
    private readonly string[] _options;
    private int _currentSelection;

    private MenuBuilder(string[] options, int row, int col)
    {
        _options = options;
        _currentSelection = 1;
        _drawMenuRowPos = row;
        _drawMenuColumnPos = col;
    }

    public static MenuBuilder BuildMenu(string[] choices, int topLength)
    {
        MenuBuilder menu = new(choices, topLength + 1, 1);
        menu.ModifyMenuLeftJustified();
        ResetCursorVisible();
        SetCursorVisibility(false);
        Console.ResetColor();

        return menu;
    }

    private void ModifyMenuLeftJustified()
    {
        string space = "";

        int maximumWidth = _options.Select(t => t.Length).Prepend(0).Max();
        maximumWidth += 6;

        for (int i = 0; i < _options.Length; i++)
        {
            int spacesToAdd = maximumWidth - _options[i].Length;
            for (int j = 0; j < spacesToAdd; j++)
            {
                space += "";
            }

            _options[i] += space;
            space = "";
        }
    }

    private static void SetConsoleTextColor(Color foreground)
    {
        Console.ForegroundColor = foreground;
    }

    private static void ResetCursorVisible()
    {
        Console.CursorVisible = Console.CursorVisible != true;
    }

    public static void SetCursorVisibility(bool visible)
    {
        Console.CursorVisible = visible;
    }

    private static void SetCursorPosition(int row, int column)
    {
        if (row > 0 && row < Console.WindowHeight)
        {
            Console.CursorTop = row;
        }

        if (column > 0 && column < Console.WindowWidth)
        {
            Console.CursorLeft = column;
        }
    }

    public int RunMenu()
    {
        bool run = true;
        DrawMenu();
        while (run)
        {
            int keyPressedCode = CheckKeyPress();
            switch (keyPressedCode)
            {
                case 10:
                    {
                        _currentSelection--;
                        if (_currentSelection < 1)
                        {
                            _currentSelection = _options.Length;
                        }

                        break;
                    }
                case 11:
                    {
                        _currentSelection++;
                        if (_currentSelection > _options.Length)
                        {
                            _currentSelection = 1;
                        }

                        break;
                    }
                case 12:
                    run = false;
                    break;
            }

            DrawMenu();
        }

        return _currentSelection;
    }

    private void DrawMenu()
    {
        string leftPointer = "  ";
        for (int i = 0; i < _options.Length; i++)
        {
            Console.ResetColor();
            SetCursorPosition(_drawMenuRowPos + i, _drawMenuColumnPos);
            SetConsoleTextColor(Color.White);
            if (i == _currentSelection - 1)
            {
                SetConsoleTextColor(Colors.PrimaryColor);
                leftPointer = "» ";
            }

            Console.WriteLine(leftPointer + _options[i]);
            leftPointer = "  ";
            Console.ResetColor();
        }
    }

    private static int CheckKeyPress()
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        {
            ConsoleKey keyPressed = keyInfo.Key;
            return keyPressed switch
            {
                ConsoleKey.UpArrow => 10,
                ConsoleKey.DownArrow => 11,
                ConsoleKey.Enter => 12,
                _ => 0
            };
        }
    }
}