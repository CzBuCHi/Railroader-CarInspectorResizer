namespace CarInspectorResizer.Example1.HarmonyPatches;

using CarInspectorResizer.Behaviors;
using Game.Messages;
using HarmonyLib;
using JetBrains.Annotations;
using Model;
using Model.AI;
using UI.Builder;
using UI.CarInspector;
using UI.Common;
using UI.EngineControls;

[PublicAPI]
[HarmonyPatch]
internal static class CarInspectorPatches {

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CarInspector), "Populate")]
    public static void Populate(ref Window ____window) {
        var windowAutoHeight = ____window.gameObject!.GetComponent<CarInspectorAutoHeightBehavior>()!;
        windowAutoHeight.ExpandAlways(30);
        windowAutoHeight.ExpandTab("equipment", 45);
        windowAutoHeight.ExpandOrders(AutoEngineerMode.Road, 30);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CarInspector), "PopulatePanel")]
    public static void PopulatePanel(UIPanelBuilder builder) {
        builder.AddField("Global Button", builder.ButtonStrip(strip => strip.AddButton("Example1", () => { }), 4)!);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CarInspector), "PopulateEquipmentPanel")]
    public static void PopulateEquipmentPanel(UIPanelBuilder builder) {
        builder.AddSection("Equipment Buttons", section => section.ButtonStrip(strip => strip.AddButton("Example1", () => { }), 4));
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CarInspector), "BuildContextualOrders")]
    public static void BuildContextualOrders(UIPanelBuilder builder, AutoEngineerPersistence persistence, Car ____car) {
        var helper = new AutoEngineerOrdersHelper(____car, persistence);
        var mode = helper.Mode();
        if (mode == AutoEngineerMode.Road) {
            builder.AddField("Road Buttons", builder.ButtonStrip(strip => strip.AddButton("Example1", () => { }), 4)!);
        }
    }

}