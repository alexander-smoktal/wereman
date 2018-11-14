using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : MonoBehaviour
{
    [Header("Dependencies")]
    // [SerializeField] private Camera m_Camera = null;

    // private PlayerControl m_Control = null;
    private HealthComponent m_Health = null;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        // m_Control = GetComponent<PlayerControl>();
        m_Health = GetComponent<HealthComponent>();
    }

    private void Respawn()
    {
        if (m_Health != null)
            m_Health.Respawn();
    }
       
    void Update()
    {
        UpdateDeath();
    }

    private void UpdateDeath()
    {
        if (IsDead())
            OnDeath();
    }

    private bool IsDead()
    {
        bool isDead = (m_Health != null) && !m_Health.isAlive;

        return isDead;
    }
    
    void OnDeath()
    {

    }

    public void MoveTo(Vector2Int position)
    {

    }

    public void Attack()
    {

    }
}