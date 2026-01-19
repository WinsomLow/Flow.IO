# FlowDrafter

FlowDrafter is a WPF-based flow editor focused on composing, previewing, and
iterating on flow components. The goal is to make it easy to build a library
of reusable controls, snap them together visually, and test how data moves
through a flow.

![FlowDrafter screenshot](Asset/Screenshot%202026-01-19%20124552.png)

The solution is split into a core library, a component library, and the drafter
app that hosts and loads plugins.

## Solution layout

- `Flow.Core` contains core models, view models, and the FlowControl API.
- `Flow.Component` contains built-in flow components (example plugins).
- `Flow.Drafter` is the host application that loads plugins and renders flows.

## Plugin versioning

Plugins must declare a version that matches the FlowControl API version. The
drafter will skip plugins with missing or incompatible versions.

The API version is defined in `Flow.Core/Control/FlowControl.cs` as
`FlowControl.ApiVersionString`.

Example:

```csharp
[FlowPlugin(FlowControl.ApiVersionString)]
public partial class Process : FlowControl
{
  // ...
}
```

Compatibility rules:

- Major and minor must match exactly.
- Build and revision must match if specified in the plugin version.

## Build

Open `Flow.IO.sln` in Visual Studio and build the solution.

## Notes

- Plugins are loaded by the drafter from assemblies on disk.
- The plugin loader will skip types that are abstract, do not inherit
  `FlowControl`, or fail version checks.

## Custom plugins

You can customize your own flow control by building it as a plugin that
derives from `FlowControl`. The drafter will discover and load compatible
plugin assemblies at runtime.

## Future direction

FlowDrafter is intended to evolve into a more flexible flow authoring tool.
Planned improvements include broader plugin compatibility, cleaner separation
of UI logic from the shell, user-configurable plugin discovery, and stronger
test coverage. Longer term, multiple themes and richer UI testing are expected
to make the editor easier to adapt for different visual styles and workflows.

## TODO

1. Support backward compatibility.
2. Move logic to individual objects instead of MainWindow.
3. Add a settings file to allow users to set the plugin path.
4. Unit tests.
5. UI tests.
6. Support multiple themes since style is separate from control.
