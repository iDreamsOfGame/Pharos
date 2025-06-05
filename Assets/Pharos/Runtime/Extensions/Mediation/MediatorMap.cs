using System;
using System.Collections.Generic;
using Pharos.Framework;
using Pharos.Common.ViewCenter;

namespace Pharos.Extensions.Mediation
{
    public class MediatorMap : IMediatorMap, IViewHandler
    {
        private static readonly IMediatorUnmapper NullUnmapper = new NullMediatorUnmapper();

        private readonly Dictionary<Type, MediatorMapper> viewTypeToMapper = new();

        private readonly ILogger logger;

        private readonly IMediatorViewHandler viewHandler;

        public MediatorMap(IContext context)
        {
            logger = context.GetLogger(this);
            var mediatorManager = new MediatorManager(context.Injector);
            viewHandler = new MediatorViewHandler(mediatorManager);
        }

        public IMediatorMapper Map<T>() where T : IView
        {
            return Map(typeof(T));
        }

        public IMediatorMapper Map(Type viewType)
        {
            if (!viewType.IsAssignableFrom(typeof(IView)))
            {
                logger.LogError(new ArgumentException(nameof(viewType)));
                return null;
            }
            
            if (!viewTypeToMapper.ContainsKey(viewType))
                viewTypeToMapper[viewType] = CreateMapper(viewType);

            return viewTypeToMapper[viewType];
        }

        public IMediatorUnmapper Unmap<T>() where T : IView
        {
            return Unmap(typeof(T));
        }

        public IMediatorUnmapper Unmap(Type viewType)
        {
            if (!viewType.IsAssignableFrom(typeof(IView)))
            {
                logger.LogError(new ArgumentException(nameof(viewType)));
                return null;
            }
            
            return viewTypeToMapper.TryGetValue(viewType, out var unmapper) ? unmapper : NullUnmapper;
        }

        public void HandleViewInitialized(IView view, Type viewType)
        {
            viewHandler.HandleViewInitialized(view, viewType);
        }

        private MediatorMapper CreateMapper(Type viewType)
        {
            return new MediatorMapper(viewType, viewHandler, logger);
        }
    }
}