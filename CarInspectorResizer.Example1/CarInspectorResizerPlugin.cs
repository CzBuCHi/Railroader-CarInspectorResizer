namespace CarInspectorResizer.Example1;

using HarmonyLib;
using JetBrains.Annotations;
using Railloader;
using Serilog;

[UsedImplicitly]
public sealed class CarInspectorResizerExample1Plugin : SingletonPluginBase<CarInspectorResizerExample1Plugin> {

    public static IModdingContext Context { get; private set; } = null!;
    public static IUIHelper UiHelper { get; private set; } = null!;

    private readonly ILogger _Logger = Log.ForContext<CarInspectorResizerExample1Plugin>()!;

    public CarInspectorResizerExample1Plugin(IModdingContext context, IUIHelper uiHelper) {
        Context = context;
        UiHelper = uiHelper;
    }

    public override void OnEnable() {
        _Logger.Information("OnEnable");
        var harmony = new Harmony("CarInspectorResizer.Example1");
        harmony.PatchAll();
    }

    public override void OnDisable() {
        _Logger.Information("OnDisable");
        var harmony = new Harmony("CarInspectorResizer.Example1");
        harmony.UnpatchAll();
    }

}