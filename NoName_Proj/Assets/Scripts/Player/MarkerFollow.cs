using UnityEngine;

public class MarkerFollow : MonoBehaviour
{
    public Transform target;
    public float trackingTime;

    float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer < trackingTime && target != null)
        {
            transform.position = target.position;
        }
    }
}