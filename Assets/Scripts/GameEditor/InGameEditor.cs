using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameEditor : MonoBehaviour
{
    #region Types
    public enum GroundType
    {
        Sand,
        Grass,
        Dirt,
    }

    public struct Properties
    {
        public GroundType groundType;
        public bool       stone;

        public CellType.Type GetCellType()
        {
            CellType.Type type = CellType.Type.None;

            switch(groundType)
            {
                case GroundType.Sand:  type = CellType.Type.Sand; break;
                case GroundType.Grass: type = CellType.Type.Grass; break;
                case GroundType.Dirt:  type = CellType.Type.Dirt; break;
            }

            if (stone)
                type = type | CellType.Type.Stone;

            return type;
        }
    }
    #endregion

    #region Members
    private bool       m_IsEdit = false;
    private Properties m_Properties;
    #endregion

    #region Serialize Fields
    [SerializeField] private PlayfieldControl m_PlayfieldControl = null;
    #endregion

    #region Init
    public InGameEditor()
    {
        m_Properties = new Properties
        {
            groundType = GroundType.Sand,
            stone = false,
        };
    }
    #endregion

    #region MonoBehaviour
    void Start ()
    {
        Debug.Assert(m_PlayfieldControl != null, "Invalid Playfield Control");
        if(m_PlayfieldControl != null)
        {
            m_PlayfieldControl.SetEditMode(m_IsEdit);
            m_PlayfieldControl.SetEditorProperties(m_Properties);
        }
	}
	
	void Update ()
    {
		
	}
    #endregion

    #region Control Properties
    private void UpdateEditMode()
    {
        if (m_PlayfieldControl != null)
            m_PlayfieldControl.SetEditMode(m_IsEdit);
    }

    private void UpdateProperties()
    {
        if (m_PlayfieldControl != null)
            m_PlayfieldControl.SetEditorProperties(m_Properties);
    }
    #endregion

    #region Callbacks
    public void OnIsEdit(bool isEdit)
    {
        m_IsEdit = isEdit;

        UpdateEditMode();
    }

    public void OnChangeGroundType(int groundType)
    {
        m_Properties.groundType = (GroundType)groundType;

        UpdateProperties();
    }

    public void OnChangeStone(bool stone)
    {
        m_Properties.stone = stone;

        UpdateProperties();
    }
    #endregion
}
