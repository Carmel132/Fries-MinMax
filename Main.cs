using System;
using System.Collections.Generic;
using System.Linq;
using Fries_MinMax.Source_Files;

namespace Fries_MinMax
{
    public static class Fries
    {
        public static void Main()
        {
            var p = new Turn();
            bool ai = true;
            p.p1 = new Hand(1, 1);
            p.p2 = new Hand(1, 1);
            //Console.WriteLine(new Command("s2-3") == new Command("s0-0")) ;
            //Console.WriteLine(p.validCommands().Contains(new Command("s0-0")));
            p.validCommands().ForEach(c => Console.Write($"{(string)c}, "));

            while (!p.won())
            {
                if (ai)
                {
                    var n = new Node(new List<Command> { }, p);

                    n.next.ForEach(c => Console.Write($"({(string)c.path.Last()} : {c.score})"));

                    var cmd = Minimax.next(n);
                    Console.WriteLine((string)cmd);
                    p.command(cmd);
                    p.next();
                    ai = false;
                }
                else
                {
                    Console.WriteLine((string)p);

                    Command cmd = new Command(Console.ReadLine());
                    p.command(cmd); 
                    p.next();
                    ai = true;
                }
            }
            Console.ReadLine();
        }
    }
}
