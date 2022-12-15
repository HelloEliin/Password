using System.Text.Json.Serialization;

namespace PasswordStuff
{
        public class SignIn
        {

            public static int SignInNow()
            {
                var json = CreateUserFile.GetJson();

                Console.WriteLine("\n\nUSERNAME OR TYPE '10' TO QUIT");
                var username = Console.ReadLine();

               if(username == "10" || username == "10")
               {
                return -10;
                }

                if(string.IsNullOrWhiteSpace(username))
                {
                    return -1;

            }

            Console.WriteLine("\n\nPASSWORD OR TYPE '10' TO QUIT");
            var password = CreateUser.ReadPassword();

            if (password == "10")
            {
                return -10;
            }
            if (string.IsNullOrWhiteSpace(password))
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
