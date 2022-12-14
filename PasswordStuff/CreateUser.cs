using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using System.Text;
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

            Console.WriteLine("\nENTER USERNAME (OR PRESS 'Q' TO QUIT)");
            var userName = Console.ReadLine();
            if (userName == "q" || userName == "Q")
            {
                return;
            }

            if (String.IsNullOrEmpty(userName))
            {
                do
                {
                    Console.WriteLine("You have to choose username. Enter a username");
                    userName = Console.ReadLine();
                    if (userName == "q" || userName == "Q")
                    {
                        return;
                    }
                } while (userName == "");
            }


            bool usernameAvalible = IsUserNameAvalible(userName);

            while (!usernameAvalible)
            {
                Console.WriteLine("\nSomeone else already uses this username. Enter another.");
                userName = Console.ReadLine();
                if (userName == "q" || userName == "Q")
                {
                    return;
                }

                if (string.IsNullOrEmpty(userName))
                {
                    Console.WriteLine("You have to choose username");

                }
                usernameAvalible = IsUserNameAvalible(userName);

            }

            Console.WriteLine("ENTER FIRST NAME");
            var firstName = Console.ReadLine();

            if (firstName == "q" || firstName == "Q")
            {
                return;
            }

            if (String.IsNullOrEmpty(firstName))
            {
                do
                {
                    Console.WriteLine("You have to enter first name");
                    firstName = Console.ReadLine();
                    if (firstName == "q" || firstName == "Q")
                    {
                        return;
                    }
                } while (firstName == "");

            }




            Console.WriteLine("\nENTER YOUR LASTNAME");
            var lastName = Console.ReadLine();
            if (lastName == "q" || lastName == "Q")
            {
                return;
            }

            if (String.IsNullOrEmpty(lastName))
            {
                do
                {
                    Console.WriteLine("You have to enter lastname");
                    lastName = Console.ReadLine();
                    if (lastName == "q" || lastName == "Q")
                    {
                        return;
                    }
                } while (lastName == "");

            }



            Console.WriteLine("\nENTER YOUR EMAIL");
            var email = Console.ReadLine();


            if (email == "q" || email == "Q")
            {
                return;
            }

            if (String.IsNullOrEmpty(email))
            {
                do
                {
                    Console.WriteLine("You have to enter an email.");
                    email = Console.ReadLine();
                    if (email == "q" || email == "Q")
                    {
                        return;
                    }

                } while (email == "");

            }

            if (!email.Contains("@") || !email.Contains("."))
            {
                do
                {
                    Console.WriteLine("That doesn't look like an email. Try again.");
                    email = Console.ReadLine();
                    if (email == "q" || email == "Q")
                    {
                        return;
                    }
                } while (!email.Contains("@") || !email.Contains("."));

            }

            bool emailUsed = IfMailAlreadyExists(email);
            if (emailUsed)
            {

                Console.WriteLine("\nThere is already an account with this email. Have you forgotten your password? Contact support.");
                return;

            }



            Console.WriteLine("\nENTER A PASSWORD. (MIN 8 LETTERS, 1 BIG LETTER AND ONE SYMBOL)");
            var password = ReadPassword();


            if (password.Length < 8)
            {
                do
                {
                    Console.WriteLine("\nToo short");
                    password = ReadPassword();

                } while(password.Length < 8);
           
            }

            if (password.Any(char.IsSymbol) == false && password.Any(char.IsUpper) == false)
            {
                do
                {
                    Console.WriteLine("Symbol and big letter requierd");
                    password = ReadPassword();
                } while (password.Any(char.IsSymbol) == false && password.Any(char.IsUpper) == false);
            }



            if (password.Any(char.IsSymbol) == false)
            {
                do
                {
                    Console.WriteLine("\nSymbol requierd");
                    password = ReadPassword();

                } while (password.Any(char.IsSymbol) == false);
       
            }

            if (password.Any(char.IsUpper) == false)
            {
                do
                {
                    Console.WriteLine("\nBig letter requierd");
                    password = ReadPassword();
                } while (password.Any(char.IsUpper) == false);

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
            Console.WriteLine("\n\nPRESS ENTER TO GO BACK TO MENU" +
                "\n\n\nMY NAME\n" +
                json[user].FirstName + " " + json[user].LastName + "\n\n" +
                "MY EMAIL\n" +
                json[user].Email + "\n\n" +
                "MY USERNAME\n" +
                json[user].UserName + "\n\n" +
                "MY PASSWORD \n" +
                json[user].Password);

            var choice = Console.ReadLine().ToLower();
            if (choice == "")
            {
                Menus.UserSystemMenu(user);
                return;
            }
        }




        public static void PromoteUser()
        {
            var json = CreateUserFile.GetJson();
            ShowAllUsers();
            Console.WriteLine("SELECT USER ABOVE (OR PRESS 'Q TO QUIT)");
            var choice = Console.ReadLine();

            if (choice == "q" || choice == "Q")
            {
                return;
            }
            int num = 0;

            bool valid = int.TryParse(choice, out num);

            if (!valid)
            {
                do
                {
                    if (!valid)
                    {
                        Console.WriteLine("You have to choose a number.");
                        choice = Console.ReadLine();
                        if(choice == "q" || choice == "Q")
                        {
                            return;
                        }
                        valid = int.TryParse(choice, out num);

                    }
                } while (!valid);
            }

            if(num > (json.Count - 1) || num < 0)
            {
                do
                {
                    Console.WriteLine("That user don't exists.");
                    choice = Console.ReadLine();
                    if (choice == "q" || choice == "Q")
                    {
                        return;
                    }
                    valid = int.TryParse(choice, out num);

                } while (num > (json.Count - 1) || num < 0);
            }


            Console.WriteLine("SELECT NEW ACCESSLEVEL\n");
            Console.WriteLine(
                "[1] ACCESSLEVEL MODERATOR\n" +
                "[2] ACCESSLEVEL ADMIN\n\n");

            int number = 0;
            var whatAccess = Console.ReadLine();
            if (whatAccess == "q" || whatAccess == "Q")
            {
                return;
            }

            bool validNumber = int.TryParse(whatAccess, out number);

            if (!validNumber || whatAccess == "")
            {
                do
                {
                    Console.WriteLine("That level don't exists. Try again.");
                    whatAccess = Console.ReadLine();
                    if (whatAccess == "2" || whatAccess == "1")
                    {
                        break;
                    }
                    if(whatAccess == "q" || whatAccess == "Q")
                    {
                        return;
                    }

                } while (whatAccess != "2" || whatAccess != "1");
            }




            if (whatAccess == "1")
            {

                json[num].AccessLevelMod = true;
                json[num].AccessLevelAdm = false;
                json[num].AccessLevelOne = false;

            }

            if (whatAccess == "2")
            {
                json[num].AccessLevelMod = false;
                json[num].AccessLevelAdm = true;
                json[num].AccessLevelOne = false;

            }

            Console.WriteLine("\n\nUSER IS PROMOTED");
            CreateUserFile.UpDate(json);


        }

    



    public static void DemoteUser()
        {
            var json = CreateUserFile.GetJson();
            ShowAllUsers();
            Console.WriteLine("SELECT USER ABOVE (OR PRESS 'Q TO QUIT)");
            var choice = Console.ReadLine();

            if (choice == "q" || choice == "Q")
            {
                return;
            }
            int num = 0;

            bool valid = int.TryParse(choice, out num);

            if (!valid)
            {
                do
                {
                    if (!valid)
                    {
                        Console.WriteLine("You have to choose a number.");
                        choice = Console.ReadLine();
                        if (choice == "q" || choice == "Q")
                        {
                            return;
                        }
                        valid = int.TryParse(choice, out num);

                    }
                } while (!valid);
            }

            if (num > (json.Count - 1) || num < -1)
            {
                do
                {
                    Console.WriteLine("That user don't exists.");
                    choice = Console.ReadLine();
                    if (choice == "q" || choice == "Q")
                    {
                        return;
                    }
                    valid = int.TryParse(choice, out num);

                } while (num > (json.Count - 1) || num < -1);
            }


            Console.WriteLine("SELECT NEW ACCESSLEVEL\n");
            Console.WriteLine(
                "[1] ACCESSLEVEL ONE\n" +
                "[2] ACCESSLEVEL MODERATOR\n");

            int number = 0;
            var whatAccess = Console.ReadLine();
            if (whatAccess == "q" || whatAccess == "Q")
            {
                return;
            }


            bool validNumber = int.TryParse(whatAccess, out number);

            if (!validNumber || whatAccess == "")
            {
                do
                {
                    Console.WriteLine("That level don't exists. Try again.");
                    whatAccess = Console.ReadLine();
                    if (whatAccess == "2" || whatAccess == "1")
                    {
                        break;
                    }
                    if (whatAccess == "q" || whatAccess == "Q")
                    {
                        return;
                    }

                } while (whatAccess != "2" || whatAccess != "1");
            }

            if (whatAccess == "1")
            {

                json[num].AccessLevelOne = true;
                json[num].AccessLevelAdm = false;
                json[num].AccessLevelMod = false;

            }

            if (whatAccess == "2")
            {
                json[num].AccessLevelAdm = false;
                json[num].AccessLevelMod = true;
                json[num].AccessLevelOne = false;

            }

            Console.WriteLine("\n\nUSER IS DEMOTED");
            CreateUserFile.UpDate(json);


        }


        public static void DeleteUser()
        {
            var json = CreateUserFile.GetJson();
            ShowAllUsers();
            Console.WriteLine("SELECT USER ABOVE (OR PRESS 'Q TO QUIT)");
            var choice = Console.ReadLine();

            if (choice == "q" || choice == "Q")
            {
                return;
            }
            int num = 0;

            bool valid = int.TryParse(choice, out num);

            if (!valid)
            {
                do
                {
                    if (!valid)
                    {
                        Console.WriteLine("You have to choose a number.");
                        choice = Console.ReadLine();
                        if (choice == "q" || choice == "Q")
                        {
                            return;
                        }
                        valid = int.TryParse(choice, out num);

                    }
                } while (!valid);
            }

            if (num > (json.Count -1) || num < -1)
            {
                do
                {
                    Console.WriteLine("That user don't exists.");
                    choice = Console.ReadLine();
                    if (choice == "q" || choice == "Q")
                    {
                        return;
                    }
                    valid = int.TryParse(choice, out num);

                } while (num > (json.Count -1)|| num < 0);
            }

            Console.WriteLine("DO YOU WANT DO DELETE THIS USER? Y/N");
            var yesOrNo = Console.ReadLine().ToLower();

            if (yesOrNo == "y")
            {
                json.RemoveAt(num);
                Console.WriteLine("\n\nUSER IS DELETED");

            }
        else {
                return;
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
            ShowAllUsers();
            Console.WriteLine("SELECT USER ABOVE (OR PRESS 'Q TO QUIT)");
            var choice = Console.ReadLine();

            if (choice == "q" || choice == "Q")
            {
                return;
            }
            int num = 0;

            bool valid = int.TryParse(choice, out num);

            if (!valid)
            {
                do
                {
                    if (!valid)
                    {
                        Console.WriteLine("You have to choose a number.");
                        choice = Console.ReadLine();
                        if (choice == "q" || choice == "Q")
                        {
                            return;
                        }
                        valid = int.TryParse(choice, out num);

                    }
                } while (!valid);
            }

            if (num > (json.Count - 1) || num < 0)
            {
                do
                {
                    Console.WriteLine("That user don't exists.");
                    choice = Console.ReadLine();
                    if (choice == "q" || choice == "Q")
                    {
                        return;
                    }
                    valid = int.TryParse(choice, out num);

                } while (num > (json.Count - 1) || num < 0);
            }

            Console.WriteLine("ENTER NEW USERNAME");
            var newUserName = Console.ReadLine();
            if(newUserName == "q" || newUserName == "Q")
            {
                return;
            }

            do
            {

                if (String.IsNullOrEmpty(newUserName))
                {
                    Console.WriteLine("You have to enter new username");
                    newUserName = Console.ReadLine();
                    if(newUserName == "q" || newUserName == "Q")
                    {
                        return;
                    }
                }
            } while (string.IsNullOrEmpty(newUserName));


            Console.WriteLine("USERNAME CHANGED");


            json[num].UserName = newUserName;
            CreateUserFile.UpDate(json);


             
        }


        public static void ChangePassword()
        {
            var json = CreateUserFile.GetJson();
            ShowAllUsers();
            Console.WriteLine("SELECT USER ABOVE (OR PRESS 'Q TO QUIT)");
            var choice = Console.ReadLine();

            if (choice == "q" || choice == "Q")
            {
                return;
            }
            int num = 0;

            bool valid = int.TryParse(choice, out num);

            if (!valid)
            {
                do
                {
                    if (!valid)
                    {
                        Console.WriteLine("You have to choose a number.");
                        choice = Console.ReadLine();
                        if (choice == "q" || choice == "Q")
                        {
                            return;
                        }
                        valid = int.TryParse(choice, out num);

                    }
                } while (!valid);
            }

            if (num > (json.Count - 1) || num < 0)
            {
                do
                {
                    Console.WriteLine("That user don't exists.");
                    choice = Console.ReadLine();
                    if (choice == "q" || choice == "Q")
                    {
                        return;
                    }
                    valid = int.TryParse(choice, out num);

                } while (num > (json.Count - 1) || num < 0);
            }

            Console.WriteLine("ENTER NEW PASSWORD");
            var newPassword = ReadPassword();
            if (newPassword == "q" || newPassword == "Q")
            {
                return;
            }
            bool validOrNot = ValidPassword(newPassword);
            if (!validOrNot)
            {
                do
                {
                    Console.WriteLine("\nENTER NEW PASSWORD");
                    newPassword = ReadPassword();
                    validOrNot = ValidPassword(newPassword);
                    if(newPassword == "q" || newPassword == "Q")
                    {
                        return;
                    }

                } while (!validOrNot);
            }

            Console.WriteLine("\n\nPASSWORD CHANGED");

            json[num].Password = newPassword;
            CreateUserFile.UpDate(json);
        }





        public static void ChangeName()
        {
            var json = CreateUserFile.GetJson();
            ShowAllUsers();
            Console.WriteLine("SELECT USER ABOVE (OR PRESS 'Q TO QUIT)");
            var choice = Console.ReadLine();

            if (choice == "q" || choice == "Q")
            {
                return;
            }
            int num = 0;

            bool valid = int.TryParse(choice, out num);

            if (!valid)
            {
                do
                {
                    if (!valid)
                    {
                        Console.WriteLine("You have to choose a number.");
                        choice = Console.ReadLine();
                        if (choice == "q" || choice == "Q")
                        {
                            return;
                        }
                        valid = int.TryParse(choice, out num);

                    }
                } while (!valid);
            }

            if (num > (json.Count - 1) || num < 0)
            {
                do
                {
                    Console.WriteLine("That user don't exists.");
                    choice = Console.ReadLine();
                    if (choice == "q" || choice == "Q")
                    {
                        return;
                    }
                    valid = int.TryParse(choice, out num);

                } while (num > (json.Count - 1) || num < 0);
            }

            Console.WriteLine("ENTER NEW FIRSTNAME");
            var newFirstName = Console.ReadLine();

            if (newFirstName == "q" || newFirstName == "Q")
            {
                return;
            }

            do
            {

                if (String.IsNullOrEmpty(newFirstName))
                {
                    Console.WriteLine("You have to enter new firstname");
                    newFirstName = Console.ReadLine();

                    if (newFirstName == "q" || newFirstName == "Q")
                    {
                        return;
                    }
                }
            } while (string.IsNullOrEmpty(newFirstName));


            Console.WriteLine("ENTER NEW LASTNAME");
            var newLastName = Console.ReadLine();

            if (newLastName == "q" || newLastName == "Q")
            {
                return;
            }

            do
            {

                if (String.IsNullOrEmpty(newLastName))
                {
                    Console.WriteLine("You have to enter new firstname");
                    newLastName = Console.ReadLine();

                    if (newLastName == "q" || newLastName == "Q")
                    {
                        return;
                    }
                }
            } while (string.IsNullOrEmpty(newLastName));

            Console.WriteLine("\n\nNAME CHANGED");

            json[num].FirstName = newFirstName;
            json[num].LastName = newLastName;

            CreateUserFile.UpDate(json);
        }






        public static void ChangeEmail()
        {
            var json = CreateUserFile.GetJson();
            ShowAllUsers();
            Console.WriteLine("SELECT USER ABOVE (OR PRESS 'Q TO QUIT)");
            var choice = Console.ReadLine();

            if (choice == "q" || choice == "Q")
            {
                return;
            }
            int num = 0;

            bool valid = int.TryParse(choice, out num);

            if (!valid)
            {
                do
                {
                    if (!valid)
                    {
                        Console.WriteLine("You have to choose a number.");
                        choice = Console.ReadLine();
                        if (choice == "q" || choice == "Q")
                        {
                            return;
                        }
                        valid = int.TryParse(choice, out num);

                    }
                } while (!valid);
            }

            if (num > (json.Count - 1) || num < 0)
            {
                do
                {
                    Console.WriteLine("That user don't exists.");
                    choice = Console.ReadLine();
                    if (choice == "q" || choice == "Q")
                    {
                        return;
                    }
                    valid = int.TryParse(choice, out num);

                } while (num > (json.Count - 1) || num < 0);
            }

            Console.WriteLine("ENTER NEW EMAIL");
            var newEmail = Console.ReadLine();

            string truey = IsValidEmail(newEmail);
            if (truey == "-1")
            {
                do
                {
                    Console.WriteLine("ENTER NEW EMAIL");
                    newEmail = Console.ReadLine();
                    truey = IsValidEmail(newEmail);

                } while (truey == "-1");
            }

            if(truey == "-2")
            {
                return;
            }


            Console.WriteLine("\n\nEMAIL CHANGED");
            json[num].Email = newEmail;


            CreateUserFile.UpDate(json);
        }












        public static bool IfUserExists(int user)
        {
            var json = CreateUserFile.GetJson();
            if (user > json.Count || user < 0)
            {
                Console.WriteLine("\n\nThat user don't exists");
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


        public static bool IsUserNameAvalible(string username)
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



        public static void ShowAllMods()
        {
            var json = CreateUserFile.GetJson();
            for (int i = 0; i < json.Count; i++)
            {
                if (json[i].AccessLevelMod == true)
                {
                    Console.WriteLine("\n\nIS MOD\n(USERNAME)\n\n" + json[i].UserName);
                }

            }

            var isThereAnyMods = json.All(x => x.AccessLevelMod == false);
            if (isThereAnyMods)
            {
                Console.WriteLine("\n\nNO MODS IN THIS SYSTEM");
            }

        }
         

        public static void SearchUser()
        {
            var json = CreateUserFile.GetJson();
            Console.WriteLine("ENTER USERNAME TO SEARCH FOR\n");
            var userToSearch = Console.ReadLine();

            for (int i = 0; i < json.Count; i++)
            {
                if (json[i].UserName == userToSearch)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n\nUSER FOUND\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(json[i].UserName + "\n\n" +
                        "AT POSITION  " + "[" + i + "]" + "  IN USERFILE");
                    return;
                }
            }
            var isUserExisting = json.All(x => x.UserName == userToSearch);
            if (!isUserExisting)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("NO MATCHES FOUND");
                Console.ForegroundColor = ConsoleColor.White;
            }

        }

        public static string IsValidEmail(string email)
        {

            do
            {

                if (String.IsNullOrEmpty(email))
                {
                    Console.WriteLine("You have to enter new email");
                    email = Console.ReadLine();
                    if (email == "q" || email == "Q")
                    {
                        return "-2";
                    }
                }
            } while (string.IsNullOrEmpty(email));

            if (!email.Contains("@") || !email.Contains("."))
            {
                do
                {
                    Console.WriteLine("That doesn't look like an email. Try again.");
                    email = Console.ReadLine();
                    if (email == "q" || email == "Q")
                    {
                        return "-2";
                    }
                } while (!email.Contains("@") || !email.Contains("."));

            }

            bool emailUsed = IfMailAlreadyExists(email);
            if (emailUsed)
            {

                Console.WriteLine("\nThere is already an account with this email.");
                return "-1";

            }

            return email;

        }










        public static bool ValidPassword(string password)
        {
           

            if (password.Length < 8)
            {
                Console.WriteLine("\nToo short");
                return false;
                
            }

            else if (password.Any(char.IsSymbol) == false && password.Any(char.IsUpper) == false)
            {
                Console.WriteLine("Symbol and big letter requierd");
                return false;

            }
            else if (password.Any(char.IsSymbol) == false)
            {
                Console.WriteLine("\nSymbol requierd");
                return false;

            }

            else if (password.Any(char.IsUpper) == false)
            {
                Console.WriteLine("\nBig letter requierd");
                return false;

            }

            else
            {
                return true;
            }

        }




    }


}