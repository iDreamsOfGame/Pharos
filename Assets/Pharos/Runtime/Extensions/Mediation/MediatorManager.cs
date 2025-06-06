using System;
using System.Collections.Generic;
using Pharos.Common.EventCenter;
using Pharos.Framework.Injection;
using Pharos.Common.ViewCenter;
using Pharos.Framework.Helpers;
using UnityEngine;
using Object = UnityEngine.Object;

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
                if (mapping.GuardTypes == null || mapping.GuardTypes.Count == 0 || Guards.Approve(injector, mapping.GuardTypes))
                {
                    var mediatorType = mapping.MediatorType;
                    mediator = injector.GetOrCreateNewInstance(mediatorType) as IMediator;
                    if (mediator != null)
                    {
                        if (mapping.HookTypes is { Count: > 0 })
                        {
                            injector.Map(mediatorType).ToValue(mediator);
                            injector.Build();
                            Hooks.Hook(injector, mapping.HookTypes);
                            injector.Unmap(mediatorType);
                        }

                        AddMediator(view, viewType, mediator, mapping);
                    }
                }
            }

            return mediator;
        }

        public void RemoveMediator(IView view)
        {
            if (!viewToMappingMediatorPair.TryGetValue(view, out var mappingMediatorPair))
                return;

            if (view is IEventView eventView)
                eventView.ViewDispatcher = null;

            var mediator = mappingMediatorPair.Value;
            viewToMappingMediatorPair.Remove(view);
            DestroyMediator(mediator);
            
#if UNITY_EDITOR
            TryDestroyMediatorMappingInfo(view);
#endif
        }

        private static void DestroyMediator(IMediator mediator)
        {
            mediator.PreDestroy();
            mediator.Destroy();
            mediator.View = null;
            mediator.ViewDispatcher = null;
            mediator.PostDestroy();
        }
        
#if UNITY_EDITOR
        private static void TryAddMediatorMappingInfo(IView view, IMediator mediator)
        {
            if (view is Component viewComponent)
            {
                var gameObject = viewComponent.gameObject;
                if (!gameObject)
                    return;

                var mediatorMappingInfo = gameObject.GetComponent<MediatorMappingInfo>();
                if (!mediatorMappingInfo)
                    mediatorMappingInfo = gameObject.AddComponent<MediatorMappingInfo>();

                mediatorMappingInfo.View = view;
                mediatorMappingInfo.Mediator = mediator;
            }
        }

        private static void TryDestroyMediatorMappingInfo(IView view)
        {
            if (view is Component viewComponent)
            {
                var gameObject = viewComponent.gameObject;
                if (!gameObject)
                    return;

                var mediatorMappingInfo = gameObject.GetComponent<MediatorMappingInfo>();
                if (!mediatorMappingInfo)
                    return;

                if (Application.isPlaying)
                {
                    Object.DestroyImmediate(mediatorMappingInfo);
                }
                else
                {
                    Object.Destroy(mediatorMappingInfo);
                }
            }
        }
#endif

        private void AddMediator(IView view,
            Type viewType,
            IMediator mediator,
            IMediatorMapping mapping)
        {
            if (!viewToMappingMediatorPair.ContainsKey(view))
                viewToMappingMediatorPair[view] = new KeyValuePair<IMediatorMapping, IMediator>(mapping, mediator);

            InitializeMediator(view, viewType, mediator);

#if UNITY_EDITOR
            TryAddMediatorMappingInfo(view, mediator);
#endif
        }

        private IEventDispatcher InitializeEventView(IView view, Type viewType)
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
                return viewDispatcher;
            }

            return null;
        }

        private void InitializeMediator(IView view, Type viewType, IMediator mediator)
        {
            var viewDispatcher = InitializeEventView(view, viewType);

            mediator.PreInitialize();
            mediator.View = view;
            mediator.ViewDispatcher = viewDispatcher;
            mediator.Initialize();
            mediator.PostInitialize();
        }
    }
}