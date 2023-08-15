using System;
using System.Collections.Generic;

namespace Fries_MinMax.Source_Files
{
    public interface IVal
    {
        uint val { get; set; }
    }

    public struct LeftVal : IVal
    {
        public LeftVal(uint _val)
        {
            val = _val;
        }

        public uint val { get; set; }

        public static LeftVal operator +(LeftVal l, IVal r) { return new LeftVal((l.val + r.val) % CONST.MAX); }
        public static LeftVal operator -(LeftVal l, IVal r) { return new LeftVal((l.val - r.val) % CONST.MAX); }
        public static bool operator ==(LeftVal l, IVal r) { return l.val == r.val; }
        public static bool operator !=(LeftVal l, IVal r) { return l.val != r.val; }

        public static implicit operator string(LeftVal h)
        {
            return h.val.ToString();
        }
    }

    public struct RightVal : IVal
    {
        public RightVal(uint _val)
        {
            val = _val;
        }

        public uint val { get; set; }

        public static RightVal operator +(RightVal l, IVal r) { return new RightVal((l.val + r.val) % CONST.MAX); }
        public static RightVal operator -(RightVal l, IVal r) { return new RightVal((l.val - r.val) % CONST.MAX); }
        public static bool operator ==(RightVal l, IVal r) { return l.val == r.val; }
        public static bool operator !=(RightVal l, IVal r) { return l.val != r.val; }

        public static implicit operator string(RightVal h)
        {
            return h.val.ToString();
        }
    }

    public struct Hand
    {
        public Hand(uint _l = 1, uint _r = 1)
        {
            l = new LeftVal(_l);
            r = new RightVal(_r);
        }


        public LeftVal l;
        public RightVal r;

        public bool isValidSplit(Hand _h)
        {
            return (_h != this) && (_h.r.val + _h.l.val == l.val + r.val || (_h.r.val + _h.l.val == 0 && l.val + r.val == CONST.MAX)) && l.val + r.val != 0;
        }
        public bool isValidSplit(uint l, uint r)
        {
            return isValidSplit(new Hand(l, r));
        }
        public bool isValidSplit(LeftVal l, LeftVal r)
        {
            return isValidSplit(l.val, r.val);
        }

        public bool isValidCommand(Command cmd, Hand o)
        {
            if (cmd.type == COMMAND.ATTACK)
            {
                if (cmd.p == COMMAND.LEFT)
                {
                    if (l.val == 0) { return false; }
                }
                else if (r.val == 0) { return false; }

                if (cmd.o == COMMAND.LEFT)
                {
                    if (o.l.val == 0) { return false; }
                }
                else if (o.r.val == 0) { return false; }
            }
            else
            {
                return isValidSplit(cmd.l, cmd.r);
            }
            return true;
        }

        public void split(Hand _h)
        {
            if (isValidSplit(_h))
            {
                l = _h.l;
                r = _h.r;
            }
        }
        public void split(uint l, uint r)
        {
            split(new Hand(l, r));
        }
        public void split(Command cmd)
        {
            split(cmd.l, cmd.r);
        }

        public void command(Command command, ref Hand o)
        {
            if (isValidCommand(command, o))
            {
                if (command.type == COMMAND.SPLIT)
                {
                    split(command);
                }
                else
                {
                    uint a = command.p == COMMAND.LEFT ? l.val : r.val;
                    if (command.o == COMMAND.LEFT) { o += new LeftVal(a); }
                    else { o += new RightVal(a); }
                }
            }
            else { throw new InvalidOperationException(); }
        }
        public void commands(List<Command> cmds, ref Hand o)
        {
            foreach (Command cmd in cmds)
            {
                command(cmd, ref o);
            }
        }
        public static void commands(List<Command> cmds, ref Turn p)
        {
            foreach (Command cmd in cmds)
            {
                p.p1.command(cmd, ref p.p2);
                p.next();
            }
        }

        public double score()
        {
            if (this == new Hand(0, 0)) { return 1; }
            if (this == new Hand( 1, 0)){ return -0.5; }
            if ((l + r).val == 0) { return 0.5; }
            return 0;
        }

        public static bool operator ==(Hand l, Hand r)
        {
            return (l.l == r.l && l.r == r.r) || (l.r == r.l && l.l == r.r);
        }
        public static bool operator !=(Hand l, Hand r)
        {
            return !(l == r);
        }

        public static Hand operator +(Hand h, LeftVal v)
        {
            return new Hand((h.l + v).val, h.r.val);
        }
        public static Hand operator -(Hand h, LeftVal v)
        {
            return new Hand((h.l - v).val, h.r.val);
        }

        public static Hand operator +(Hand h, RightVal v)
        {
            return new Hand(h.l.val, (h.r + v).val);
        }
        public static Hand operator -(Hand h, RightVal v)
        {
            return new Hand(h.l.val, (h.r - v).val);
        }

        public static implicit operator string(Hand h)
        {
            return $"<{(string)h.l}, {(string)h.r}>";
        }
    }
}