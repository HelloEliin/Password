using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
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
                    Console.WriteLine("You have to choose username");
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
                Console.WriteLine("\nSomeone else already uses this username");
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

            string theEmail = IsValidEmail(email);

            if (theEmail == "-1")
            {
                do
                {
                    Console.WriteLine("ENTER NEW EMAIL");
                    email = Console.ReadLine();
                    theEmail = IsValidEmail(email);

                } while (theEmail == "-1");
            }

            if (theEmail == "-2")
            {
                return;
            }

            email = theEmail;



            Console.WriteLine("\nENTER A PASSWORD. (MIN 8 LETTERS, 1 BIG LETTER AND ONE SYMBOL)");
            var password = ReadPassword();

            if (password == "q" || password == "Q")
            {
                return;
            }
            bool validOrNot = ValidPassword(password);
            if (!validOrNot)
            {
                do
                {
                    Console.WriteLine("\nENTER NEW PASSWORD");
                    password = ReadPassword();
                    validOrNot = ValidPassword(password);
                    if (password == "q" || password == "Q")
                    {
                        return;
                    }

                } while (!validOrNot);
            }


            Console.WriteLine("\n\n\nYAY! ACCOUNT CREATED, YOU CAN NOW SIGN IN!");
    

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
            int user = SelectUser();
            if (user == -1)
            {
                return;
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

                json[user].AccessLevelMod = true;
                json[user].AccessLevelAdm = false;
                json[user].AccessLevelOne = false;

            }

            if (whatAccess == "2")
            {
                json[user].AccessLevelMod = false;
                json[user].AccessLevelAdm = true;
                json[user].AccessLevelOne = false;

            }

            Console.WriteLine("\n\nUSER IS PROMOTED");
            CreateUserFile.UpDate(json);

        }

    



    public static void DemoteUser()
        {
            var json = CreateUserFile.GetJson();
            ShowAllUsers();
            int user = SelectUser();
            if (user == -1)
            {
                return;
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

                json[user].AccessLevelOne = true;
                json[user].AccessLevelAdm = false;
                json[user].AccessLevelMod = false;

            }

            if (whatAccess == "2")
            {
                json[user].AccessLevelAdm = false;
                json[user].AccessLevelMod = true;
                json[user].AccessLevelOne = false;

            }

            Console.WriteLine("\n\nUSER IS DEMOTED");
            CreateUserFile.UpDate(json);

        }


        public static void DeleteUser()
        {
            var json = CreateUserFile.GetJson();
            ShowAllUsers();
            int user = SelectUser();
            if (user == -1)
            {
                return;
            }
            Console.WriteLine("DO YOU WANT DO DELETE THIS USER? Y/N");
            var yesOrNo = Console.ReadLine().ToLower();

            if (yesOrNo == "y")
            {
                json.RemoveAt(user);
                Console.WriteLine("\n\nUSER IS DELETED");

            }
            else
            {
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
            int user = SelectUser();
            if (user == -1)
            {
                return;
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


            json[user].UserName = newUserName;
            CreateUserFile.UpDate(json);


             
        }


        public static void ChangeUsersPassword()
        {
            var json = CreateUserFile.GetJson();
            int user = SelectUser();
            if (user == -1)
            {
                return;
            }

            Console.WriteLine("\nENTER A PASSWORD. (MIN 8 LETTERS, 1 BIG LETTER AND ONE SYMBOL)");
            var newPassword = ReadPassword();

            if (newPassword == "q" || newPassword == "Q")
            {
                return;
            }
            bool isValid = ValidPassword(newPassword);


            bool validOrNot = ValidPassword(newPassword);
            if (!validOrNot)
            {
                do
                {
                    Console.WriteLine("\nENTER NEW PASSWORD");
                    newPassword = ReadPassword();
                    validOrNot = ValidPassword(newPassword);
                    if (newPassword == "q" || newPassword == "Q")
                    {
                        return;
                    }

                } while (!validOrNot);
            }

            Console.WriteLine("\n\nPASSWORD CHANGED");

            json[user].Password = newPassword;
            CreateUserFile.UpDate(json);
        }





        public static void ChangeUsersName()
        {
            var user = SelectUser();
            if(user == -1)
            {
                return;
            }
        }






        public static void ChangeUsersEmail()
        {
            var json = CreateUserFile.GetJson();
            ShowAllUsers();
            int user = SelectUser();
            if (user == -1)
            {
                return;
            }
            ChangeEmail(user);
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
            if (String.IsNullOrEmpty(email))
            {
                do
                {

                    Console.WriteLine("You have to enter new email");
                    email = Console.ReadLine();
                    if (email == "q" || email == "Q")
                    {
                        return "-2";
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

    



        public static void EditOwnProfile(int userIndex)
        {
            var json = CreateUserFile.GetJson();
            Console.WriteLine("\n\nPRESS ENTER TO GO BACK TO MENU" +
    "\n\n\nMY NAME\n" +
    json[userIndex].FirstName + " " + json[userIndex].LastName + "\n\n" +
    "MY EMAIL\n" +
    json[userIndex].Email + "\n\n" +
    "MY USERNAME\n" +
    json[userIndex].UserName + "\n\n" +
    "MY PASSWORD \n" +
    json[userIndex].Password);

            Console.WriteLine("\n\n[1] EDIT NAME\n" +
                "[2] EDIT PASSWORD\n" +
                "[3] EDIT EMAIL\n");
            var choice = Console.ReadLine();
            if (string.IsNullOrEmpty(choice))
            {
                return;
            }

            switch (choice)
            {
                case "1": CreateUser.ChangeOwnName(userIndex);
                    break;
                case "2":
                    CreateUser.ChangeOwnPassword(userIndex);
                break;
               case "3":
                    CreateUser.ChangeEmail(userIndex);

                    break;
                default: Console.WriteLine("Try again");
                    break;
            }

            return;
        }







        public static void ChangeOwnName(int userIndex)

        {
            var json = CreateUserFile.GetJson();
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

            json[userIndex].FirstName = newFirstName;
            json[userIndex].LastName = newLastName;

            CreateUserFile.UpDate(json);

        }



        public static int SelectUser()
        {
            var json = CreateUserFile.GetJson();
            ShowAllUsers();
            Console.WriteLine("SELECT USER ABOVE (OR PRESS 'Q TO QUIT)");
            var choice = Console.ReadLine();

            if (choice == "q" || choice == "Q")
            {
                return -1;
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
                            return -1;
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
                        return -1;
                    }
                    valid = int.TryParse(choice, out num);

                } while (num > (json.Count - 1) || num < 0);
            }

            return num;
        }


        public static void ChangeOwnPassword(int userIndex)
        {
            var json = CreateUserFile.GetJson();
            Console.WriteLine("\nENTER A PASSWORD. (MIN 8 LETTERS, 1 BIG LETTER AND ONE SYMBOL. PRESS 'Q' TO QUIT)");
            var newPassword = ReadPassword();

            if (newPassword == "q" || newPassword == "Q")
            {
                return;
            }
            bool isValid = ValidPassword(newPassword);


            bool validOrNot = ValidPassword(newPassword);
            if (!validOrNot)
            {
                do
                {
                    Console.WriteLine("\nENTER NEW PASSWORD");
                    newPassword = ReadPassword();
                    validOrNot = ValidPassword(newPassword);
                    if (newPassword == "q" || newPassword == "Q")
                    {
                        return;
                    }

                } while (!validOrNot);
            }

            Console.WriteLine("\n\nPASSWORD CHANGED");

            json[userIndex].Password = newPassword;
            CreateUserFile.UpDate(json);
        }




        public static void ChangeEmail(int user)
        {
            var json = CreateUserFile.GetJson();
            Console.WriteLine("ENTER NEW EMAIL");
            var newEmail = Console.ReadLine();

            string theEmail = IsValidEmail(newEmail);

            if (theEmail == "-1")
            {
                do
                {
                    Console.WriteLine("ENTER NEW EMAIL");
                    newEmail = Console.ReadLine();
                    theEmail = IsValidEmail(newEmail);

                } while (theEmail == "-1");
            }

            if (theEmail == "-2")
            {
                return;
            }

            newEmail = theEmail;

            Console.WriteLine("\n\nEMAIL CHANGED");
            json[user].Email = newEmail;


            CreateUserFile.UpDate(json);
        }




       




    }


}