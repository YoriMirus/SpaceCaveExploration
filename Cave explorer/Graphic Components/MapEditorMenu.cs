﻿using System;
using System.Collections.Generic;
using System.Text;

using Cave_Explorer.Enums;
using Cave_Explorer.Models;
using Cave_Explorer.Helpers;

namespace Cave_Explorer.Graphic_Components
{
    class MapEditorMenu
    {
        string mapNameInput;

        private int currentCursorIndex;
        private int currentCursorIndexLimit;
        private MapEditorSection currentSection;
        private List<string> foundMaps;
        public MapEditorMenu()
        {
            mapNameInput = "";
            currentSection = MapEditorSection.Menu;
            DisplaySection();
            WaitForInput();
        }
        private void WaitForInput()
        {
            while (true)
            {
                ConsoleKey input = Console.ReadKey(true).Key;
                
                switch (input)
                {
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        if (currentCursorIndex > 0)
                            currentCursorIndex--;
                        break;
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        if (currentCursorIndex < currentCursorIndexLimit)
                            currentCursorIndex++;
                        break;
                    case ConsoleKey.Enter:
                        if(currentSection == MapEditorSection.Menu)
                        {
                            switch(currentCursorIndex)
                            {
                                case 0:
                                    currentSection = MapEditorSection.NewMap;
                                    currentCursorIndex = 0;
                                    Console.Clear();
                                    break;
                                case 1:
                                    currentSection = MapEditorSection.EditMap;
                                    currentCursorIndex = 0;
                                    Console.Clear();
                                    break;
                                case 2:
                                    currentCursorIndex = 0;
                                    Console.Clear();
                                    return;
                            }
                        }
                        else if (currentSection == MapEditorSection.EditMap)
                        {
                            StartEditor(foundMaps[currentCursorIndex]);
                        }
                        break;
                    case ConsoleKey.Escape:
                        if(currentSection == MapEditorSection.EditMap)
                        {
                            currentSection = MapEditorSection.Menu;
                            currentCursorIndex = 0;
                            Console.Clear();
                        }
                        else if(currentSection == MapEditorSection.Menu)
                        {
                            Console.Clear();
                            return;
                        }
                        break;
                }
                DisplaySection();
            }
        }

        //Displaying methods
        private void DisplaySection()
        {
            switch (currentSection)
            {
                case MapEditorSection.Menu:
                    DisplayMenu();
                    break;
                case MapEditorSection.NewMap:
                    DisplayNewMap();
                    break;
                case MapEditorSection.EditMap:
                    DisplayEditMap();
                    break;
                default:
                    throw new ArgumentException("Unexpected section.");
            }
        }
        private void DisplayMenu()
        {
            currentCursorIndexLimit = 2;
            Console.SetWindowSize(30, 14);
            Console.SetBufferSize(31, 15);

            MainMenuHelper.MakeFrame();
            MainMenuHelper.WriteInCenter("Map editor", 2);
            MainMenuHelper.FillALine('=', 1, 4);

            if(currentCursorIndex == 0)
                MainMenuHelper.WriteInCenter("Create a new map", 6, ConsoleColor.Gray, ConsoleColor.Blue);
            else
                MainMenuHelper.WriteInCenter("Create a new map", 6);

            if(currentCursorIndex == 1)
                MainMenuHelper.WriteInCenter("Edit an existing map", 8, ConsoleColor.Gray, ConsoleColor.Blue);
            else
                MainMenuHelper.WriteInCenter("Edit an existing map", 8);
            
            if(currentCursorIndex == 2)
                MainMenuHelper.WriteInCenter("Back to main menu", 10, ConsoleColor.Gray, ConsoleColor.Blue);
            else
                MainMenuHelper.WriteInCenter("Back to main menu", 10);

        }
        private void DisplayNewMap()
        {
            while (true)
            {
                currentCursorIndexLimit = 2;
                Console.SetWindowSize(30, 14);
                Console.SetBufferSize(31, 15);

                MainMenuHelper.MakeFrame();
                MainMenuHelper.WriteInCenter("New map", 2);
                MainMenuHelper.FillALine('=', 1, 4);

                MainMenuHelper.WriteInCenter("Map name: ", 6);
                MainMenuHelper.WriteText(mapNameInput.PadRight(14), 8, 8);
                MainMenuHelper.WriteInCenter("‾‾‾‾‾‾‾‾‾‾‾‾‾‾", 9);

                Console.SetCursorPosition(8 + mapNameInput.Length, 8);
                Console.CursorVisible = true;
                ConsoleKeyInfo input = Console.ReadKey();

                switch (input.Key)
                {
                    case ConsoleKey.Enter:
                        StartEditor();
                        return;
                    case ConsoleKey.Backspace:
                        if (mapNameInput.Length > 0)
                            mapNameInput = mapNameInput.Remove(mapNameInput.Length - 1);
                        break;
                    case ConsoleKey.Escape:
                        currentSection = MapEditorSection.Menu;
                        Console.Clear();
                        Console.CursorVisible = false;
                        DisplayMenu(); //I need to display the menu here, or else the player will be on a blank screen.
                        return;
                    default:
                        if (mapNameInput.Length < 14)
                        {
                            if (input.KeyChar == ' ')
                                mapNameInput += ' ';
                            else
                                //trim used for getting rid of characters like pgup, del, insert, tab
                                mapNameInput += input.KeyChar.ToString().Trim(); 
                        }
                        break;
                }
            }
        }
        private void DisplayEditMap()
        {
            foundMaps = Helper.GetAndVerifyMaps(Environment.CurrentDirectory + "\\Map layouts\\Main");

            currentCursorIndexLimit = foundMaps.Count - 1;

            MainMenuHelper.MakeFrame();
            MainMenuHelper.WriteInCenter("Edit a map", 2);

            for (int i = 0; i < foundMaps.Count; i++)
            {
                string mapName = foundMaps[i].Split('\\')[^1];
                if (currentCursorIndex == i)
                    MainMenuHelper.WriteInCenter(mapName, 4 + i, ConsoleColor.Gray, ConsoleColor.Blue);
                else
                    MainMenuHelper.WriteInCenter(mapName, 4 + i);
            }
        }

        /// <summary>
        /// Starts an editor by using the inputted map name
        /// </summary>
        private void StartEditor()
        {
            Console.Clear();
            new MapEditorUI(new MapEditor(mapNameInput)).Start();
            currentSection = MapEditorSection.Menu;
            Console.Clear();
            Console.CursorVisible = false;
            DisplaySection();
        }

        private void StartEditor(string mapDirectory)
        {
            Console.Clear();
            new MapEditorUI(new MapEditor(mapDirectory)).Start();
            currentSection = MapEditorSection.Menu;
            Console.Clear();
            Console.CursorVisible = false;
            DisplaySection();
        }
    }
}
