namespace CarInspectorResizer.Example1.HarmonyPatches;

using System;
using System.Collections.Generic;
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
    [HarmonyPatch(typeof(CarInspector), "Awake")]
    public static void Awake(ref Window ____window) {
        var windowAutoHeight = ____window.gameObject!.GetComponent<CarInspectorAutoHeightBehavior>()!;
        windowAutoHeight.ExpandTab("equipment", 50);
        windowAutoHeight.ExpandOrders(AutoEngineerMode.Road, 50);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CarInspector), "PopulateEquipmentPanel")]
    public static void PopulateEquipmentPanel(UIPanelBuilder builder) {
        builder.AddSection("Equipment Buttons", section => {
            section.ButtonStrip(strip => {
                strip.AddButton("Example1", () => { });
                strip.AddButton("Example1", () => { });
                strip.AddButton("Example1", () => { });
            }, 4);
        });
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CarInspector), "BuildContextualOrders")]
    public static void BuildContextualOrders(UIPanelBuilder builder, AutoEngineerPersistence persistence, Car ____car) {
        var helper = new AutoEngineerOrdersHelper(____car, persistence);
        var mode = helper.Mode();
        if (mode == AutoEngineerMode.Road) {
            builder.AddSection("Road Buttons", section => {
                section.ButtonStrip(strip => {
                    strip.AddButton("Example1", () => { });
                    strip.AddButton("Example1", () => { });
                    strip.AddButton("Example1", () => { });
                }, 4);
            });
        }
    }

}