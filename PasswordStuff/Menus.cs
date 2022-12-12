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
                "[2] PROMOTE USER\n" +
                "[3] DEMOTE USER\n" +
                "[4] DELETE USER\n" +
                "[5] ALL USERS\n" +
                "[6] EDIT USER\n" +
                "[7] CREATE NEW USER\n" +
                "[10] SIGN OUT");
        }


        public static void UserSystemMenu(int userIndex)
        {
            var json = CreateUserFile.GetJson();

            if(json[userIndex].AccessLevelOne == true)
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
                case "1": CreateUser.ShowMyUserInfo(userIndex);
                    break;
                case "2":
                    if (json[userIndex].AccessLevelMod == true || json[userIndex].AccessLevelAdm == true)
                    {
                        CreateUser.PromoteUser();
                    }
                    else
                    {
                        Console.WriteLine("Select a choice");
                        Menus.UserSystemMenu(userIndex);
                    }
                    break;
                case "3":
                    if (json[userIndex].AccessLevelMod == true || json[userIndex].AccessLevelAdm == true)
                    {
                        CreateUser.DemoteUser();
                    }
                    else
                    {
                        Console.WriteLine("Select a choice");
                        Menus.UserSystemMenu(userIndex);
                    }

                    break;  
                case "4":
                    if (json[userIndex].AccessLevelAdm == true)
                    {
                        CreateUser.DeleteUser();
                    }
                    else
                    {
                        Console.WriteLine("Select a choice");
                        Menus.UserSystemMenu(userIndex);
                    }

                    break;
                case "5":
                    if (json[userIndex].AccessLevelAdm == true)
                    {
                        CreateUser.ShowAllUsers();
                    }
                    else
                    {
                        Console.WriteLine("Select a choice");
                        Menus.UserSystemMenu(userIndex);
                    }
                    break;
                case "6":
                    if (json[userIndex].AccessLevelAdm == true)
                    {
                        CreateUser.EditUser();
                    }
                    else
                    {
                        Console.WriteLine("Select a choice");
                        Menus.UserSystemMenu(userIndex);
                    }
                    break;
                case "7":
                    if (json[userIndex].AccessLevelAdm == true)
                    {
                        CreateUser.CreateNewUser();
                    }
                    else
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