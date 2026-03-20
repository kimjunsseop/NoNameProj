using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OcclusionFader : MonoBehaviour
{
    [Header("Settings")]
    public LayerMask wallLayer;
    public float fadeAlpha = 0.3f;
    public float checkInterval = 0.05f;

    private float timer;

    private Camera cam;
    private MaterialPropertyBlock mpb;

    private HashSet<Renderer> prevRenderers = new HashSet<Renderer>();
    private HashSet<Renderer> currentRenderers = new HashSet<Renderer>();

    void Awake()
    {
        cam = GetComponent<Camera>();
        mpb = new MaterialPropertyBlock();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer < checkInterval) return;
        timer = 0f;

        UpdateOcclusion();
    }

    void UpdateOcclusion()
    {
        if (OcclusionTargetRegistry.Instance == null) return;

        var targets = OcclusionTargetRegistry.Instance.Targets;
        if (targets.Count == 0) return;

        // 🔥 Bounds 생성
        bool initialized = false;
        Bounds bounds = new Bounds();

        foreach (var t in targets)
        {
            if (t == null) continue;

            if (!initialized)
            {
                bounds = new Bounds(t.position, Vector3.zero);
                initialized = true;
            }
            else
            {
                bounds.Encapsulate(t.position);
            }
        }

        if (!initialized) return;

        Vector3 camPos = cam.transform.position;
        Vector3 dir = bounds.center - camPos;
        float dist = dir.magnitude;

        RaycastHit[] hits = Physics.BoxCastAll(
            camPos,
            bounds.extents,
            dir.normalized,
            Quaternion.identity,
            dist,
            wallLayer
        );

        currentRenderers.Clear();

        foreach (var hit in hits)
        {
            Renderer r = hit.collider.GetComponent<Renderer>();
            if (r != null)
            {
                currentRenderers.Add(r);
            }
        }

        // 🔥 Fade 적용
        foreach (var r in currentRenderers)
        {
            SetAlpha(r, fadeAlpha);
        }

        // 🔥 복구
        foreach (var r in prevRenderers)
        {
            if (!currentRenderers.Contains(r))
            {
                SetAlpha(r, 1f);
            }
        }

        // 🔥 상태 갱신
        prevRenderers.Clear();
        foreach (var r in currentRenderers)
        {
            prevRenderers.Add(r);
        }
    }

    void SetAlpha(Renderer r, float alpha)
    {
        r.GetPropertyBlock(mpb);
        mpb.SetFloat("_Alpha", alpha);
        r.SetPropertyBlock(mpb);
    }
}