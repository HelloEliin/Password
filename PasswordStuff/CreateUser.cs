using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace PasswordStuff
{
    public class CreateUser
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public bool AccessLevelOne { get; set; }
        public bool AccessLevelMod { get; set; }
        public bool AccessLevelAdm { get; set; }


        public static void CreateNewUser()
        {
            var json = CreateUserFile.GetJson();

            Console.WriteLine("\nENTER USERNAME");
            var userName = Console.ReadLine();
            bool usernameAvalible = IfUserNameIsAvalible(userName);
            if (!usernameAvalible)
            {
                Console.WriteLine("\nSomeone else already uses this username");
                CreateNewUser();
            }
            if (string.IsNullOrEmpty(userName))
            {
                Console.WriteLine("You have to choose username");
                CreateNewUser();
            }

            Console.WriteLine("\nENTER YOUR FIRSTNAME");
            var firstName = Console.ReadLine();
            if (string.IsNullOrEmpty(firstName))
            {
                Console.WriteLine("\n\nYou have to enter a firstname");
            }
            Console.WriteLine("\nENTER YOUR LASTNAME");
            var lastName = Console.ReadLine();
            if (string.IsNullOrEmpty(lastName))
            {
                Console.WriteLine("\nYou have to enter lastname");
            }
            Console.WriteLine("\nENTER YOUR EMAIL");
            var email = Console.ReadLine();

            bool emailUsed = IfMailAlreadyExists(email);

            if (emailUsed)
            {
                Console.WriteLine("\nThere is already a account with this email");
                return;
            }

            if (!email.Contains("@") || !email.Contains(".") || string.IsNullOrEmpty(email))
            {
                Console.WriteLine("\nInvalid emailadress");
                return;
            }


            Console.WriteLine("\nENTER A PASSWORD. (MIN 8 LETTERS, 1 BIG LETTER AND ONE SYMBOL)");
            var password = ReadPassword();


            if (password.Length < 8)
            {
                Console.WriteLine("\nToo short");
                return;
            }
    

            else if (password.Any(char.IsSymbol) == false)
            {
                Console.WriteLine("\nSymbol requierd");
                return;
            }

            else if(password.Any(char.IsUpper) == false)
            {
                Console.WriteLine("\nBig letter requierd");
                return;
            }
            else
            {
                Console.WriteLine("\n\n\nYAY! ACCOUNT CREATED, YOU CAN NOW SIGN IN!");
            }

            var user = new CreateUser()
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = userName,
                Email = email,
                Password = password,
                AccessLevelOne = true,
                AccessLevelMod = false,
                AccessLevelAdm = false,

            };

            json.Add(user);
            CreateUserFile.UpDate(json);

            return;

        }



        public static void ShowMyUserInfo(int user)
        {
            var json = CreateUserFile.GetJson();
            Console.WriteLine("PRESS 'Q' TO GO BACK TO MENU" +
                "\n\n\nMY NAME\n" +
                json[user].FirstName + " " + json[user].LastName + "\n\n" +
                "MY EMAIL\n" +
                json[user].Email + "\n\n" +
                "MY USERNAME\n" +
                json[user].UserName + "\n\n" +
                "MY PASSWORD \n" +
                json[user].Password);

            var choice = Console.ReadLine().ToLower();

            if (choice == "q")
            {
                Menus.UserSystemMenu(user);
            }
            else
            {
                Console.WriteLine("PRESS 'Q' TO GO BACK TO MENU.");
                ShowMyUserInfo(user);
            }


        }




        public static void PromoteUser()
        {
            var json = CreateUserFile.GetJson();
            ShowAllUsers();
            Console.WriteLine("SELECT USER ABOVE");
            var choice = Console.ReadLine();
            int num = 0;


            bool valid = int.TryParse(choice, out num);
            if (!valid)
            {
                Console.WriteLine("You have to choose a number.");
                return;
            }

            Console.WriteLine("SELECT NEW ACCESSLEVEL");
            Console.WriteLine(
                "[1] ACCESSLEVEL MODERATOR" +
                "[2] ACCESSLEVEL ADMIN");

            var whatAccess = Console.ReadLine();

            if (whatAccess == "1")
            {
                json[num].AccessLevelMod = true;
                json[num].AccessLevelAdm = false;
                json[num].AccessLevelOne = false;

            }

            if (whatAccess == "2")
            {
                json[num].AccessLevelAdm = false;
                json[num].AccessLevelAdm = true;
                json[num].AccessLevelOne = false;
            }

            Console.WriteLine("USER IS PROMOTED");

            CreateUserFile.UpDate(json);
        }



        public static void DemoteUser()
        {
            var json = CreateUserFile.GetJson();
            ShowAllUsers();
            Console.WriteLine("SELECT USER ABOVE");
            var choice = Console.ReadLine();
            int num = 0;


            bool valid = int.TryParse(choice, out num);
            if (!valid)
            {
                Console.WriteLine("You have to choose a number.");
                return;
            }

            Console.WriteLine("SELECT NEW ACCESSLEVEL");
            Console.WriteLine(
                "[1] ACCESSLEVEL ONE\n" +
                "[2] ACCESSLEVEL MODERATOR\n");

            var whatAccess = Console.ReadLine();

            if (whatAccess == "1")
            {
                json[num].AccessLevelMod = false;
                json[num].AccessLevelAdm = false;
                json[num].AccessLevelOne = true;

            }

            if (whatAccess == "2")
            {
                json[num].AccessLevelAdm = false;
                json[num].AccessLevelAdm = false;
                json[num].AccessLevelOne = false;
            }

            Console.WriteLine("\n\nUSER IS DEMOTED");

            CreateUserFile.UpDate(json);
        }


        public static void DeleteUser()
        {
            var json = CreateUserFile.GetJson();
            ShowAllUsers();
            Console.WriteLine("SELECT USER ABOVE");
            var choice = Console.ReadLine();
            int num = 0;


            bool valid = int.TryParse(choice, out num);
            if (!valid)
            {
                Console.WriteLine("You have to choose a number.");
                return;
            }

            bool existingUser = IfUserExists(num);
            if (!existingUser)
            {
                return;
            }

            Console.WriteLine("DO YOU WANT DO DELETE THIS USER? Y/N");
            var yesOrNo = Console.ReadLine().ToLower();

            if (yesOrNo == "y")
            {
                json.RemoveAt(num);
                Console.WriteLine("\n\nUSER IS DELETED");

            }

            CreateUserFile.UpDate(json);
        }


        public static void ShowAllUsers()
        {
            var json = CreateUserFile.GetJson();
            int whichIndex = 0;

            foreach (var user in json)
            {
                Console.WriteLine("\n\n\n[" + whichIndex + "]\n\n" +
                    "NAME\n" + user.FirstName + " " + user.LastName + "\n\n" +
                    "EMAIL\n" + user.Email + "\n\n" +
                    "USERNAME\n" + user.UserName + "\n\n" +
                    "PASSWORD\n" + user.Password + "\n\n" +
                    " -- ACCESSLEVEL -- \n\n" + "USER\n" + user.AccessLevelOne + "\n" +
                    "\nMODERATOR\n" + user.AccessLevelMod + "\n" +
                    "\nADMIN\n" + user.AccessLevelAdm + "\n"
                    );
                whichIndex++;
            }


        }



        public static void ChangeUsername()
        {

            var json = CreateUserFile.GetJson();
            int num = 0;
            ShowAllUsers();
            Console.WriteLine("SELECT USER ABOVE");
            var choice = Console.ReadLine();

            bool valid = int.TryParse(choice, out num);
            if (!valid)
            {
                Console.WriteLine("You have to choose a number.");
                return;
            }

            Console.WriteLine("ENTER NEW USERNAME");
            var newUserName = Console.ReadLine();

            if (String.IsNullOrEmpty(newUserName))
            {
                Console.WriteLine("You have to enter new username");
                return;
            }

            json[num].UserName = newUserName;
            CreateUserFile.UpDate(json);



        }


        public static void ChangePassword()
        {
            var json = CreateUserFile.GetJson();
            int num = 0;
            ShowAllUsers();
            Console.WriteLine("SELECT USER ABOVE");
            var choice = Console.ReadLine();

            bool valid = int.TryParse(choice, out num);
            if (!valid)
            {
                Console.WriteLine("You have to choose a number.");
                return;
            }

            Console.WriteLine("ENTER NEW PASSWORD");
            var newPassword = Console.ReadLine();

            if (String.IsNullOrEmpty(newPassword))
            {
                Console.WriteLine("You have to enter new password");
                return;
            }

            json[num].Password = newPassword;
            CreateUserFile.UpDate(json);

        }





        public static void ChangeName()
        {

            var json = CreateUserFile.GetJson();
            int num = 0;
            ShowAllUsers();
            Console.WriteLine("SELECT USER ABOVE");
            var choice = Console.ReadLine();

            bool valid = int.TryParse(choice, out num);
            if (!valid)
            {
                Console.WriteLine("You have to choose a number.");
                return;
            }
            var existingUser = IfUserExists(num);
            if (!existingUser)
            {
                return;
            }

            Console.WriteLine("ENTER NEW FIRST NAME");
            var newFirstName = Console.ReadLine();

            if (String.IsNullOrEmpty(newFirstName))
            {
                Console.WriteLine("You have to enter new firstname");
                return;
            }

            Console.WriteLine("ENTER NEW LAST NAME");
            var newLastName = Console.ReadLine();

            if (String.IsNullOrEmpty(newLastName))
            {
                Console.WriteLine("You have to enter new last name");
                return;
            }

            json[num].FirstName = newFirstName;
            json[num].LastName = newLastName;

            CreateUserFile.UpDate(json);


        }


        public static void ChangeEmail()
        {
            var json = CreateUserFile.GetJson();
            int num = 0;
            ShowAllUsers();
            Console.WriteLine("SELECT USER ABOVE");
            var choice = Console.ReadLine();

            bool valid = int.TryParse(choice, out num);
            if (!valid)
            {
                Console.WriteLine("You have to choose a number.");
                return;
            }
            var existingUser = IfUserExists(num);
            if (!existingUser)
            {
                return;
            }

            Console.WriteLine("ENTER NEW EMAIL");
            var newEmail = Console.ReadLine();

            if (String.IsNullOrEmpty(newEmail))
            {
                Console.WriteLine("You have to enter new email");
                return;
            }

            json[num].Email = newEmail;
            Console.WriteLine("EMAIL CHANGED");

            CreateUserFile.UpDate(json);


        }



        public static bool IfUserExists(int user)
        {
            var json = CreateUserFile.GetJson();
            if (user > json.Count || user < 0)
            {
                Console.WriteLine("\n\nThat user dont exists");
                return false;
            }
            return true;
        }



        public static bool IfMailAlreadyExists(string email)
        {
            var json = CreateUserFile.GetJson();
            for(int i  = 0; i < json.Count; i++)
            {
                if (json[i].Email == email)
                {
                    return true;
                }
            }

            return false;
        }


        public static bool IfUserNameIsAvalible(string username)
        {
            var json = CreateUserFile.GetJson();
            for (int i = 0; i < json.Count; i++)
            {
                if (json[i].UserName == username)
                {
                    return false;
                }
            }

            return true;
        }


        public static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter)
            {
                if (info.Key != ConsoleKey.Backspace)
                {
                    Console.Write("*");
                    password += info.KeyChar;

                }

                else if (info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        password = password.Substring(0, password.Length - 1);
                        int pos = Console.CursorLeft;
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(pos - 1, Console.CursorTop);
                    }
                }
                info = Console.ReadKey(true);

            }
          
            Console.WriteLine();
            return password;
        }



    }


}