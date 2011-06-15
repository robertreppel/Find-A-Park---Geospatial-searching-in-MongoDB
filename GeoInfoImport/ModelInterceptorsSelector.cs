using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using Castle.Core;
using Castle.DynamicProxy;
using Castle.MicroKernel.Proxy;

namespace GeoInfoImport
{
    class ModelInterceptorsSelector : IModelInterceptorsSelector
    {
        public bool HasInterceptors(ComponentModel model)
        {
            var interceptMethodInvocationsInNamespace =
                ConfigurationManager.AppSettings["InterceptMethodInvocationsInNamespace"];
            if (model.Implementation != null)
                if (model.Implementation.Namespace != null)
                    return model.Implementation.Namespace.Equals(interceptMethodInvocationsInNamespace);
            return false;
        }

        public InterceptorReference[] SelectInterceptors(ComponentModel model, InterceptorReference[] interceptors)
        {
            string assemblyContainingInterceptors = ConfigurationManager.AppSettings["AssemblyContainingInterceptors"];
            Assembly asm = Assembly.Load(assemblyContainingInterceptors);
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
