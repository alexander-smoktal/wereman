using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarComponent : HealthVisualizerComponent
{
    [Header("Dependencies")]
    [SerializeField] private GameObject m_HealthObject = null;

    void UpdateHealthObject(float _value)
    {
        Debug.Assert(m_HealthObject != null, "Invalid health object");
        if (m_HealthObject == null)
            return;

        Vector3 scale = m_HealthObject.transform.localScale;
        scale.x = _value;

        m_HealthObject.transform.localScale = scale;
    }

    public override void SetHealth(float _value)
    {
        _value = Mathf.Clamp(_value, 0, 1);

        UpdateHealthObject(_value);
    }
}
