using Mic.Logger;
using Mic.Repository;
using Mic.Repository.Repositories;
using Mic.Repository.Utils;

namespace Test
{
    class Program
    {
        private static AdminRepository userRepository = ClassInstance<AdminRepository>.Instance;
        static void Main(string[] args)
        {//MySqlConnectionString

            Duration duration = new ByShell32();
            var result = duration.GetDuration($@"E:\4229759733.mp3");
        }
    }
}
