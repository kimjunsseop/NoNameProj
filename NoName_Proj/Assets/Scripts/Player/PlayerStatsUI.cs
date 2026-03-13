using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatsUI : MonoBehaviour
{
    public Image hpBar;
    public TextMeshProUGUI expText;
    public TextMeshProUGUI killText;
    public TextMeshProUGUI bulletDamageText;
    public StageData nowStage;
    public Animator anim;
    PlayerStats stats;

    void OnEnable()
    {
        GameEvents.OnPlayerSpawned += SetPlayer;
    }

    void Start()
    {
        if(anim == null)
        {
            anim = GetComponentInChildren<Animator>();
        }

        stats.OnHpChanged += UpdateHp;
        stats.OnExpChanged += UpdateExp;
        GameEvents.OnStageProgress += UpdateStageProgress;
        GameEvents.OnBulletDamageChanged += UpdateBulletDamage;

        UpdateHp(stats.currentHp, stats.maxHp);
        UpdateExp(stats.currentExp);
        UpdateStageProgress(0, nowStage.killTarget);
    }

    void SetPlayer(Transform t)
    {
        stats = t.GetComponent<PlayerStats>();
    }


    void UpdateHp(int hp, int max)
    {
        hpBar.fillAmount = (float)hp / max;
        
        if(anim != null)
        {
            anim.Play("HPBar");
        }
    }

    void UpdateExp(int exp)
    {
        expText.text = "EXP : " + exp;
    }
    void UpdateStageProgress(int current, int target)
    {
        killText.text = current + " / " + target;
    }

    void UpdateBulletDamage(float damage)
    {
        bulletDamageText.text = "BulletPower : " + damage;
    }
}