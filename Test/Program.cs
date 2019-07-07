using Mic.Logger;
using Mic.Repository;
using Mic.Repository.Repositories;


namespace Test
{
    class Program
    {
        private static AdminRepository userRepository = ClassInstance<AdminRepository>.Instance;
        static void Main(string[] args)
        {//MySqlConnectionString
            userRepository = new AdminRepository();
            LoggerProvider.Logger.Error("与读卡器的连接遇到问题, 15秒后重试");
            var a = userRepository.GetAll();
            var b = userRepository.GetById(1);
            var c = userRepository.GetById(2);
            System.Console.WriteLine("OK");
            System.Console.ReadKey();
        }
    }
}
