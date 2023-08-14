using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
namespace Fries_MinMax.Source_Files
{
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

        public static implicit operator string(Command __C)
        {
            return __C.c;
        }
    }

    /*public class State
    {
        public Hand p1 = new Hand(1, 1);
        public Hand p2 = new Hand(1, 1);
        public bool p1turn = true;

        public List<string> initPaths()
        {
            var _p1 = p1turn ? p1 : p2;
            var _p2 = p1turn ? p2 : p1;

            var lst = new List<string>();
            for (int l = 0; l < 5; ++l)
            {
                for (int r = 0; r < 5; ++r)
                {
                    if (_p1.isValidCommand($"s{l}{r}", _p2) && !lst.Contains($"s{r}{l}"))
                    {
                        lst.Add($"s{l}{r}");
                    }
                }
            }

            foreach (char l in new char[]{ 'l', 'r' })
            {
                foreach (char r in new char[] { 'l', 'r' })
                {
                    if (_p1.isValidCommand($"a{l}{r}", _p2))
                    {
                        lst.Add($"a{l}{r}");
                    }
                }
            }
            return lst;
        }
    }*/


    public class MinMax
    {

    }
}
