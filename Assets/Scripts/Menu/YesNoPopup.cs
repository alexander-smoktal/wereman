using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YesNoPopup : PopupMenu
{
    [SerializeField] Text m_TextField = null;

    public void SetText(string _text)
    {
        Debug.Assert(m_TextField != null, "Invalid text field");
        if(m_TextField != null)
        {
            m_TextField.text = _text;
        }
    }

}
