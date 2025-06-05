using System.Collections.Generic;

// ReSharper disable ForCanBeConvertedToForeach

namespace Pharos.Common.ViewCenter
{
    public class ViewRegistry
    {
        public static ViewRegistry Instance { get; } = new();
        
        private readonly List<IViewHandler> handlers = new();

        public void AddHandler(IViewHandler handler)
        {
            if (!handlers.Contains(handler))
                handlers.Add(handler);
        }

        public void RemoveHandler(IViewHandler handler)
        {
            handlers.Remove(handler);
        }

        public void RegisterView(IView view)
        {
            var viewType = view.GetType();
            for (var i = 0; i < handlers.Count; i++)
            {
                var handler = handlers[i];
                handler?.HandleViewInitialized(view, viewType);
            }
        }

        public void RemoveView(IView view)
        {
            for (var i = 0; i < handlers.Count; i++)
            {
                var handler = handlers[i];
                handler?.HandleViewDestroying(view);
            }
        }
    }
}