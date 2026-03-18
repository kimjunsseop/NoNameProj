// using TMPro;
// using UnityEngine;

// public class DamageText : MonoBehaviour
// {
//     public TextMeshProUGUI text;
//     public Canvas canvas;

//     float timer;
//     Vector3 moveDir;

//     public void Show(int damage, Vector3 worldPos)
//     {
//         transform.position = worldPos;

//         text.text = damage.ToString();

//         timer = 1f;

//         moveDir = new Vector3( Random.Range(-0.5f,0.5f), 1, Random.Range(-0.5f,0.5f));
//     }

//     void Update()
//     {
//         transform.position += moveDir * Time.deltaTime * 2f;

//         timer -= Time.deltaTime;

//         if(timer <= 0)
//         {
//             PoolManager.Instance.Return(gameObject);
//         }
//     }

//     void LateUpdate()
//     {
//         if (Camera.main == null) return;
        
//         canvas.transform.forward = Camera.main.transform.forward;
//     }
// }

using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Canvas canvas;
    public Animator animator;

    public void Show(int damage, Vector3 worldPos)
    {
        transform.position = worldPos;

        text.text = damage.ToString();

        // 🔥 애니메이션 재생
        animator.Play("DamagePop", 0, 0f);
    }

    void LateUpdate()
    {
        if (Camera.main == null) return;

        canvas.transform.forward = Camera.main.transform.forward;
    }

    // 🔥 애니메이션 끝날 때 호출됨
    public void OnAnimationEnd()
    {
        PoolManager.Instance.Return(gameObject);
    }
}