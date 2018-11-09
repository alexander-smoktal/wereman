using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TextAssigner : MonoBehaviour
{
    #region Serializable
    [System.Serializable]
    private class TextAssignable
    {
        [SerializeField] public Text m_TextComponent = null;
        [SerializeField] public string m_TextName = "";
    }

    [SerializeField] private TextAssignable[] m_TextComponents = null;
    #endregion

    #region Members
    private const EventManager.Events UpdateTextEvent = TextManager.UpdateTextEvent;
    private UnityAction m_Listener = null;
    #endregion

    #region MonoBehaviour
    void Start ()
    {
        RegisterListener();
        AssignTexts();
    }

    void OnDestroy()
    {
        UnregisterListener();
    }
    #endregion

    #region Update Text
    private void UpdateText()
    {
        AssignTexts();
    }

    private void AssignTexts()
    {
        if (m_TextComponents == null)
            return;

        foreach(TextAssignable assignable in m_TextComponents)
        {
            Debug.Assert(assignable.m_TextComponent != null, "Invalid text component");
            if (assignable.m_TextComponent == null)
                continue;

            assignable.m_TextComponent.text = TextManager.Instance.GetString(assignable.m_TextName);
        }
    }
    #endregion

    #region Events
    private void RegisterListener()
    {
        Debug.Assert(m_Listener == null, "Listener already registered");
        if (m_Listener == null)
        {
            m_Listener = new UnityAction(UpdateText);
            EventManager.Instance.RegisterListener(UpdateTextEvent, m_Listener);
        }
    }

    private void UnregisterListener()
    {
        if (m_Listener != null)
        {
            if (EventManager.Instantiable)
                EventManager.Instance.UnregisterListener(UpdateTextEvent, m_Listener);

            m_Listener = null;
        }
    }
    #endregion
}
