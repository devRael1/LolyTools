﻿using Loly.src.Variables.Class;

namespace Loly.src.Menus.Core;

public class MenuBuilder
{
    private readonly int _drawMenuColumnPos;
    private readonly int _drawMenuRowPos;
    private readonly string[] _options;
    private int _currentSelection;
    private readonly bool _groupOptions;

    private MenuBuilder(string[] options, int row, int col, bool groupOptions)
    {
        _options = options;
        _currentSelection = 1;
        _drawMenuRowPos = row;
        _drawMenuColumnPos = col;
        _groupOptions = groupOptions;
    }

    public static MenuBuilder BuildMenu(string[] choices, int topLength, bool groupOptions = false)
    {
        MenuBuilder menu = new(choices, topLength + 1, 1, groupOptions);
        menu.ModifyMenuLeftJustified();
        ResetCursorVisible();
        SetCursorVisibility(false);
        Console.ResetColor();

        return menu;
    }

    private void ModifyMenuLeftJustified()
    {
        var space = "";

        var maximumWidth = _options.Select(t => t.Length).Prepend(0).Max();
        maximumWidth += 4;

        for (var i = 0; i < _options.Length; i++)
        {
            var spacesToAdd = maximumWidth - _options[i].Length;
            for (var j = 0; j < spacesToAdd; j++) space += "";

            _options[i] += space;
            space = "";
        }
    }

    private static void SetConsoleTextColor(ConsoleColor foreground)
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
        if (row > 0 && row < Console.WindowHeight) Console.CursorTop = row;
        if (column > 0 && column < Console.WindowWidth) Console.CursorLeft = column;
    }

    public int RunMenu()
    {
        var run = true;
        DrawMenu();
        while (run)
        {
            var keyPressedCode = CheckKeyPress();
            switch (keyPressedCode)
            {
                case 10:
                {
                    _currentSelection--;
                    if (_currentSelection < 1) _currentSelection = _options.Length;
                    break;
                }
                case 11:
                {
                    _currentSelection++;
                    if (_currentSelection > _options.Length) _currentSelection = 1;
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
        var leftPointer = "  ";
        for (var i = 0; i < _options.Length; i++)
        {
            Console.ResetColor();
            if (_groupOptions)
            {
                SetCursorPosition(_drawMenuRowPos + i % 5, _drawMenuColumnPos + (i / 5) * (_options.Max(o => o.Length) + 3));
            }
            else
            {
                SetCursorPosition(_drawMenuRowPos + i, _drawMenuColumnPos);
            }
            SetConsoleTextColor(ConsoleColor.White);
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