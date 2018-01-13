using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Auctus.Util
{
    public class Retry
    {
        private readonly int attempts;
        private readonly int milissecondsDelay;

        private Retry(int attempts, int milissecondsDelay)
        {
            this.attempts = attempts;
            this.milissecondsDelay = milissecondsDelay;
        }

        public static Retry Get(int attempts = 5, int milissecondsDelay = 3000)
        {
            return new Retry(attempts, milissecondsDelay);
        }

        public T Execute<T>(Delegate operation, params object[] param)
        {
            var exceptions = new List<Exception>();
            for (int attempt  = 0; attempt < attempts; attempt++)
            {
                try
                {
                    if (attempt > 0)
                        Thread.Sleep(milissecondsDelay);

                    var result = operation.DynamicInvoke(param);
                    return (T)ChangeType(result, typeof(T));
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }
            throw new AggregateException(exceptions);
        }

        private static object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return Convert.ChangeType(value, t);
        }
    }
}
