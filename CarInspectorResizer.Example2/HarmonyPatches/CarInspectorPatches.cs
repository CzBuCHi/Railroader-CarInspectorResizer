namespace CarInspectorResizer.Example2.HarmonyPatches;

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
    [HarmonyPatch(typeof(CarInspector), "Populate")]
    public static void Populate(ref Window ____window) {
        var windowAutoHeight = ____window.gameObject!.GetComponent<CarInspectorAutoHeightBehavior>()!;
        windowAutoHeight.ExpandOrders(AutoEngineerMode.Road, 100);
        windowAutoHeight.ExpandOrders(AutoEngineerMode.Yard, 50);
        windowAutoHeight.UpdateWindowHeight();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CarInspector), "BuildContextualOrders")]
    public static void BuildContextualOrders(UIPanelBuilder builder, AutoEngineerPersistence persistence, Car ____car) {

        var helper = new AutoEngineerOrdersHelper(____car, persistence);
        var mode = helper.Mode();
        switch (mode) {
            case AutoEngineerMode.Road:
                builder.AddSection("Road Buttons", section => {
                    section.ButtonStrip(strip => {
                        strip.AddButton("Example2", () => { });
                        strip.AddButton("Example2", () => { });
                        strip.AddButton("Example2", () => { });
                    }, 4);
                });

                builder.AddSection("More Road buttons", section => {
                    section.ButtonStrip(strip => {
                        strip.AddButton("Example2", () => { });
                        strip.AddButton("Example2", () => { });
                        strip.AddButton("Example2", () => { });
                    }, 4);
                });
                break;

            case AutoEngineerMode.Yard:
                builder.AddSection("Yard Buttons", section => {
                    section.ButtonStrip(strip => {
                        strip.AddButton("Example2", () => { });
                        strip.AddButton("Example2", () => { });
                        strip.AddButton("Example2", () => { });
                    }, 4);
                });

                break;
        }
    }

}