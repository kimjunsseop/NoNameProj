using System.Collections.Generic;
using UnityEngine;

public class OcclusionTargetRegistry : MonoBehaviour
{
    public static OcclusionTargetRegistry Instance;

    private readonly List<Transform> targets = new List<Transform>();
    public IReadOnlyList<Transform> Targets => targets;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Register(Transform t)
    {
        if (t == null) return;
        if (!targets.Contains(t))
            targets.Add(t);
    }

    public void Unregister(Transform t)
    {
        if (t == null) return;
        targets.Remove(t);
    }
}