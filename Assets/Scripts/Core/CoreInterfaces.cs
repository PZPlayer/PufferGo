using System;               
using UnityEngine;

public interface IDamagble
{
    void Damage(float value, bool isCrit = false);
}

public interface IHealble
{
    void Heal(float value);
}

public interface IAirShowable
{
    float AirTimer { get; }
    event Action<float> OnAirChanged;
}

public interface IShowScale
{
    event Action<Vector2> OnScaleChanged;
}

    public interface IHealthShowable
{
    float MaxHealth { get; }
    float CurrentHealth { get; }
    event System.Action<float> OnHealthChanged;
    event System.Action OnDeathEvent;
}