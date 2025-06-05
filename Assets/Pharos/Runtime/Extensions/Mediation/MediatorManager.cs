using System;
using System.Collections.Generic;
using Pharos.Common.EventCenter;
using Pharos.Framework.Injection;
using Pharos.Common.ViewCenter;
using Pharos.Framework.Helpers;

namespace Pharos.Extensions.Mediation
{
    public class MediatorManager : IMediatorManager
    {
        private readonly IInjector injector;

        private readonly Dictionary<Type, IEventDispatcher> viewTypeToViewDispatcherCache = new();
        
        private readonly Dictionary<IView, KeyValuePair<IMediatorMapping, IMediator>> viewToMappingMediatorPair = new();
        
        public MediatorManager(IInjector injector)
        {
            this.injector = injector;
        }

        public IMediator GetMediator(IView view)
        {
            return viewToMappingMediatorPair.TryGetValue(view, out var mappingMediatorPair) ? mappingMediatorPair.Value : null;
        }

        public IMediator CreateMediator(IView view, Type viewType, IMediatorMapping mapping)
        {
            var mediator = GetMediator(view);
            if (mediator == null)
            {
                if (mapping.Guards == null || mapping.Guards.Count == 0 || Guards.Approve(injector, mapping.Guards))
                {
                    var mediatorType = mapping.MediatorType;
                    mediator = injector.GetOrCreateNewInstance(mediatorType) as IMediator;
                    if (mediator != null)
                    {
                        if (mapping.Hooks is { Count: > 0 })
                        {
                            injector.Map(mediatorType).ToValue(mediator);
                            injector.Build();
                            Hooks.Hook(injector, mapping.Hooks);
                            injector.Unmap(mediatorType);
                        }

                        AddMediator(view, viewType, mediator, mapping);
                    }
                }
            }

            return mediator;
        }
        
        private static void InitializeMediator(IView view, IMediator mediator)
        {
            mediator.PreInitialize();
            mediator.View = view;
            mediator.Initialize();
            mediator.PostInitialize();
        }

        private static void DestroyMediator(IMediator mediator)
        {
            mediator.PreDestroy();
            mediator.Destroy();
            mediator.View = null;
            mediator.PostDestroy();
        }

        private void AddMediator(IView view, Type viewType, IMediator mediator, IMediatorMapping mapping)
        {
            if (!viewToMappingMediatorPair.ContainsKey(view))
                viewToMappingMediatorPair[view] = new KeyValuePair<IMediatorMapping, IMediator>(mapping, mediator);

            if (mapping.AutoDestroyEnabled)
                view.Destroying += OnViewDestroying;

            InitializeEventView(view, viewType);
            InitializeMediator(view, mediator);
        }

        private void RemoveMediator(IView view)
        {
            if (!viewToMappingMediatorPair.TryGetValue(view, out var mappingMediatorPair))
                return;

            var mapping = mappingMediatorPair.Key;
            var mediator = mappingMediatorPair.Value;
            viewToMappingMediatorPair.Remove(view);
            
            if (mapping.AutoDestroyEnabled)
                view.Destroying -= OnViewDestroying;
            
            DestroyMediator(mediator);
        }

        private void InitializeEventView(IView view, Type viewType)
        {
            if (view is IEventView eventView)
            {
                IEventDispatcher viewDispatcher;
                
                if (eventView.ViewDispatcherCacheEnabled)
                {
                    viewDispatcher = viewTypeToViewDispatcherCache.GetValueOrDefault(viewType);
                    if (viewDispatcher == null)
                    {
                        viewDispatcher = new EventDispatcher();
                        viewTypeToViewDispatcherCache.Add(viewType, viewDispatcher);
                    }
                }
                else
                {
                    viewDispatcher = new EventDispatcher();
                }
                
                eventView.ViewDispatcher = viewDispatcher;
            }
        }

        private void OnViewDestroying(IView view)
        {
            if (view is IEventView eventView)
                eventView.ViewDispatcher = null;
            
            RemoveMediator(view);
        }
    }
}