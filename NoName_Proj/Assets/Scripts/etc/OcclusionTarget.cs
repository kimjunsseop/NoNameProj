using UnityEngine;

public class OcclusionTarget : MonoBehaviour
{
    void OnEnable()
    {
        OcclusionTargetRegistry.Instance?.Register(transform);
    }

    void OnDisable()
    {
        OcclusionTargetRegistry.Instance?.Unregister(transform);
    }
}