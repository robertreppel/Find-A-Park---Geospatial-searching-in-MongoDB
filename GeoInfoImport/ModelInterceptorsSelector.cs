using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Castle.Core;
using Castle.DynamicProxy;
using Castle.MicroKernel.Proxy;
using EventSource;

namespace GeoInfoImport
{
    class ModelInterceptorsSelector : IModelInterceptorsSelector
    {
        public bool HasInterceptors(ComponentModel model)
        {
            if (model.Implementation != null)
                if (model.Implementation.Namespace != null)
                    return model.Implementation.Namespace.Equals("GeoData");
            return false;
        }

        public InterceptorReference[] SelectInterceptors(ComponentModel model, InterceptorReference[] interceptors)
        {
            Assembly asm = Assembly.Load("EventSource");
            Type ti = typeof(IInterceptor);
            var interceptorReferences = new List<InterceptorReference>(); //[] {};

            foreach (Type t in asm.GetTypes())
            {
                if (ti.IsAssignableFrom(t))
                {
                    var newInterceptor = InterceptorReference.ForType(t);
                    interceptorReferences.Add(newInterceptor);
                }
            }

            return interceptorReferences.ToArray();
        }
    }
}
