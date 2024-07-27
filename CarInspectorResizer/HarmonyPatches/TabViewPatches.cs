namespace CarInspectorResizer.HarmonyPatches;

using System;
using System.Collections.Generic;
using System.Linq;
using CarInspectorResizer.Overrides;
using HarmonyLib;
using JetBrains.Annotations;
using UI.Builder;
using UI.TabView;
using UnityEngine.UI;

[PublicAPI]
[HarmonyPatch]
internal static class TabViewPatches {

    [HarmonyPostfix]
    [HarmonyPatch(typeof(TabView), "AddTab")]
    public static void AddTab(string title, string tabId, Action<UIPanelBuilder> closure, TabView __instance, ref List<Toggle> ____toggles) {
        if (__instance.SelectedTabState is not UIStateOverride<string> state) {
            return;
        }

        var toggle = ____toggles.Last();
        toggle.onValueChanged!.AddListener(selected => {
            if (selected) {
                state.ValueChanged?.Invoke(tabId);
            }
        });
    }

}