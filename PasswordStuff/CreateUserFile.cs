using System.Text.Json;


namespace PasswordStuff
{
    public class CreateUserFile
    {

            private static string _currentDi = Environment.CurrentDirectory;
            private static string _path = Directory.GetParent(_currentDi).Parent.Parent.FullName + @"\UserFile.json";


            public void CreateFile()
            {
                if (!File.Exists(_path))
                {
                    using (var fs = File.Create(_path)) { }
                    File.WriteAllText(_path, "[]");


                }

            }


            public static void UpDate(List<CreateUser> lists)
            {
                var jsondata = JsonSerializer.Serialize(lists);
                File.WriteAllText(_path, jsondata);
            }

            public static List<CreateUser> GetJson()
            {
                var jsondata = File.ReadAllText(_path);
                var lists = JsonSerializer.Deserialize<List<CreateUser>>(jsondata);

                return lists;
            }
        }
}