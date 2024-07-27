namespace CarInspectorResizer.HarmonyPatches;

using System;
using System.Collections.Generic;
using CarInspectorResizer.Behaviors;
using CarInspectorResizer.Extensions;
using HarmonyLib;
using JetBrains.Annotations;
using Model;
using UI.Builder;
using UI.CarInspector;
using UI.Common;

[PublicAPI]
[HarmonyPatch]
internal static class CarInspectorPatches {

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CarInspector), "Awake")]
    public static void Awake(ref Window ____window, ref HashSet<IDisposable> ____observers, ref UIState<string?> ____selectedTabState) {
        var windowAutoHeight = ____window.gameObject!.GetOrAddComponent<CarInspectorAutoHeightBehavior>();
        ____selectedTabState = windowAutoHeight.SelectedTabState;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CarInspector), "Populate")]
    public static void Populate(Car car, ref Window ____window) {
        var windowAutoHeight = ____window.gameObject!.GetComponent<CarInspectorAutoHeightBehavior>()!;
        windowAutoHeight.Populate(car);
    }

}