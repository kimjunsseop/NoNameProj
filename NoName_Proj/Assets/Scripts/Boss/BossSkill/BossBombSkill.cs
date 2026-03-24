using UnityEngine;
using System.Collections;

public class BossBombSkill : BossSkill
{
    public float warningTime = 1.5f;

    public GameObject markerPrefab;
    public GameObject warningUI;
    public GameObject projectilePrefab;

    public Transform firePoint; // 머리 위치

    public float trackingTime = 1.2f; // 유도 시간

    protected override IEnumerator Execute()
    {
        Transform targetPos = brain.player;

        // 1. 마크 생성
        GameObject marker = Instantiate(markerPrefab, targetPos.position, Quaternion.Euler(90,0,0));
        var follow = marker.GetComponent<MarkerFollow>();
        follow.target = targetPos;
        follow.trackingTime = trackingTime;

        // 2. 경고 UI
        if (warningUI != null)
        {
            warningUI.SetActive(true);
        }

        // 3. 대기
        yield return new WaitForSeconds(warningTime);

        if (marker != null) 
        {
            Destroy(marker);
        }

        if (warningUI != null) 
        {
            warningUI.SetActive(false);
        }
        // 4. 발사
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        proj.GetComponent<BossProjectile>().Init(targetPos, trackingTime);
    }
}