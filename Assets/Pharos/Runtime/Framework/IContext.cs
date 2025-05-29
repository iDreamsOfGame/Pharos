using System;
using System.Collections.Generic;
using Pharos.Framework.Helpers;
using Pharos.Framework.Injection;

namespace Pharos.Framework
{
    public interface IContext : IPinEvent, ILifecycle
    {
        /// <summary>
        /// The injection binder this context relies on. Use this to bind and unbind anything you require.
        /// </summary>
        IInjector Injector { get; }

        /// <summary>
        /// Gets or sets the current log level. 
        /// </summary>
        LogLevel LogLevel { get; set; }

        internal Pin Pin { get; }

        internal LifecycleManager LifecycleManager { get; }

        internal LogManager LogManager { get; }

        internal ExtensionManager ExtensionManager { get; }

        internal ConfigManager ConfigManager { get; }

        /// <summary>
        /// Retrieves a logger for a given source. 
        /// </summary>
        /// <param name="source">Logging source. </param>
        /// <returns>The new logger instance. </returns>
        ILogger GetLogger(object source);

        /// <summary>
        /// Adds a custom log handler. 
        /// </summary>
        /// <param name="handler">The log handler instance to add. </param>
        /// <returns>The implementation instance of interface <see cref="IContext"/>. </returns>
        IContext AddLogHandler(ILogHandler handler);

        /// <summary>
        /// Adds custom extensions or bundles into the context. 
        /// </summary>
        /// <typeparam name="T">IExtension type. </typeparam>
        /// <returns>The implementation instance of interface <see cref="IContext"/>. </returns>
        IContext AddExtension<T>() where T : IExtension;

        /// <summary>
        /// Adds custom extensions or bundles into the context. 
        /// </summary>
        /// <param name="type">Type with an 'Extend(IContext context)' method signature. </param>
        /// <returns>The implementation instance of interface <see cref="IContext"/>. </returns>
        IContext AddExtension(Type type);

        /// <summary>
        /// Adds custom extension or bundle instance into the context. 
        /// </summary>
        /// <param name="extension">The instance implements of <see cref="IExtension"/>. </param>
        /// <returns>The implementation instance of interface <see cref="IContext"/>. </returns>
        IContext AddExtension(IExtension extension);

        /// <summary>
        /// Adds all custom extensions or bundles in enumerable collection into the context. 
        /// </summary>
        /// <param name="types">The collection of Type with an 'Extend(IContext context)' method signature. </param>
        /// <returns>The implementation instance of interface <see cref="IContext"/>. </returns>
        IContext AddExtensions(IEnumerable<Type> types);

        /// <summary>
        /// Configures the context with custom configurations. 
        /// </summary>
        /// <typeparam name="T">Configuration type. </typeparam>
        /// <returns>The implementation instance of interface <see cref="IContext"/>. </returns>
        IContext Configure<T>() where T : IConfig;

        /// <summary>
        /// Configures the context with custom configurations. 
        /// </summary>
        /// <param name="type">Type with a 'Configure' method signature. </param>
        /// <returns>The implementation instance of interface <see cref="IContext"/>. </returns>
        IContext Configure(Type type);

        /// <summary>
        /// Configures the context with custom configurations. 
        /// </summary>
        /// <param name="objects">Configuration objects. </param>
        IContext Configure(params object[] objects);

        /// <summary>
        /// Configures the context with all custom configurations. 
        /// </summary>
        /// <param name="types">The collection of Type with a 'Configure' method signature. </param>
        /// <returns>The implementation instance of interface <see cref="IContext"/>. </returns>
        IContext ConfigureAll(IEnumerable<Type> types);

        /// <summary>
        /// Pins instances in memory. 
        /// </summary>
        /// <param name="instances">Instances to pin. </param>
        /// <returns>The implementation instance of interface <see cref="IContext"/>. </returns>
        IContext Detain(params object[] instances);

        /// <summary>
        /// Unpins instances from memory. 
        /// </summary>
        /// <param name="instances">Instances to unpin. </param>
        /// <returns>The implementation instance of interface <see cref="IContext"/>. </returns>
        IContext Release(params object[] instances);

        /// <summary>
        /// Adds an uninitialized context as a child.
        /// </summary>
        /// <param name="child">The context to add as a child. </param>
        /// <returns>The implementation instance of interface <see cref="IContext"/>. </returns>
        IContext AddChild(IContext child);

        /// <summary>
        /// Removes a child context from this context. 
        /// </summary>
        /// <param name="child">The child context to remove. </param>
        /// <returns>The implementation instance of interface <see cref="IContext"/>. </returns>
        IContext RemoveChild(IContext child);
    }
}