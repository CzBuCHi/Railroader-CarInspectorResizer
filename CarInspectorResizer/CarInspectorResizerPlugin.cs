namespace CarInspectorResizer;

using HarmonyLib;
using JetBrains.Annotations;
using Railloader;
using Serilog;

[UsedImplicitly]
public sealed class CarInspectorResizerPlugin : SingletonPluginBase<CarInspectorResizerPlugin> {

    public static IModdingContext Context { get; private set; } = null!;
    public static IUIHelper UiHelper { get; private set; } = null!;

    private readonly ILogger _Logger = Log.ForContext<CarInspectorResizerPlugin>()!;

    public CarInspectorResizerPlugin(IModdingContext context, IUIHelper uiHelper) {
        Context = context;
        UiHelper = uiHelper;
    }

    public override void OnEnable() {
        _Logger.Information("OnEnable");
        var harmony = new Harmony("CarInspectorResizer");
        harmony.PatchAll();
    }

    public override void OnDisable() {
        _Logger.Information("OnDisable");
        var harmony = new Harmony("CarInspectorResizer");
        harmony.UnpatchAll();
    }

}