using UnityEngine;
using System.Collections;

public abstract class BossSkill : MonoBehaviour
{
    protected BossBrain brain;

    public float cooldown = 3f;

    protected bool isRunning = false;

    public void Init(BossBrain brain)
    {
        this.brain = brain;
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    public IEnumerator Run()
    {
        isRunning = true;
        yield return Execute();
        yield return new WaitForSeconds(cooldown);
        isRunning = false;
    }

    protected abstract IEnumerator Execute();
}