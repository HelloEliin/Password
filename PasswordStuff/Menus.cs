﻿using System;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;

namespace PasswordStuff
{
    public class Menus

    {

        public static void FrontPageMenu()
        {
            Console.WriteLine("\n\n\n\nUSERSYSTEM\n\n" +
                "[S]IGN IN \n" +
                "[C]REATE ACCOUNT \n" +
                "[Q]UIT PROGRAM");

        }

        public static void MenuForUserAndMod()
        {
            Console.WriteLine("\n\n[1] MY USERINFO\n" +
                "[2] EDIT MY PROFILE\n" +
                "[10] SIGN OUT");
        }


        public static void MenuForAdmin()
        {
            Console.WriteLine(
                "\n\n[1] MY USERINFO\n" +
                "[2] EDIT MY PROFILE\n" +
                "[3] VIEW ALL USERS\n" +
                "[4] MENU FOR USERS\n" +
                "[5] CREATE NEW USER\n" +
                "[6] SHOW ALL MODS\n" +
                "[10] SIGN OUT");
        }

        public static void AdmSubMenu()
        {
            Console.WriteLine("\n\n" +
                "[1] BACK TO MENU\n" +
                "[2] VIEW ALL USERS\n" +
                "[3] EDIT USER\n" +
                "[4] DELETE USER\n" +
                "[5] CREATE NEW USER\n" +
                "[6] PROMOTE USER\n" +
                "[7] DEMOTE USER\n" +
                "[8] SEARCH USER\n\n");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1": Menus.MenuForAdmin();
                    break;
                case "2":
                    CreateUser.ShowAllUsers();
                    break; 
                case "3":
                    EditUserMenu();
                    break;
                case "4":
                    CreateUser.DeleteUser();
                    break;  
                case "5":
                    CreateUser.CreateNewUser();
                    break;
                case "6":
                    CreateUser.PromoteUser();
                    break;
                case "7":
                    CreateUser.DemoteUser();
                    break;
                case "8":
                    CreateUser.SearchUser();
                    break;
                default: Console.WriteLine("Try again.");
                    break;
            }



        }

        public static void EditUserMenu()
        {
            Console.WriteLine(
               "[1] BACK TO MENU\n" +
               "[2] CHANGE USERNAME\n" +
               "[3] CHANGE PASSWORD\n" +
               "[4] CHANGE NAME\n" +
               "[5] CHANGE EMAIL\n");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1": Menus.MenuForAdmin();
                    break;
                case "2": CreateUser.ChangeUsername();
                    break;
                case "3": CreateUser.ChangeUsersPassword();
                    break;
                case "4": CreateUser.ChangeUsersName();
                    break;
                case "5": CreateUser.ChangeUsersEmail();
                    break;
            }
        }


        public static void UserSystemMenu(int userIndex)
        {
            var json = CreateUserFile.GetJson();

            if (json[userIndex].AccessLevelOne == true || json[userIndex].AccessLevelMod == true)
            {
                Menus.MenuForUserAndMod();
            }

            if (json[userIndex].AccessLevelAdm == true)
            {

                Menus.MenuForAdmin();

            }

            var choice = Console.ReadLine();



            switch (choice)
            {
                case "1":
                    CreateUser.ShowMyUserInfo(userIndex);
                    Menus.UserSystemMenu(userIndex);
                    break;
                case "2":
                       CreateUser.EditOwnProfile(userIndex);
                        Menus.UserSystemMenu(userIndex);
                    
                    break;
                    case"3":
                    if (json[userIndex].AccessLevelAdm == true)
                    {
                        CreateUser.ShowAllUsers();
                        Menus.UserSystemMenu(userIndex);
                    }
                    else
                    {
                        Console.WriteLine("Try again");
                        Menus.UserSystemMenu(userIndex);
                    }
                    break;

                case "4":
                    if(json[userIndex].AccessLevelAdm == true)
                    {
                        Menus.AdmSubMenu();
                        Menus.UserSystemMenu(userIndex);
                    }
                    if (json[userIndex].AccessLevelOne == true || json[userIndex].AccessLevelMod == true)
                    {
                        Console.WriteLine("Try again");
                        Menus.UserSystemMenu(userIndex);
                    }

                    break;
                case "5":
                    if (json[userIndex].AccessLevelAdm == true)
                    {
                        CreateUser.CreateNewUser();
                        Menus.UserSystemMenu(userIndex);
                    }

                    if (json[userIndex].AccessLevelOne == true || json[userIndex].AccessLevelMod == true)
                    {
                        Console.WriteLine("Try again");
                        Menus.UserSystemMenu(userIndex);
                    }

                    break;
                case "6": if(json[userIndex].AccessLevelAdm == true)
                    {

                        CreateUser.ShowAllMods();
                        Menus.UserSystemMenu(userIndex);
                    }
                    else
                    {
                        Console.WriteLine("Try again");
                        Menus.UserSystemMenu(userIndex);
                    }
                    break;
                case "10":
                    break;
                default:
                    Console.WriteLine("Try again.");
                    Menus.UserSystemMenu(userIndex);
                    break;

            }





        }
    }
}