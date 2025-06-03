namespace Pharos.Extensions.DirectAsyncCommand
{
    public interface IDirectAsyncCommandConfigurator : IDirectAsyncCommandMapper
    {
        /// <summary>
        /// Guards to check before allowing a command to execute. 
        /// </summary>
        /// <returns>Self</returns>
        /// <param name="guards">Guards</param>
        IDirectAsyncCommandConfigurator WithGuards(params object[] guards);

        /// <summary>
        /// Hooks to run before command execution. 
        /// </summary>
        /// <returns>Self</returns>
        /// <param name="hooks">Hooks</param>
        IDirectAsyncCommandConfigurator WithHooks(params object[] hooks);

        /// <summary>
        /// Should the payload values be injected into the command instance?
        /// </summary>
        /// <returns>Self</returns>
        /// <param name="value">Toggle</param>
        IDirectAsyncCommandConfigurator WithPayloadInjection(bool value = true);
    }
}