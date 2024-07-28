namespace CarInspectorResizer;

using HarmonyLib;
using JetBrains.Annotations;
using Railloader;
using Serilog;
using UI.Builder;

[UsedImplicitly]
public sealed class CarInspectorResizerPlugin : SingletonPluginBase<CarInspectorResizerPlugin>, IModTabHandler {

    private const string ModIdentifier = "CarInspectorResizer";

    public static IModdingContext Context { get; private set; } = null!;
    public static IUIHelper UiHelper { get; private set; } = null!;
    public static Settings Settings { get; private set; } = null!;

    private readonly ILogger _Logger = Log.ForContext<CarInspectorResizerPlugin>()!;

    public CarInspectorResizerPlugin(IModdingContext context, IUIHelper uiHelper) {
        Context = context;
        UiHelper = uiHelper;
        Settings = Context.LoadSettingsData<Settings>(ModIdentifier) ?? new Settings();
    }

    public override void OnEnable() {
        _Logger.Information("OnEnable");
        var harmony = new Harmony(ModIdentifier);
        harmony.PatchAll();
    }

    public override void OnDisable() {
        _Logger.Information("OnDisable");
        var harmony = new Harmony(ModIdentifier);
        harmony.UnpatchAll();
    }

    public void ModTabDidOpen(UIPanelBuilder builder) {
        builder
            .AddField("Debug messages",
                builder.AddToggle(() => Settings.Debug, o => Settings.Debug = o)!
            )!
            .Tooltip("Debug messages", "Send debug messages to console");
    }

    public void ModTabDidClose() {
        Context.SaveSettingsData(ModIdentifier, Settings);
    }

    public static void ConsoleMessage(string message) {
        if (Settings.Debug) {
            UI.Console.Console.shared!.AddLine("[CIR]: " + message);
        }
    }

}