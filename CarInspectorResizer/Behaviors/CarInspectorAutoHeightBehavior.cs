namespace CarInspectorResizer.Behaviors;

using System;
using System.Collections.Generic;
using CarInspectorResizer.Overrides;
using Game.Messages;
using JetBrains.Annotations;
using Model;
using Model.AI;
using UI.Common;
using UI.EngineControls;
using UnityEngine;

[PublicAPI]
public sealed class CarInspectorAutoHeightBehavior : MonoBehaviour {

    internal UIStateOverride<string?> SelectedTabState { get; } = new(null);

    private readonly HashSet<IDisposable> _Observers = new();
    private Car? _Car;
    private Window _Window = null!;

    private float _ExpandAlways;
    private readonly Dictionary<string, float> _TabExpansions = new();
    private readonly Dictionary<AutoEngineerMode, float> _OrdersExpansions = new();

    private static Vector2? _OriginalWindowSize;

    public float MinHeight { get; set; } = 330;

    public void Awake() {
        _Window = gameObject!.GetComponent<Window>()!;
        SelectedTabState.ValueChanged = _ => UpdateWindowHeight();
    }

    internal void Populate(Car car) {
        if (_Car == car) {
            return;
        }

        _Car = car;

        _OriginalWindowSize ??= _Window.GetContentSize();

        foreach (var observer in _Observers) {
            observer.Dispose();
        }

        _Observers.Clear();

        var persistence = new AutoEngineerPersistence(_Car.KeyValueObject!);
        _Observers.Add(persistence.ObserveOrders(_ => UpdateWindowHeight()));
        _Observers.Add(persistence.ObserveContextualOrdersChanged(UpdateWindowHeight));
    }

    public void ExpandAlways(float height) {
        _ExpandAlways += height;
    }

    public void ExpandTab(string tabName, float height) {
        _TabExpansions.TryGetValue(tabName, out var value);
        _TabExpansions[tabName] = value + height;
    }

    public void ExpandOrders(AutoEngineerMode mode, float height) {
        _OrdersExpansions.TryGetValue(mode, out var value);
        _OrdersExpansions[mode] = value + height;
    }

    public void UpdateWindowHeight() {
        if (_Car == null || _OriginalWindowSize == null) {
            return;
        }

        var persistence = new AutoEngineerPersistence(_Car.KeyValueObject!);
        var helper = new AutoEngineerOrdersHelper(_Car, persistence);
        var mode = helper.Mode();

        var height = _OriginalWindowSize.Value.y + _ExpandAlways;

        if (SelectedTabState.Value != null) {
            _TabExpansions.TryGetValue(SelectedTabState.Value, out var tabExpansion);
            height += tabExpansion;
        }

        if (SelectedTabState.Value == "orders") {
            _OrdersExpansions.TryGetValue(mode, out var ordersExpansion);
            height += ordersExpansion;

            if (!string.IsNullOrEmpty(persistence.PassengerModeStatus!)) {
                height += 30;
            }

            if (persistence.ContextualOrders!.Count > 0) {
                height += 30;
            }
        }

        _Window.SetContentSize(new Vector2(_OriginalWindowSize.Value.x, Mathf.Max(MinHeight, height)));
    }

}