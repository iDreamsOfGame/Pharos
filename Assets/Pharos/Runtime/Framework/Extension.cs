namespace Pharos.Framework
{
    public abstract class Extension : IExtension, IConfig
    {
        public virtual void Enable(IContext context)
        {
        }

        public virtual void Configure()
        {
            RegisterModels();
            RegisterServices();
            RegisterCommands();
            RegisterMediators();
            RegisterOthers();
        }
        
        public virtual void Disable(IContext context)
        {
        }
        
        protected virtual void RegisterModels()
        {
        }

        protected virtual void RegisterServices()
        {
        }

        protected virtual void RegisterCommands()
        {
        }

        protected virtual void RegisterMediators()
        {
        }

        protected virtual void RegisterOthers()
        {
        }
    }
}