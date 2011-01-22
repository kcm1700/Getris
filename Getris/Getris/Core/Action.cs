using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace getris.Core
{
    public abstract class Action
    {
        public readonly string data;
        public Action(string data)
        {
            this.data = data;
        }
        virtual public bool IsValid()
        {
            return false;
        }
    }

    public class Move : Action
    {
        public Move(string data)
            : base(data)
        {
        }
        override public bool IsValid()
        {
            switch (data)
            {
                case "left":
                case "right":
                case "drop":
                case "down":
                    return true;
                default:
                    return false;
            }
        }
    }
    public class GoTo : Action
    {
        public GoTo(string data)
            : base(data)
        {
        }
        override public bool IsValid()
        {
            string[] str = data.Split(':');
            if (str.Length != 3)
                return false;
            if (str[0] != "left" && str[0] != "right")
                return false;
            if (str[1] != Convert.ToString(Convert.ToByte(str[1])))
                return false;
            if (str[2] != Convert.ToString(Convert.ToByte(str[2])))
                return false;
            return true;
        }

    }
    public class Rotate : Action
    {
        public Rotate(string data)
            : base(data)
        {
        }
        override public bool IsValid()
        {
            switch (data)
            {
                case "cw":
                case "ccw":
                case "left:cw":
                case "left:ccw":
                case "right:cw":
                case "right:ccw":
                    return true;
                default:
                    return false;
            }
        }
    }
    public class Chat : Action
    {
        public Chat(string data)
            : base(data)
        {
        }
        override public bool IsValid()
        {
            return true;
        }
    }
    public class Turn : Action
    {
        public Turn(string data)
            : base(data)
        {
        }
        override public bool IsValid()
        {
            switch (data)
            {
                case "left":
                case "right":
                    return true;
                default:
                    return false;
            }
        }
    }
    public class Status : Action
    {
        public Status(string data)
            : base(data)
        {
        }
        override public bool IsValid()
        {
            return true;
        }
    }
    public class Attack : Action
    {
        public Attack(string data)
            : base(data)
        {
        }
        override public bool IsValid()
        {
            return true;
        }
    }
    public class NullAction : Action
    {
        public NullAction(string data="")
            : base(data)
        {
        }
        override public bool IsValid()
        {
            if (data == "")
                return true;
            else
                return false;
        }
    }
}
