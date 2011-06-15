using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using GeoData;

namespace EventSource
{

    public class NewPlaceAspect : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            if (invocation.Arguments.Length > 0)
            {

                var argument = invocation.GetArgumentValue(0);
                var place = argument as Geoname;
                if (place != null)
                {
                    Console.WriteLine("New place: {0}.", place.Name);
                }

            }
            invocation.Proceed();
        }
    }
}
