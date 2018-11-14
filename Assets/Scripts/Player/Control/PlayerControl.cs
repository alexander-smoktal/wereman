using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : PlayerControlBase
{
    #region Serializable
    [Header("Movements")]
    [SerializeField] float m_LocomotionWalkSpeed     = 1.0f;

    [Header("Camera")]
    //[SerializeField] Camera m_Camera = null;
    #endregion

    #region Members
    private bool m_IsPaused = false;
    private bool m_PrevPaused = false;

    private PlayerComponent m_PlayerComponent = null;
    #endregion

    #region Properties
    public float LocomotionWalkSpeed
    { get { return m_LocomotionWalkSpeed; } set { m_LocomotionWalkSpeed = value; } }
    #endregion

    #region MonoBehaviour
    void Awake()
    {
        m_PlayerComponent = GetComponent<PlayerComponent>();
        Debug.Assert(m_PlayerComponent != null, "Invalid Character Controller");
    }

    void Start ()
    {

    }

    protected void FixedUpdate()
    {
        if (IsPaused())
            return;


    }
    #endregion

    #region Pause
    public void SetPause(bool _value)
    {
        m_IsPaused = _value;
    }

    public bool IsPaused()
    {
        return m_IsPaused || m_PrevPaused;
    }

    private bool UpdatePause()
    {
        bool paused = IsPaused();
        m_PrevPaused = m_IsPaused;

        return paused;
    }
    #endregion

    #region Controls
    protected override void UpdateControl()
    {
        if (UpdatePause())
            return;

        if (PlayerEvents.HasEvents)
        {
            UpdateMovement();
            UpdateActions();
        }
    }

    private void UpdateMovement()
    {
        UpdateLocomotion();
    }

    private void UpdateLocomotion()
    {
        if (!PlayerEvents.HasEvent(PlayerEvents.InputEvents.MoveTo))
            return;

        Vector2Int position = (Vector2Int)PlayerEvents.GetEvent(PlayerEvents.InputEvents.MoveTo);
        m_PlayerComponent.MoveTo(position);
    }

    private void UpdateActions()
    {
        if (!PlayerEvents.HasEvent(PlayerEvents.InputEvents.Attack))
            return;
        
        m_PlayerComponent.Attack();
    }
    #endregion
}
