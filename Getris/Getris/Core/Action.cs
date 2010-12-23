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
    }

    public class Move : Action
    {
        public Move(string data)
            : base(data)
        {
        }
    }
    public class GoTo : Action
    {
        public GoTo(string data)
            : base(data)
        {
        }
    }
    public class Rotate : Action
    {
        public Rotate(string data)
            : base(data)
        {
        }
    }
    public class Chat : Action
    {
        public Chat(string data)
            : base(data)
        {
        }
    }
    public class Turn : Action
    {
        public Turn(string data)
            : base(data)
        {
        }
    }
    public class Status : Action
    {
        public Status(string data)
            : base(data)
        {
        }
    }
    public class Attack : Action
    {
        public Attack(string data)
            : base(data)
        {
        }
    }
}
