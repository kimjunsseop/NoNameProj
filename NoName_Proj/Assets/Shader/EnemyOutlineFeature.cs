using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EnemyOutlineFeature : ScriptableRendererFeature
{
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private LayerMask enemyLayer;

    private EnemyOutlinePass pass;

    public override void Create()
    {
        pass = new EnemyOutlinePass(outlineMaterial, enemyLayer);
        pass.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (outlineMaterial == null)
            return;

        renderer.EnqueuePass(pass);
    }
}