using System.Text.Json.Serialization;

namespace PasswordStuff
{
        public class SignIn
        {

            public static int SignInNow()
            {
                var json = CreateUserFile.GetJson();

                Console.WriteLine("\n\nUSERNAME:");
                var username = Console.ReadLine();
                if(string.IsNullOrEmpty(username))
                {
                    return -1;
                    
                }

                Console.WriteLine("\n\nPASSWORD:");
                var password = CreateUser.ReadPassword();
                if (string.IsNullOrEmpty(password))
                {
                    return -1;
                }


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
