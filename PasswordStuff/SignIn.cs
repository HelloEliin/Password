using System.Text.Json.Serialization;

namespace PasswordStuff
{
    internal partial class Program
    {
        public class SignIn
        {

            public static int SignInNow()
            {
                var json = CreateUserFile.GetJson();

                Console.WriteLine("\n\nUSERNAME:");
                var username = Console.ReadLine();
                Console.WriteLine("\n\nPASSWORD:");
                var password = Console.ReadLine();

                for (int i = 0; i < json.Count; i++)
                {
                    if (json[i].UserName == username)
                    {
                        if (json[i].Password == password)
                        {
                            return i;
                        }

                    }

                }

                return -1;



            }





        }
    }
}