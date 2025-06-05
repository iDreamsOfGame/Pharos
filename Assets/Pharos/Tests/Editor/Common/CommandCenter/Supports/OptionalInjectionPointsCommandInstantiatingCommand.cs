using Pharos.Common.CommandCenter;
using Pharos.Framework.Injection;
using ReflexPlus.Attributes;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    internal class OptionalInjectionPointsCommandInstantiatingCommand : ICommand
    {
        [Inject]
        private IInjector injector;

        public void Execute()
        {
            var command = injector.CreateNewInstance<OptionalInjectionPointsCommand>();
            command.Execute();
        }
    }
}