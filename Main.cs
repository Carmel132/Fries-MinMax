using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Fries_MinMax.Source_Files;

namespace Fries_MinMax
{
    public static class Fries
    {
        public static void Main()
        {
            var a = new Hand(1, 4);
            var b = new Hand(2, 4);
            var c = new Command("alr");
            a.command(c, ref b);
            Console.WriteLine((string)b);
            Console.ReadLine();
        }

    }
}
