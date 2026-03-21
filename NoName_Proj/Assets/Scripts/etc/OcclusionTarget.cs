using UnityEngine;

public class OcclusionTarget : MonoBehaviour
{
    void OnEnable()
    {
        if (OcclusionTargetRegistry.Instance != null)
            OcclusionTargetRegistry.Instance.Register(transform);
    }

    void OnDisable()
    {
        if (OcclusionTargetRegistry.Instance != null)
            OcclusionTargetRegistry.Instance.Unregister(transform);
    }
}