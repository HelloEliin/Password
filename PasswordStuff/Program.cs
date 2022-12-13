using System.Security.Cryptography.X509Certificates;

namespace PasswordStuff
{
    internal partial class Program
    {
        static void Main(string[] args)
        {

            bool isRunning = true;
            var json = new CreateUserFile();
            json.CreateFile();

            while (isRunning)
            {

                Menus.FrontPageMenu();
                var choice = Console.ReadLine().ToLower();

                switch (choice)
                {
                    case "s": int userIndex = SignIn.SignInNow();
                        if (userIndex == -1)
                        {
                            Console.WriteLine("\n\nWrong username or password.");
                            
                        }
                        else if(userIndex == -10)
                        {
                            break;
                        }

                        else {
                            Menus.UserSystemMenu(userIndex);
                        }
                        
                        break;
                    case "c": CreateUser.CreateNewUser();
                        break;
                    case "q":
                        isRunning= false;
                        break;

                    default:
                        Console.WriteLine("\nChoose option.");
                        break;
                }
            }
            

        }
    }
}