using Pharos.Common.CommandCenter;
using Pharos.Framework.Injection;
using VContainer;

namespace PharosEditor.Tests.Common.CommandCenter.Supports
{
    [InjectIgnore]
    internal class OptionalInjectionPointsCommandInstantiatingCommand : ICommand
    {
        [Inject]
        private IPharosInjector injector;

        public void Execute()
        {
            var command = injector.CreateNewInstance<OptionalInjectionPointsCommand>();
            command.Execute();
        }
    }
}