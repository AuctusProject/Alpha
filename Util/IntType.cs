using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.Util
{
    public class IntType
    {
        public int Value { get; protected set; }

        protected IntType(int value)
        {
            Value = value;
        }

        public static bool operator ==(IntType obj1, IntType obj2)
        {
            return !object.ReferenceEquals(obj1, null) && !object.ReferenceEquals(obj2, null) && obj1.Value == obj2.Value;
        }
        
        public static bool operator !=(IntType obj1, IntType obj2)
        {
            return !(obj1 == obj2);
        }

        public override bool Equals(object obj)
        {
            if (obj is int)
                return this.Value.Equals((int)obj);
            else if (obj is IntType)
                return this.Value.Equals(((IntType)obj).Value);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }
    }
}
