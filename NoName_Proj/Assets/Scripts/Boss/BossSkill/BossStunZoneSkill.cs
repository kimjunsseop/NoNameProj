using UnityEngine;
using System.Collections;

public class BossStunZoneSkill : BossSkill
{
    public GameObject zonePrefab;

    protected override IEnumerator Execute()
    {
        Vector3 playerPos = brain.player.position;
        Vector3 bossPos = transform.position;

        // 🔥 중간 위치 계산
        Vector3 pos = (playerPos + bossPos) * 0.5f;

        Instantiate(zonePrefab, pos, Quaternion.Euler(90, 0, 0));

        yield return new WaitForSeconds(1f);
    }
}