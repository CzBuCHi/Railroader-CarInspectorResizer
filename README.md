# Railroader Mod: CarInspectorResizer

This mod is useless on its own, but it is allowing other mods to extend car inspectino window height.

`CarInspectorResizer.Example1` and `CarInspectorResizer.Example2` are example mods to ilustrate how multiple mods can tweak car inspector height.

Usage:
1. Add reference to this mod (duh)
2. Add this harmony patch to your mod:
```
[HarmonyPatch]
internal static class CarInspectorPatches {

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CarInspector), "Awake")]
    public static void Awake(ref Window ____window) {
        var windowAutoHeight = ____window.gameObject!.GetComponent<CarInspectorAutoHeightBehavior>()!;
        windowAutoHeight.MinHeight = 330;                           // window minimal height (330 is default value)
        windowAutoHeight.ExpandTab("equipment", 75);                // this will expand window by 75 units when equipment tab is selected
        windowAutoHeight.ExpandOrders(AutoEngineerMode.Road, 50);   // this will expand window by 50 units when Road mode on orders tab is selected
        windowAutoHeight.ExpandOrders(AutoEngineerMode.Yard, 100);  // this will expand window by 100 units when Yard mode on orders tab is selected
    }
}
```

Vanila game tab names are `car`, `equipment`, `pass`, `ops` and `orders`. You need to use those in `ExpandTab` method

## Installation

* Download `CarInspectorResizer-VERSION.zip` from the releases page
* Install with [Railloader]([https://www.nexusmods.com/site/mods/21](https://railroader.stelltis.ch/))


## Project Setup

In order to get going with this, follow the following steps:

1. Clone the repo
2. Copy the `Paths.user.example` to `Paths.user`, open the new `Paths.user` and set the `<GameDir>` to your game's directory.
3. Open the Solution
4. You're ready!

### During Development
Make sure you're using the _Debug_ configuration. Every time you build your project, the files will be copied to your Mods folder and you can immediately start the game to test it.

### Publishing
Make sure you're using the _Release_ configuration. The build pipeline will then automatically do a few things:

1. Makes sure it's a proper release build without debug symbols
1. Replaces `$(AssemblyVersion)` in the `Definition.json` with the actual assembly version.
1. Copies all build outputs into a zip file inside `bin` with a ready-to-extract structure inside, named like the project they belonged to and the version of it.
