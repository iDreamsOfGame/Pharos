using Pharos.Common.CommandCenter;
using VContainer;
using IInjector = Pharos.Framework.Injection.IInjector;

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