using Pharos.Common.ViewCenter;
using Pharos.Framework;

namespace Pharos.Extensions.Mediation
{
    public class MediatorMapExtension : IExtension
    {
        public void Enable(IContext context)
        {
            var injector = context.Injector;
            injector.Map<ViewRegistry>().ToValue(ViewRegistry.Instance);
            injector.Map<IMediatorMap>().ToSingleton<MediatorMap>();
            context.Initializing += OnContextInitializing;
            context.Destroying += OnContextDestroying;
        }

        public void Disable(IContext context)
        {
            context.Initializing -= OnContextInitializing;
            context.Destroying -= OnContextDestroying;
        }
        
        private static void OnContextInitializing(object obj)
        {
            if (obj is not IContext context)
                return;
            
            var injector = context.Injector;
            var viewRegistry = injector.GetInstance<ViewRegistry>();
            var mediatorMap = injector.GetInstance<IMediatorMap>() as MediatorMap;
            viewRegistry.AddHandler(mediatorMap);
        }
        
        private static void OnContextDestroying(object obj)
        {
            if (obj is not IContext context)
                return;
            
            var injector = context.Injector;
            var viewRegistry = injector.GetInstance<ViewRegistry>();
            var mediatorMap = injector.GetInstance<IMediatorMap>() as MediatorMap;
            viewRegistry.RemoveHandler(mediatorMap);
            
            if (injector.HasMapping<IMediatorMap>())
                injector.Unmap<IMediatorMap>();
        }
    }
}