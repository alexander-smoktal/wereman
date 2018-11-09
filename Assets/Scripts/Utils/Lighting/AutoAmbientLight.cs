using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAmbientLight : MonoBehaviour
{
    const string c_AssertPrefix = "[AutoAmbientLight] ";

    [SerializeField] Color m_ColorMultiplier = new Color(1, 1, 1);

    private Light m_Light = null;

	void Awake ()
    {
        Init();
        UpdateAmbient();
    }

    private void Init()
    {
        m_Light = gameObject.GetComponent<Light>();
        Debug.Assert(m_Light != null, c_AssertPrefix + "Failed to get light component");
    }

    private void UpdateAmbient()
    {
        Debug.Assert(m_Light != null, c_AssertPrefix + "Invalid light component");
        if (m_Light == null)
            return;

        RenderSettings.ambientLight = m_Light.color * m_ColorMultiplier;
    }
}
