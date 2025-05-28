using System;

namespace Pharos.Common.CommandCenter
{
    public class CommandMapper : ICommandMapper, ICommandUnmapper, ICommandConfigurator
    {
        private readonly ICommandMappingList mappings;

        private ICommandMapping mapping;

        public CommandMapper(ICommandMappingList mappings)
        {
            this.mappings = mappings;
        }

        public ICommandConfigurator ToCommand<T>()
        {
            return ToCommand(typeof(T));
        }

        public ICommandConfigurator ToCommand(Type commandType)
        {
            mapping = new CommandMapping(commandType);
            mappings.AddMapping(mapping);
            return this;
        }

        public void FromCommand<T>()
        {
            FromCommand(typeof(T));
        }

        public void FromCommand(Type commandType)
        {
            mappings.RemoveMappingFor(commandType);
        }

        public void FromAll()
        {
            mappings.RemoveAllMappings();
        }

        public ICommandConfigurator WithGuards(params object[] guards)
        {
            mapping.AddGuards(guards);
            return this;
        }

        public ICommandConfigurator WithGuards<T>()
        {
            return WithGuards(typeof(T));
        }

        public ICommandConfigurator WithGuards<T1, T2>()
        {
            return WithGuards(typeof(T1), typeof(T2));
        }

        public ICommandConfigurator WithGuards<T1, T2, T3>()
        {
            return WithGuards(typeof(T1), typeof(T2), typeof(T3));
        }

        public ICommandConfigurator WithGuards<T1, T2, T3, T4>()
        {
            return WithGuards(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
        }

        public ICommandConfigurator WithGuards<T1, T2, T3, T4, T5>()
        {
            return WithGuards(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
        }

        public ICommandConfigurator WithHooks(params object[] hooks)
        {
            mapping.AddHooks(hooks);
            return this;
        }

        public ICommandConfigurator WithHooks<T>()
        {
            return WithHooks(typeof(T));
        }

        public ICommandConfigurator WithHooks<T1, T2>()
        {
            return WithHooks(typeof(T1), typeof(T2));
        }

        public ICommandConfigurator WithHooks<T1, T2, T3>()
        {
            return WithHooks(typeof(T1), typeof(T2), typeof(T3));
        }

        public ICommandConfigurator WithHooks<T1, T2, T3, T4>()
        {
            return WithHooks(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
        }

        public ICommandConfigurator WithHooks<T1, T2, T3, T4, T5>()
        {
            return WithHooks(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
        }

        public ICommandConfigurator ExecuteOnce(bool value = true)
        {
            mapping.ShouldExecuteOnce = value;
            return this;
        }

        public ICommandConfigurator WithPayloadInjection(bool value)
        {
            mapping.PayloadInjectionEnabled = value;
            return this;
        }
    }
}