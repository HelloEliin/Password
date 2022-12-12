using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
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

            Console.WriteLine("ENTER USERNAME");
            var userName = Console.ReadLine();

            Console.WriteLine("ENTER YOUR FIRSTNAME");
            var firstName = Console.ReadLine();
            Console.WriteLine("ENTER YOUR LASTNAME");
            var lastName = Console.ReadLine();
            Console.WriteLine("ENTER YOUR EMAIL");
            var email = Console.ReadLine();
            Console.WriteLine("ENTER A PASSWORD");
            var password = Console.ReadLine();

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
                json[user].FirstName+ " " + json[user].LastName + "\n\n" +
                "MY EMAIL\n" +
                json[user].Email + "\n\n" +
                "MY USERNAME\n" +
                json[user].UserName + "\n\n" +
                "MY PASSWORD \n" +
                json[user].Password);

            var choice = Console.ReadLine().ToLower();

            if(choice == "q")
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
            Console.WriteLine("Promoting");
        }


        public static void DemoteUser()
        {
            Console.WriteLine("Demoting.");
        }


        public static void DeleteUser()
        {
            Console.WriteLine("Deleting.");
        }


        public static void ShowAllUsers()
        {
            var json = CreateUserFile.GetJson();
            int whichIndex = 0;

            foreach (var user in json)
            {
                Console.WriteLine("[" + whichIndex + "]\n");
                Console.WriteLine("NAME");
                Console.WriteLine(user.FirstName+ " " + user.LastName + "\n");
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

            json[num].FirstName= newFirstName;
            json[num].LastName= newLastName;    

            CreateUserFile.UpDate(json);


        }


        public static void ChangeEmail()
        {

        }








    }
}