using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AutoTilingMaterialComponent : MonoBehaviour
{
    enum Orientation
    {
        XY,
        YZ,
        XZ
    }

    #region Serializable
    [Header("Dependencies")]
    [SerializeField] private int m_MaterialIndex = 0;

    [Header("Properties")]
    [SerializeField] private Vector2     m_Size = new Vector2(1.0f, 1.0f);
    [SerializeField] private Orientation m_Orientation = Orientation.XY;
    #endregion

    #region Members
    private Material m_Material = null;
    #endregion

    #region MonoBehaviour
    public void OnValidate()
    {
        UpdateInEditor();
    }

    void Awake()
    {
        if(Application.isPlaying)
            InitMaterial();
        else
            InitSharedMaterial();
    }

    void Start ()
    {
        UpdateMaterial();
    }
	
	void Update ()
    {
        UpdateInEditor();
    }
    #endregion

    #region Init Material
    void UpdateInEditor()
    {
        if (Application.isPlaying)
            return;

        InitSharedMaterial();
        UpdateMaterial();
    }

    private void InitSharedMaterial()
    {
        Renderer renderer = GetComponent<MeshRenderer>();
        Debug.Assert(renderer != null, "Failed to find the mesh renderer component");
        if (renderer == null)
            return;

        Debug.Assert(0 <= m_MaterialIndex, "Invalid material index");
        Debug.Assert(m_MaterialIndex < renderer.sharedMaterials.Length, "Invalid material index");
        if ((0 > m_MaterialIndex) || (m_MaterialIndex >= renderer.sharedMaterials.Length))
            return;

        m_Material = new Material(renderer.sharedMaterials[m_MaterialIndex]);
        renderer.sharedMaterial = m_Material;
    }

    private void InitMaterial()
    {
        Renderer renderer = GetComponent<MeshRenderer>();
        Debug.Assert(renderer != null, "Failed to find the mesh renderer component");
        if (renderer == null)
            return;

        Debug.Assert(0 <= m_MaterialIndex, "Invalid material index");
        Debug.Assert(m_MaterialIndex < renderer.materials.Length, "Invalid material index");
        if ((0 > m_MaterialIndex) || (m_MaterialIndex >= renderer.materials.Length))
            return;

        m_Material = renderer.materials[m_MaterialIndex];
    }
    #endregion

    #region Update Material
    private Vector2 GetObjectScale()
    {
        Vector2 scale;
        switch (m_Orientation)
        {
            case Orientation.XY:
                scale = new Vector2(transform.lossyScale.x, transform.lossyScale.y);
                break;

            case Orientation.YZ:
                scale = new Vector2(transform.lossyScale.y, transform.lossyScale.z);
                break;

            case Orientation.XZ:
                scale = new Vector2(transform.lossyScale.x, transform.lossyScale.z);
                break;

            default:
                Debug.Assert(false, "Invalid orientation");
                scale = new Vector2();
                break;
        }

        return scale;
    }

    private void UpdateMaterial()
    {
        Debug.Assert(m_Material != null, "Invalid material");
        if (m_Material == null)
            return;
        
        Vector2 objectScale = GetObjectScale();
        Vector2 scale = new Vector2(objectScale.x / m_Size.x, objectScale.y / m_Size.y);
        m_Material.SetTextureScale("_MainTex", scale);
    }
    #endregion
}
