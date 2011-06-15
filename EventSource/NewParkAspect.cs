using System;
using Castle.DynamicProxy;
using GeoData;

namespace EventSource
{
    public class NewParkAspect : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            if(invocation.Method.Name.Equals("Save"))
            {
                var argument = invocation.GetArgumentValue(0);
                var park = argument as Park;
                if (park != null)
                {
                    Console.WriteLine("Intercepted park {0} event.", park.Name);
                }              
            }
            invocation.Proceed();
        }
    }
}