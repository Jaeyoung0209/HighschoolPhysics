using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OhmPattern : MonoBehaviour, IPattern
{
    private Animator animator;
    private List<System.Action> actionlist = new List<System.Action>();
    private int patternnumber = 2;
    private GameObject Player;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        actionlist.Add(OhmStartEvent);
        actionlist.Add(Pattern1);
        actionlist.Add(Pattern2);
        actionlist.Add(Pattern3);
        actionlist.Add(Pattern4);
        Player = GameObject.Find("Player");
        Signup();
    }

    public void OhmStartEvent()
    {
        animator.SetTrigger("Enter");
        StartCoroutine("CheckAnimationFinished");
    }

    public void Pattern1()
    {
        animator.SetTrigger("Pattern1");
        StartCoroutine("CheckAnimationFinished");
    }

    public void Pattern2()
    {
        animator.SetTrigger("Pattern2");
        StartCoroutine("CheckAnimationFinished");
    }

    public void Pattern3()
    {
        animator.SetTrigger("Pattern3");
        StartCoroutine("CheckAnimationFinished");
    }

    public void Pattern4()
    {
        StartCoroutine("Pattern4Coroutine");
        animator.SetTrigger("Pattern4");
        StartCoroutine("CheckAnimationFinished");
    }

    IEnumerator Pattern4Coroutine()
    {
        yield return new WaitForSeconds(5.5f);
        Player.GetComponent<PlayerControls>().Slow(true);
        yield return new WaitForSeconds(22);
        Player.GetComponent<PlayerControls>().Slow(false);
    }

    public void Signup() => StartCoroutine("SignupCoroutine");

    IEnumerator SignupCoroutine()
    {
        yield return new WaitForSeconds(1);
        for (int i = 1; i < 5; i++)
        {
            System.Action CurrentIndex = actionlist[i];
            int randomIndex = Random.Range(i, actionlist.Count);
            actionlist[i] = actionlist[randomIndex];
            actionlist[randomIndex] = CurrentIndex;
        }
        EventManager.Instance.Apply(patternnumber, actionlist);
    }

    public void FinishEvent()
    {
        EventManager.Instance.Finished();
    }

    IEnumerator CheckAnimationFinished()
    {

        while (true)
        {
            yield return new WaitForSeconds(3);
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("NoEvent"))
            {
                FinishEvent();
                break;
            }
        }
    }

}
