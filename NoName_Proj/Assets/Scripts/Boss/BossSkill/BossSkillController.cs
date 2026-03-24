using UnityEngine;
using System.Collections;

public class BossSkillController : MonoBehaviour
{
    BossSkill[] skills;
    BossBrain brain;

    int currentIndex = 0;
    bool isLoopRunning = false;

    void Awake()
    {
        brain = GetComponent<BossBrain>();
        skills = GetComponents<BossSkill>();

        foreach (var skill in skills)
        {
            skill.Init(brain);
        }
    }

    void OnEnable()
    {
        StartCoroutine(SkillLoop());
    }

    IEnumerator SkillLoop()
    {
        yield return new WaitForSeconds(2f); // 시작 딜레이

        isLoopRunning = true;

        while (isLoopRunning)
        {
            if (skills.Length == 0)
                yield break;

            BossSkill skill = skills[currentIndex];

            yield return StartCoroutine(skill.Run());

            currentIndex++;
            if (currentIndex >= skills.Length)
                currentIndex = 0;
        }
    }
}