using System;
using Castle.DynamicProxy;

namespace EventSource
{
    public class EventSourceAspect : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            try {
                Console.WriteLine("Intercepted event.");
                invocation.Proceed();
            }
            catch (Exception e)
            {
                Console.WriteLine("Rollback");
                throw;
            }
        }
    }
}