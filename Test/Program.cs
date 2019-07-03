using Mic.Repository;
using Mic.Repository.IRepositories;
using Mic.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        private static IUserRepository userRepository;
        static void Main(string[] args)
        {//MySqlConnectionString
            userRepository = new UserRepository();
            var a = userRepository.GetAll();
            var b = userRepository.GetById(1);
            var c = userRepository.GetById(2);
            System.Console.WriteLine("OK");
            System.Console.ReadKey();
        }
    }
}
