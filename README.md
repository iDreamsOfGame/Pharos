# Pharos

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![Version](https://img.shields.io/github/v/tag/iDreamsOfGame/Pharos?label=version&sort=semver)](https://github.com/iDreamsOfGame/Pharos/tags)
[![openupm](https://img.shields.io/badge/openupm-io.github.idreamsofgame.pharos-blue)](https://openupm.com/packages/io.github.idreamsofgame.pharos)

Pharos is a lightweight application framework by using [VContainerPlus](https://github.com/iDreamsOfGame/VContainerPlus) — a modified version of the open-source [VContainer](https://github.com/hadashiA/VContainer) dependency injection library — for creating applications based upon the classic Model-View-Controller design meta-pattern.

The architecture is ported from the ideas of [Robotlegs](https://github.com/robotlegs/robotlegs-framework) (a well-known MVCS framework originally written for ActionScript 3, and its C# port [robotlegs-sharp-framework](https://github.com/iDreamsOfGame/robotlegs-sharp-framework)). Pharos offers:

- Dependency injection (powered by [VContainerPlus](https://github.com/iDreamsOfGame/VContainerPlus), with **C# Source Generator**–based injection instead of reflection)
- Module management (child contexts)
- Command management (event-driven commands and direct commands)
- **Asynchronous command queue management (`DirectAsyncCommandMap`)**
- View management (mediation)
- Plug-and-play extensions

## Requirements

- Unity **2022.3** or newer
- [VContainerPlus](https://github.com/iDreamsOfGame/VContainerPlus) (installed automatically as a package dependency) — a modified version of [VContainer](https://github.com/hadashiA/VContainer) that generates injection code at compile time

## Installation

The package is published on [OpenUPM](https://openupm.com/packages/io.github.idreamsofgame.pharos/) as `io.github.idreamsofgame.pharos`.

### Install via OpenUPM (recommended)

**Using the Unity Package Manager UI**

1. Open **Edit → Project Settings → Package Manager**.
2. Add a new scoped registry with the following values:
   - **Name**: `OpenUPM`
   - **URL**: `https://package.openupm.com`
   - **Scope(s)**: `io.github.idreamsofgame`
3. Open **Window → Package Manager**, click **+** → **Install package by name...**, and enter:

   ```
   io.github.idreamsofgame.pharos
   ```

**Using openupm-cli**

```bash
openupm add io.github.idreamsofgame.pharos
```

**Using `Packages/manifest.json`**

```json
{
  "scopedRegistries": [
    {
      "name": "OpenUPM",
      "url": "https://package.openupm.com",
      "scopes": [
        "io.github.idreamsofgame"
      ]
    }
  ],
  "dependencies": {
    "io.github.idreamsofgame.pharos": "2.0.7"
  }
}
```

### Install via Git URL

**Using the Unity Package Manager UI**

1. Open **Window → Package Manager**, click **+** → **Install package from git URL...**, and enter:

   ```
   https://github.com/iDreamsOfGame/Pharos.git?path=/Assets/Pharos
   ```

2. To install a specific release, append a version tag:

   ```
   https://github.com/iDreamsOfGame/Pharos.git?path=/Assets/Pharos#2.0.7
   ```

**Using `Packages/manifest.json`**

```json
{
  "dependencies": {
    "io.github.idreamsofgame.pharos": "https://github.com/iDreamsOfGame/Pharos.git?path=/Assets/Pharos#2.0.7"
  }
}
```

> **Note**: Pharos depends on `io.github.idreamsofgame.vcontainerplus`. When installing from a Git URL, make sure the OpenUPM scoped registry above is also configured so Unity can resolve the dependency automatically.

## Getting Started

### The Context

A `Context` is the entry point of a Pharos application. It owns the dependency injector, manages the lifecycle of your application, and hosts all of the plug-and-play extensions.

```csharp
using Pharos.Extensions.CommandManagement;
using Pharos.Extensions.DebugLogging;
using Pharos.Extensions.DirectAsyncCommand;
using Pharos.Extensions.EventManagement;
using Pharos.Extensions.Mediation;
using Pharos.Framework;
using UnityEngine;

public sealed class GameBootstrap : MonoBehaviour
{
    private Context context;

    private void Awake()
    {
        context = new Context();

        // Install the extensions your application needs.
        context.AddExtension<EventManagementExtension>();
        context.AddExtension<CommandManagementExtension>();
        context.AddExtension<DirectAsyncCommandMapExtension>();
        context.AddExtension<MediatorMapExtension>();
        context.AddExtension<DebugLoggingExtension>();

        // Register configuration classes. They are processed during initialization.
        context.Configure<GameConfig>();

        context.Initialize();
    }

    private void OnDestroy()
    {
        context.Destroy();
    }
}
```

The context lifecycle follows `Initialize → Suspend → Resume → Destroy`, and each phase raises corresponding events (`Initializing`, `Initialized`, `Suspending`, `Suspended`, `Resuming`, `Resumed`, `Destroying`, `Destroyed`) that the [EventManagementExtension](#event-management) also relays as `LifecycleEvent`s.

### Configurations

Configurations implement `IConfig` and run when the context initializes. Because a configuration is created by the injector, its members decorated with `[Inject]` are populated automatically.

```csharp
using Pharos.Extensions.CommandManagement;
using Pharos.Extensions.DirectAsyncCommand;
using Pharos.Extensions.Mediation;
using Pharos.Framework;
using Pharos.Framework.Injection;

public sealed class GameConfig : IConfig
{
    [Inject] public IPharosInjector Injector { get; set; }
    [Inject] public IEventCommandMap CommandMap { get; set; }
    [Inject] public IMediatorMap MediatorMap { get; set; }

    public void Configure()
    {
        // Bind application services.
        Injector.Map<ILevelService>().ToSingleton<LevelService>();

        // Map events to commands and views to mediators.
        CommandMap.Map<GameEvent>(GameEvent.Type.StartGame).ToCommand<StartGameCommand>();
        MediatorMap.Map<HudView>().ToMediator<HudMediator>();
    }
}
```

### Dependency Injection

Pharos wraps VContainerPlus with its own injector (`IPharosInjector`). VContainerPlus is a modified version of the open-source [VContainer](https://github.com/hadashiA/VContainer): it uses **C# Source Generators** to generate the injection code at compile time, so `[Inject]` members are populated without the runtime performance overhead of reflection-based injection — a significant advantage on performance-sensitive platforms such as mobile devices. Bindings are created fluently through `Map`:

```csharp
Injector.Map<ILevelService>().ToSingleton<LevelService>(); // Concrete singleton
Injector.Map<ILevelService>().ToType<LevelService>();      // Transient
Injector.Map<ILevelService>().ToValue(levelService);       // Existing instance
Injector.Map<ILevelService>("remote").ToSingleton<RemoteLevelService>(); // Keyed binding

Injector.GetInstance<ILevelService>();                     // Resolve a binding
```

The context injector is automatically shared with every extension, command, mediator and configuration, so `[Inject]` works everywhere.

### Event Management

Install `EventManagementExtension` to get a context-wide event dispatcher (`IEventDispatcher`), which is also exposed through the static `EventDispatcher.Instance`. Events derive from `Event`:

```csharp
using Pharos.Common.EventCenter;

public sealed class GameEvent : Event
{
    public enum Type
    {
        StartGame,
        LevelLoaded,
    }

    public GameEvent(Type type) : base(type)
    {
    }
}
```

Dispatch and listen:

```csharp
eventDispatcher.AddEventListener<GameEvent>(GameEvent.Type.LevelLoaded, OnLevelLoaded);
eventDispatcher.Dispatch(new GameEvent(GameEvent.Type.StartGame));
eventDispatcher.RemoveEventListener<GameEvent>(GameEvent.Type.LevelLoaded, OnLevelLoaded);
```

### Command Management

Install `CommandManagementExtension` to map events to commands. A command implements `ICommand` (or extends `AsyncCommand` for asynchronous work) and is created, injected, executed and released by the framework:

```csharp
using Pharos.Common.CommandCenter;

public sealed class StartGameCommand : ICommand
{
    [Inject] public GameEvent Event { get; set; }   // The dispatched event is injected as payload.
    [Inject] public ILevelService LevelService { get; set; }

    public void Execute()
    {
        LevelService.LoadNext(Event.EventType);
    }
}
```

```csharp
// Map an event type to a command...
CommandMap.Map<GameEvent>(GameEvent.Type.StartGame).ToCommand<StartGameCommand>();

// ...with guards, hooks and options.
CommandMap.Map<GameEvent>(GameEvent.Type.StartGame)
    .ToCommand<StartGameCommand>()
    .WithGuards<HasSaveGameGuard>()
    .WithHooks<TrackAnalyticsHook>()
    .ExecuteOnce()
    .WithPayloadInjection(true);

// Remove mappings.
CommandMap.Unmap(GameEvent.Type.StartGame).FromCommand<StartGameCommand>();
CommandMap.Unmap(GameEvent.Type.StartGame).FromAll();
```

- **Guards** (`IGuard.Approve()`) decide *whether* a command is allowed to run. All guards must approve.
- **Hooks** (`IHook.Hook()`) run *before* the command executes.
- `ExecuteOnce()` removes the mapping after the first execution.
- `WithPayloadInjection()` controls whether the event (and other payload values) are injected into the command. It is enabled by default.

### Direct Commands

Install `DirectCommandMapExtension` to execute commands directly from code, without going through an event:

```csharp
using Pharos.Extensions.DirectCommand;

[Inject] public IDirectCommandMap DirectCommandMap { get; set; }

DirectCommandMap
    .Map<StartGameCommand>()
    .Execute();
```

All commands mapped on the mapper are executed immediately with an optional `CommandPayload`.

### Direct Async Commands

`DirectAsyncCommandMap` (install `DirectAsyncCommandMapExtension`) is a Pharos addition on top of the Robotlegs command system: it executes a **queue of asynchronous commands in order**. Each command must report completion by calling `Executed()`, and only then does the next command in the queue start.

**1. Write asynchronous commands**

Derive from `AsyncCommand` and call `Executed()` when the asynchronous work is done:

```csharp
using Pharos.Common.CommandCenter;

public sealed class LoadLevelCommand : AsyncCommand
{
    [Inject] public ILevelService LevelService { get; set; }

    public override void Execute()
    {
        LevelService.LoadAsync("Level_01", OnLoaded);
    }

    private void OnLoaded()
    {
        Executed(); // Reports completion. The next command in the queue starts here.
    }
}

public sealed class SpawnPlayerCommand : AsyncCommand
{
    public override void Execute()
    {
        // ... spawn the player asynchronously ...
        Executed();
    }
}
```

**2. Queue the commands and run them**

Each `IDirectAsyncCommandMap` instance represents a single, self-contained run: resolve a fresh instance for every queue you want to execute (it is registered as a transient type), configure the optional callbacks, then chain the `Map<T>()` calls and call `Execute()`.

```csharp
using Pharos.Extensions.DirectAsyncCommand;

[Inject] public IPharosInjector Injector { get; set; }

public void StartLevelSequence()
{
    var asyncCommandMap = Injector.GetInstance<IDirectAsyncCommandMap>();

    // Optional: per-command progress callback (command type, current index, total count).
    asyncCommandMap.SetCommandExecutedCallback((commandType, current, total) =>
        Debug.Log($"{commandType.Name} finished ({current}/{total})."));

    // Optional: invoked when the whole queue has finished.
    asyncCommandMap.SetCommandsExecutedCallback(() => Debug.Log("Level sequence completed."));

    // Optional: invoked when the queue was aborted.
    asyncCommandMap.SetCommandsAbortedCallback(() => Debug.Log("Level sequence aborted."));

    asyncCommandMap
        .Map<LoadLevelCommand>()
        .Map<SpawnPlayerCommand>()
        .Execute();
}
```

Like every other command mapping, each mapping can be configured with guards, hooks and payload injection:

```csharp
asyncCommandMap
    .Map<LoadLevelCommand>()
    .WithGuards<HasNextLevelGuard>()
    .WithHooks<TrackAnalyticsHook>()
    .WithPayloadInjection(true)
    .Map<SpawnPlayerCommand>()
    .Execute();
```

**3. Pass a payload**

```csharp
var payload = new CommandPayload().AddPayload<string>("Level_01");
asyncCommandMap.Map<LoadLevelCommand>().Execute(payload);

// Inside the command, the payload value is injected by type:
[Inject] public string LevelId { get; set; }
```

**4. Abort the queue**

```csharp
asyncCommandMap.Abort();     // Stops the queue and aborts the currently executing command.
asyncCommandMap.Abort(false); // Stops the queue but lets the running command finish.
```

A command can also stop the remaining queue by itself — either by calling `Abort()` or by reporting `Executed(true)`:

```csharp
public sealed class ValidateSaveCommand : AsyncCommand
{
    public override void Execute()
    {
        if (!IsValid)
        {
            Abort(); // Stops the whole queue; SetCommandsAbortedCallback is invoked.
            return;
        }

        Executed();
    }
}
```

**Notes**

- Commands that are not `IAsyncCommand` may also be mapped into the queue; they execute synchronously and the next command starts immediately.
- While a command is running, the context detains it (`Detain`/`Release`) so it cannot be garbage-collected mid-flight.
- Commands are resolved inside a child ("sandbox") injector created for the run, so nothing created by the queue leaks into the rest of the application. For convenience, the map itself is injected into its commands, e.g. `[Inject] IDirectAsyncCommandMap AsyncCommandMap`.

### Mediation

Install `MediatorMapExtension` to bind views to mediators. Views extend `View` (a `MonoBehaviour`) — or `EventView` if you want a per-view-type event dispatcher — and are registered automatically when they wake up:

```csharp
using Pharos.Common.ViewCenter;

public sealed class HudView : EventView
{
    public void NotifyCoinsChanged(int amount)
    {
        ViewDispatcher.Dispatch(new ViewEvent(ViewEvent.Type.CoinsChanged, amount));
    }
}
```

```csharp
using Pharos.Common.EventCenter;
using Pharos.Common.ViewCenter;

public sealed class ViewEvent : Event
{
    public enum Type
    {
        CoinsChanged,
    }

    public int Amount { get; }

    public ViewEvent(Type type, int amount) : base(type)
    {
        Amount = amount;
    }
}
```

Mediators extend `Mediator` and keep the view decoupled from the rest of the application:

```csharp
using Pharos.Extensions.Mediation;

public sealed class HudMediator : Mediator
{
    private HudView TypedView => (HudView)View;

    protected override void OnInitialized()
    {
        // Listen to events from the mediated view...
        AddViewListener<ViewEvent>(ViewEvent.Type.CoinsChanged, OnCoinsChanged);

        // ...and to context-wide events.
        AddContextListener<GameEvent>(GameEvent.Type.LevelLoaded, OnLevelLoaded);
    }

    private void OnCoinsChanged(ViewEvent e)
    {
        Debug.Log($"Coins: {e.Amount}");
    }

    private void OnLevelLoaded(GameEvent e)
    {
        TypedView.ShowResult();
    }
}
```

```csharp
// Map in a configuration. Mediators are created and destroyed automatically.
MediatorMap.Map<HudView>().ToMediator<HudMediator>();
```

The mediator lifecycle mirrors the view's: `OnPreInitialize`, `OnInitializing` and `OnInitialized` run when the view wakes up, and `OnPreDestroy`, `OnDestroying` and `OnDestroyed` run when the view is destroyed. View and context listeners added through the helpers are removed automatically.

> **Note**: the view registry does not buffer views. Initialize the context before your views' `Awake()` runs (for example with an early script execution order), otherwise views that wake up before the context initializes are not mediated.

### Logging

The framework logs through `ILogger`. `DebugLoggingExtension` routes framework logs to `UnityEngine.Debug`:

```csharp
context.LogLevel = LogLevel.Debug;            // Control verbosity.
context.GetLogger(this).LogInfo("Ready. ");   // Get a logger for any source.
context.AddLogHandler(myCustomLogHandler);    // Route logs to your own handler.
```

### Modules

Split a large application into modules by giving each module its own child context. A child context inherits the bindings of its parent injector but keeps its own extensions, mappings and lifecycle:

```csharp
var childContext = new Context();
childContext.AddExtension<CommandManagementExtension>();
childContext.Configure<InventoryModuleConfig>();
parentContext.AddChild(childContext);
childContext.Initialize();
```

## Writing An Extension

Extensions are plug-and-play building blocks. Implement `IExtension` and install it with `context.AddExtension<YourExtension>()`:

```csharp
using Pharos.Framework;

public sealed class MyExtension : IExtension
{
    public void Enable(IContext context)
    {
        // Map your APIs into the injector, subscribe to context lifecycle events, etc.
        context.Injector.Map<IMyService>().ToSingleton<MyService>();
    }

    public void Disable(IContext context)
    {
        // Undo what Enable did, if needed.
    }
}
```

## License

The following source/framework has been applied with the [MIT license](LICENSE).

## Acknowledgements

Thanks to the original Robotlegs contributors who developed the first and second revision of the framework, and to the authors of the [robotlegs-sharp-framework](https://github.com/iDreamsOfGame/robotlegs-sharp-framework) port. Your original source has been beautifully done with painfully complex patterns.

Thanks to the [VContainer](https://github.com/hadashiA/VContainer) authors for the excellent dependency injection foundation, and to the VContainerPlus modifications that bring compile-time source generation to it.

And thanks to whomever is reading this text — if you are benefiting from this framework, it makes maintaining it worthwhile.
