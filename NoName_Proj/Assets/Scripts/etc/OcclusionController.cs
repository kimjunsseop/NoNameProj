using UnityEngine;
using System.Collections.Generic;

public class OcclusionController : MonoBehaviour
{
    public int maxTargets = 20;
    public float maxDistance = 30f; // 최적화용

    private Vector4[] targetPositions;
    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        targetPositions = new Vector4[maxTargets];
    }

    void LateUpdate()
    {
        if (OcclusionTargetRegistry.Instance == null) return;

        var targets = OcclusionTargetRegistry.Instance.Targets;

        int count = 0;

        for (int i = 0; i < targets.Count; i++)
        {
            if (count >= maxTargets) break;

            Transform t = targets[i];
            if (t == null) continue;

            float dist = Vector3.Distance(cam.transform.position, t.position);
            if (dist > maxDistance) continue; // 멀면 제외

            targetPositions[count] = t.position;
            count++;
        }

        Shader.SetGlobalInt("_OcclusionTargetCount", count);
        Shader.SetGlobalVectorArray("_OcclusionTargets", targetPositions);
        Shader.SetGlobalVector("_CameraPos", cam.transform.position);
    }
}