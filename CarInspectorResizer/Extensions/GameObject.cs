namespace CarInspectorResizer.Extensions;

using UnityEngine;

internal static class GameObjectExtensions {

    public static TComponent GetOrAddComponent<TComponent>(this GameObject gameObject) where TComponent : Component {
        TComponent component;

        lock (gameObject) {
            if (!gameObject.TryGetComponent(out component)) {
                component = gameObject.AddComponent<TComponent>()!;
            }
        }

        return component!;
    }

}