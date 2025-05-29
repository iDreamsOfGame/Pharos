using System;
using NUnit.Framework;
using Pharos.Framework;
using Pharos.Framework.Helpers;
using Pharos.Framework.Injection;
using ReflexPlus.Attributes;

// ReSharper disable ClassNeverInstantiated.Local

namespace PharosEditor.Tests.Framework.Helpers
{
    [TestFixture]
    internal class ConfigManagerTests
    {
        private class PlainConfig : IConfig
        {
            public const string CallbackInjectKey = nameof(Callback);

            [Inject(CallbackInjectKey)]
            public Action<PlainConfig> Callback { get; private set; }

            public void Configure()
            {
                Callback?.Invoke(this);
            }
        }

        private class UntypedConfig
        {
            public const string CallbackInjectKey = nameof(Callback);

            [Inject(CallbackInjectKey)]
            public Action<UntypedConfig> Callback { get; private set; }

            public void Configure()
            {
                Callback?.Invoke(this);
            }
        }

        private IContext context;

        private IInjector injector;

        private ConfigManager configManager;

        [SetUp]
        public void Setup()
        {
            context = new Context();
            injector = context.Injector;
            configManager = context.ConfigManager;
        }

        [TearDown]
        public void Cleanup()
        {
            injector?.Dispose();
            injector = null;

            configManager?.Destroy();
            configManager = null;
        }

        [Test]
        public void AddConfig_AddedBeforeInitializationWillNotInstantiateImmediately_ReturnsNull()
        {
            PlainConfig actual = null;
            injector.Map(typeof(Action<PlainConfig>), PlainConfig.CallbackInjectKey).ToValue((Action<PlainConfig>)delegate(PlainConfig config) { actual = config; });
            configManager.AddConfig<PlainConfig>();
            Assert.That(actual, Is.Null);
        }

        [Test]
        public void AddConfig_TypeAddedBeforeInitializationWillInstantiateOnInitializing_ReturnCorrectInstance()
        {
            PlainConfig actual = null;
            injector.Map(typeof(Action<PlainConfig>), PlainConfig.CallbackInjectKey).ToValue((Action<PlainConfig>)delegate(PlainConfig config) { actual = config; });
            configManager.AddConfig(typeof(PlainConfig));
            context.Initialize();
            Assert.That(actual, Is.InstanceOf<PlainConfig>());
        }

        [Test]
        public void AddConfig_TypeAddedAfterInitializationWillInstantiateImmediately_ReturnCorrectInstance()
        {
            PlainConfig actual = null;
            injector.Map(typeof(Action<PlainConfig>), PlainConfig.CallbackInjectKey).ToValue((Action<PlainConfig>)delegate(PlainConfig config) { actual = config; });
            context.Initialize();
            configManager.AddConfig(typeof(PlainConfig));
            Assert.That(actual, Is.InstanceOf<PlainConfig>());
        }

        [Test]
        public void AddConfig_ObjectAddedBeforeInitializationWillInstantiateOnInitializing_ReturnCorrectInstance()
        {
            var expected = new PlainConfig();
            PlainConfig actual = null;
            injector.Map(typeof(Action<PlainConfig>), PlainConfig.CallbackInjectKey).ToValue((Action<PlainConfig>)delegate(PlainConfig config) { actual = config; });
            configManager.AddConfig(expected);
            context.Initialize();
            Assert.That(actual, Is.SameAs(expected));
        }

        [Test]
        public void AddConfig_ObjectAddedAfterInitializationWillInstantiateImmediately_ReturnCorrectInstance()
        {
            var expected = new PlainConfig();
            PlainConfig actual = null;
            injector.Map(typeof(Action<PlainConfig>), PlainConfig.CallbackInjectKey).ToValue((Action<PlainConfig>)delegate(PlainConfig config) { actual = config; });
            context.Initialize();
            configManager.AddConfig(expected);
            Assert.That(actual, Is.SameAs(expected));
        }

        [Test]
        public void AddConfig_UntypedAddedAfterInitializationWillInstantiateImmediately_ReturnCorrectInstance()
        {
            UntypedConfig actual = null;
            injector.Map(typeof(Action<UntypedConfig>), UntypedConfig.CallbackInjectKey).ToValue((Action<UntypedConfig>)delegate(UntypedConfig config) { actual = config; });
            context.Initialize();
            configManager.AddConfig(typeof(UntypedConfig));
            Assert.That(actual, Is.InstanceOf<UntypedConfig>());
        }
    }
}