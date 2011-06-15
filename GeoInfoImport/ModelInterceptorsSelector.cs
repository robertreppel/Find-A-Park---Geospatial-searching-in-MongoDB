using System.Reflection;
using Castle.Core;
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
                    return typeof(EventSourceAspect) != model.Implementation &&
                           model.Implementation.Namespace.StartsWith("GeoInfoImport");
            return false;
        }

        public InterceptorReference[] SelectInterceptors(ComponentModel model, InterceptorReference[] interceptors)
        {
            return new[] { InterceptorReference.ForType<EventSourceAspect>() };
        }
    }
}
