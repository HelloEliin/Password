using System;
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
                CreateNewUser();
            }

            if (!email.Contains("@") || !email.Contains(".") || string.IsNullOrEmpty(email))
            {
                Console.WriteLine("\nInvalid emailadress");
                CreateNewUser();
            }


            Console.WriteLine("ENTER A PASSWORD. (MIN 8 LETTERS, 1 BIG LETTER AND ONE SYMBOL)");
            var password = Console.ReadLine();
            if(password.Length < 8)
            {
                Console.WriteLine("\nToo short");
                CreateNewUser();
            }
            if (!password.Any(char.IsUpper));
            {
                Console.WriteLine("\nAtleast one big letter requierd");
                CreateNewUser();
            }

            if (!password.Any(char.IsSymbol))
            {
                Console.WriteLine("\nAtleast one symbol");
                CreateNewUser();
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
                Console.WriteLine("[" + whichIndex + "]\n");
                Console.WriteLine("NAME");
                Console.WriteLine(user.FirstName + " " + user.LastName + "\n");
                Console.WriteLine("EMAIL");
                Console.WriteLine(user.Email + "\n");
                Console.WriteLine("USERNAME");
                Console.WriteLine(user.UserName + "\n");
                Console.WriteLine("PASSWORD");
                Console.WriteLine(user.Password + "\n");
                Console.WriteLine("ACCESSLEVEL");
                Console.WriteLine("ADMIN : " + user.AccessLevelAdm);
                Console.WriteLine("MODERATOR : " + user.AccessLevelMod);
                Console.WriteLine("USER: " + user.AccessLevelOne + "\n\n\n");
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



    }


}