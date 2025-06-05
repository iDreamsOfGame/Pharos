using System.Collections.Generic;

namespace Pharos.Common.ViewCenter
{
    internal class ViewRegistry
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
            foreach (var handler in handlers)
            {
                handler?.HandleViewInitialized(view, viewType);
            }
        }
    }
}