using Everest.CodingChallenge.CourierService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everest.CodingChallenge.CourierService.Services
{
    /// <summary>
    /// Class to handle console based I/O operations
    /// </summary>
    public class IOConsoleOperations : IIOServiceOperations
    {
        public string ReadLine()
        {
            return Console.ReadLine() ?? string.Empty;
        }

        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}
