    using System.Collections.Generic;
using UnityEngine;

public class OcclusionTargetRegistry : MonoBehaviour
{
    public static OcclusionTargetRegistry Instance;

    private HashSet<Transform> targets = new HashSet<Transform>();
    public IReadOnlyCollection<Transform> Targets => targets;

    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
        GameEvents.OnPlayerSpawned += RegisterPlayer;
    }

    void OnDisable()
    {
        GameEvents.OnPlayerSpawned -= RegisterPlayer;
    }

    void Start()
    {
        // 🔥 DDOL Player 대응 (씬 진입 시)
        if (GameEvents.Player != null)
        {
            Register(GameEvents.Player);
        }
    }

    void RegisterPlayer(Transform player)
    {
        Register(player);

        // 🔥 안전장치 (OcclusionTarget 없으면 추가)
        if (player.GetComponent<OcclusionTarget>() == null)
        {
            player.gameObject.AddComponent<OcclusionTarget>();
        }
    }

    public void Register(Transform t)
    {
        if (t == null) return;
        targets.Add(t);
    }

    public void Unregister(Transform t)
    {
        if (t == null) return;
        targets.Remove(t);
    }
}