namespace CarInspectorResizer.Example2.HarmonyPatches;

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
        windowAutoHeight.ExpandOrders(AutoEngineerMode.Road, 30);
        windowAutoHeight.ExpandOrders(AutoEngineerMode.Yard, 30);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CarInspector), "BuildContextualOrders")]
    public static void BuildContextualOrders(UIPanelBuilder builder, AutoEngineerPersistence persistence, Car ____car) {
        var helper = new AutoEngineerOrdersHelper(____car, persistence);
        var mode = helper.Mode();
        switch (mode) {
            case AutoEngineerMode.Road:
                builder.AddField("Road Button", builder.ButtonStrip(strip => strip.AddButton("Example2", () => { }), 4)!);
                break;

            case AutoEngineerMode.Yard:
                builder.AddField("Yard Button", builder.ButtonStrip(strip => strip.AddButton("Example2", () => { }), 4)!);
                break;
        }
    }

}