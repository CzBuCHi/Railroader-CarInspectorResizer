namespace CarInspectorResizer.Overrides;

using System;

internal sealed class UIStateOverride<T>(T value) : UI.Builder.UIState<T>(value) {

    public Action<T>? ValueChanged { get; set; }

}