using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private HealthVisualizerComponent m_HealthVisual = null;

    [Header("Properties")]
    [SerializeField] private int m_Health    = 100;
    [SerializeField] private int m_MaxHealth = 100;

    private int m_SpawnHealth = 100;

    private void Awake()
    {
        m_SpawnHealth = m_Health;
    }

    void SetHealth(int _value)
    {
        if (_value <= 0)
        {
            m_Health = 0;
            OnDeath();
        }
        else if (_value > m_MaxHealth)
        {
            m_Health = m_MaxHealth;
        }
        else
        {
            m_Health = _value;
        }
    }

    public virtual void OnDeath()
    {

    }

    void UpdateHealthObject(int _value)
    {
        if (m_HealthVisual == null)
            return;

        float fHealth = (float)_value / (float)m_MaxHealth;
        m_HealthVisual.SetHealth(fHealth);
    }

    public bool isAlive
    { get { return m_Health > 0; } }

    public int Health
    {
        get { return m_Health; }
    }

    public int MaxHealth
    {
        get { return m_MaxHealth; }
    }

    public void TakeDamage(int _damage)
    {
        SetHealth(m_Health - _damage);
    }

    public void Heal(int _health)
    {
        SetHealth(m_Health + _health);
    }

    public void Respawn()
    {
        m_Health = m_SpawnHealth;
    }
}
