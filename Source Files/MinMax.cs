using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using System.Linq;


namespace Fries_MinMax.Source_Files
{
    public static class CONST
    {
        public const uint MAX = 10;
        public const uint DEPTH = 3;
    }

    /*
     * Commands follow the following formal: /^(?<type>[as])(?:(?<p>[lr])(?<o>[lr])|(?<l>\d+)-(?<r>\d+))$/
     * Ex: splitting <4, 1> to <2, 3> is s23
     * Naturally, s2-3 == s3-2, however arl != alr
     * 
     * Unlike past MinMax implementations,
     * storing a path can be done using a starting state and a set of commands,
     * saving ungodly quantities of memory
    */
    public enum COMMAND
    {
        SPLIT = 's',
        ATTACK = 'a',
        LEFT = 'l',
        RIGHT = 'r',
        NONE = -1,
    }

    public struct Command
    {
        public static Regex rx = new Regex(@"^([as])(?:([lr])([lr])|([0-9]+)\-([0-9]+))$");


        private string c;
        public COMMAND type { get; set; }
        public COMMAND p { get; set; }
        public COMMAND o { get; set; }
        public uint l { get; set; }
        public uint r { get; set; }

        public Command(string cmd)
        {
            c = cmd;
            GroupCollection groups = rx.Match(cmd).Groups;
            type = (COMMAND)Convert.ToChar(groups[1].Value);
            if (type == COMMAND.ATTACK){
                p = (COMMAND)Convert.ToChar(groups[2].Value);
                o = (COMMAND)Convert.ToChar(groups[3].Value);
                l = 0;
                r = 0;
            }
            else
            {
                p = COMMAND.NONE;
                o = COMMAND.NONE;
                l = Convert.ToUInt32(groups[4].Value);
                r = Convert.ToUInt32(groups[5].Value);
            }
        }

        public static bool operator ==(Command l, Command r)
        {
            return l.type == r.type && ((l.p == r.p && l.o == r.o && l.type == COMMAND.ATTACK) || (new Hand(l.l, l.r) == new Hand(r.l, r.r) && l.type == COMMAND.SPLIT));
        }
        public static bool operator !=(Command l, Command r)
        {
            return !(l == r);
        }

        public static implicit operator string(Command __C)
        {
            return __C.c;
        }
    }

    public class Turn
    {
        public Hand p1 = new Hand(1, 1);
        public Hand p2 = new Hand(1, 1);

        public void next() 
        {
            var temp = p2;
            p2 = p1;
            p1 = temp;
        }
        public List<Command> validCommands()
        {
            var ret = new List<Command>();
            foreach (char p in new char[] { 'l', 'r' })
            {
                foreach (char o in new char[] { 'l', 'r' })
                {
                    var c = new Command($"a{p}{o}");
                    if (p1.isValidCommand(c, p2)) { ret.Add(c); };
                }
            }
            for (uint l = 0; l < CONST.MAX; ++l)
            {
                for (uint r = 0; r < CONST.MAX; ++r)
                {
                    var c = new Command($"s{l}-{r}");
                    //if (l == 0 && r == 0) { Console.WriteLine("hi"); }
                    if (ret.Any(cmd => cmd == c)) { continue; }
                    if (p1.isValidCommand(c, p2)) { ret.Add(c); }
                }
            }
            return ret;
        }
        public void commands(List<Command> cmds)
        {
            foreach (Command cmd in cmds)
            {
                p1.command(cmd, ref p2);
                next();
            }
        }
        public void command(Command cmd)
        {
            p1.command(cmd, ref p2);
        }
        public bool won()
        {
            var win = new Hand(0, 0);
            return p1 == win || p2 == win;
        }

        public Turn clone()
        {
            var t = new Turn();
            t.p1 = p1;
            t.p2 = p2;
            return t;
        }

        public static implicit operator string(Turn p)
        {
            return $"\n{(string)p.p2}\n{(string)p.p1}\n";
        }
    }
    
    public struct Node
    {
        public List<Command> path;
        public Turn p;
        public List<Node> next;
        public double score
        {
            get
            {
                if (next.Count == 0)
                {
                    //Console.WriteLine((p.p1.score() - p.p2.score()) * Math.Pow(-1, path.Count % 2));

                    return (p.p1.score() - p.p2.score()) * Math.Pow(-1, path.Count % 2);
                }
                return next.Sum(n => n.score);
            }
        }

        public Node(List<Command> lst, Turn _p, int depth = 1)
        {
            path = lst;
            p = _p.clone();
            p.commands(lst);
            //Console.WriteLine((string)p);
            var valid = p.validCommands();
            next = new List<Node>();
            if (depth >= CONST.DEPTH) { return; }
            foreach (Command c in valid)
            {
                next.Add(new Node(lst.Append(c).ToList(), _p, depth + 1));
            }
            
        }
    }

    public class Minimax
    {
        public Turn p;

        public static Command next(Node tree)
        {
            var m = max(tree);
            m.path.ForEach(cmd => Console.Write($"{(string)cmd}, "));
            Console.WriteLine(m.score);
            return m.path[0];
        }

        private static Node max(Node n)
        {
            if (n.next.Count == 0)
            {
                return n;
            }
            var r = n.next[0];
            for (int i = 1; i < n.next.Count; ++i)
            {
                if (n.next[i].score > r.score) { r = n.next[i]; }
            }
            return max(r);
        }
    }
}
