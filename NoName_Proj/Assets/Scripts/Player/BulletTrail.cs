using UnityEngine;
using System.Collections;

public class BulletTrail : MonoBehaviour
{
    public float speed = 300f;

    Vector3 target;

    public void Init(Vector3 targetPos)
    {
        target = targetPos;

        TrailRenderer tr = GetComponent<TrailRenderer>();

        if (tr != null)
        {
            tr.Clear();
        }

        StartCoroutine(MoveTrail());
    }

    IEnumerator MoveTrail()
    {
        Vector3 start = transform.position;

        float distance = Vector3.Distance(start, target);

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * speed / distance;

            transform.position = Vector3.Lerp(start, target, t);

            yield return null;
        }

        transform.position = target;

        Destroy(gameObject, 1f);
    }
}