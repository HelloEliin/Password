using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;

namespace PasswordStuff
{
    public class Menus

    {

        public static void FrontPageMenu()
        {
            Console.WriteLine("\n\n\n\nWELCOME\n\n" +
                "[S]IGN IN \n" +
                "[C]REATE ACCOUNT");

        }

        public static void MenuForRegularUser()
        {
            Console.WriteLine("\n\n[1] MY USERINFO\n" +
                "[10] SIGN OUT");
        }


        public static void MenuForMod()
        {
            Console.WriteLine("\n\n[1] MY USERINFO\n" +
                "[2] PROMOTE USER\n" +
                "[3] DEMOTE USER\n" +
                "[10] SIGN OUT"
                );
        }

        public static void MenuForAdmin()
        {
            Console.WriteLine(
                "\n\n[1] MY USERINFO\n" +
                "[2] VIEW ALL USERS\n" +
                "[3] MENU FOR USERS\n" +
                "[4] CREATE NEW USER\n" +
                "[10] SIGN OUT");
        }

        public static void AdmSubMenu()
        {
            Console.WriteLine("\n\n" +
                "[1] BACK TO MENU\n" +
                "[2] VIEW ALL USERS\n" +
                "[3] EDIT USER\n" +
                "[4] DELETE USER\n" +
                "[5] CREATE NEW USER\n\n\n");

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
                case "3": CreateUser.ChangePassword();
                    break;
                case "4": CreateUser.ChangeName();
                    break;
                case "5": CreateUser.ChangeEmail();
                    break;
            }
        }


        public static void UserSystemMenu(int userIndex)
        {
            var json = CreateUserFile.GetJson();

            if (json[userIndex].AccessLevelOne == true)
            {
                Menus.MenuForRegularUser();
            }


            if (json[userIndex].AccessLevelMod == true)
            {
                Menus.MenuForMod();
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
                    break;
                case "2":  
                    if (json[userIndex].AccessLevelMod == true)
                    {
                        CreateUser.PromoteUser();
                        Menus.UserSystemMenu(userIndex);
                    }

                    if (json[userIndex].AccessLevelAdm == true)
                    {
                       CreateUser.ShowAllUsers();
                        Menus.UserSystemMenu(userIndex);
                    }
                    if (json[userIndex].AccessLevelOne == true)
                    {
                        Console.WriteLine("Select a choice");
                        Menus.UserSystemMenu(userIndex);
                    }
                    break;

                case "3":
                    if (json[userIndex].AccessLevelMod == true)
                    {
                        CreateUser.DemoteUser();
                        Menus.UserSystemMenu(userIndex);
                    }
                    if(json[userIndex].AccessLevelAdm == true)
                    {
                        Menus.AdmSubMenu();
                        Menus.UserSystemMenu(userIndex);
                    }
                    if (json[userIndex].AccessLevelOne == true)
                    {
                        Console.WriteLine("Select a choice");
                        Menus.UserSystemMenu(userIndex);
                    }

                    break;
                case "4":
                    if (json[userIndex].AccessLevelAdm == true)
                    {
                        CreateUser.CreateNewUser();
                        Menus.UserSystemMenu(userIndex);
                    }

                    if (json[userIndex].AccessLevelAdm == true || json[userIndex].AccessLevelMod == true)
                    {
                        Console.WriteLine("Select a choice");
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