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

    public float MinHeight { get; set; } = 330;

    public void Awake() {
        _Window = gameObject!.GetComponent<Window>()!;
        SelectedTabState.ValueChanged = value => {
            CarInspectorResizerPlugin.ConsoleMessage($"SelectedTabState: {value}");
            UpdateWindowHeight();
        };
    }

    internal void Populate(Car car) {
        _ExpandAlways = 0;
        _TabExpansions.Clear();
        _OrdersExpansions.Clear();

        if (_Car == car) {
            return;
        }

        _Car = car;

        foreach (var observer in _Observers) {
            observer.Dispose();
        }

        _Observers.Clear();

        var persistence = new AutoEngineerPersistence(_Car.KeyValueObject!);
        _Observers.Add(persistence.ObserveOrders(_ => {
            UpdateWindowHeight();
        }, false));
        _Observers.Add(persistence.ObserveContextualOrdersChanged(() => {
            UpdateWindowHeight();
        }));
    }

    public void ExpandAlways(float height) {
        _ExpandAlways += height;
        CarInspectorResizerPlugin.ConsoleMessage($"expanded by {height} to {_ExpandAlways}");
    }

    public void ExpandTab(string tabName, float height) {
        
        _TabExpansions.TryGetValue(tabName, out var value);
        _TabExpansions[tabName] = value + height;
        CarInspectorResizerPlugin.ConsoleMessage($"'{tabName}' expanded by {height} to {_TabExpansions[tabName]}");
    }

    public void ExpandOrders(AutoEngineerMode mode, float height) {
        _OrdersExpansions.TryGetValue(mode, out var value);
        _OrdersExpansions[mode] = value + height;
        CarInspectorResizerPlugin.ConsoleMessage($"'{mode}' expanded by {height} to {_OrdersExpansions[mode]}");
    }

    public void UpdateWindowHeight() {
        if (_Car == null || SelectedTabState.Value == null) {
            return;
        }

        var persistence = new AutoEngineerPersistence(_Car.KeyValueObject!);
        var helper = new AutoEngineerOrdersHelper(_Car, persistence);
        var mode = helper.Mode();

        var height = MinHeight + _ExpandAlways;
        if (_ExpandAlways > 0) {
            CarInspectorResizerPlugin.ConsoleMessage($"+{_ExpandAlways}");
        }

        if (SelectedTabState.Value != null) {
            _TabExpansions.TryGetValue(SelectedTabState.Value, out var tabExpansion);
            height += tabExpansion;
            if (tabExpansion > 0) {
                CarInspectorResizerPlugin.ConsoleMessage($"+{tabExpansion} (tabExpansion)");
            }
        }

        if (SelectedTabState.Value == "orders") {
            _OrdersExpansions.TryGetValue(mode, out var ordersExpansion);
            height += ordersExpansion;
            if (ordersExpansion > 0) {
                CarInspectorResizerPlugin.ConsoleMessage($"+{ordersExpansion} (ordersExpansion)");
            }

            if (!string.IsNullOrEmpty(persistence.PassengerModeStatus!)) {
                height += 30;
                CarInspectorResizerPlugin.ConsoleMessage("+30 (PassengerModeStatus)");
            }

            if (persistence.ContextualOrders!.Count > 0) {
                height += 30;
                CarInspectorResizerPlugin.ConsoleMessage("+30 (ContextualOrders)");
            }
        }

        var size = _Window.GetContentSize();
        CarInspectorResizerPlugin.ConsoleMessage($"Updated window height {height}");
        _Window.SetContentSize(new Vector2(size.x - 2, height));
    }

}