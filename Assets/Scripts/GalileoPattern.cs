using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalileoPattern : MonoBehaviour, IPattern
{
    private List<System.Action> actionlist = new List<System.Action>();
    private Animator animator;
    private GameObject Player;
    public GameObject bullet;
    private int patternnumber = 1;
    private void Awake()
    {
        Player = GameObject.Find("Player");
        actionlist.Add(GalileoStartEvent);
        actionlist.Add(Pattern1);
        actionlist.Add(Pattern2);
        actionlist.Add(Pattern3);
        actionlist.Add(Pattern4);
        animator = GetComponent<Animator>();
        Signup();
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

    public void GalileoStartEvent()
    {
        animator.SetTrigger("Enter");
        StartCoroutine("CheckAnimationFinished");
    }

    public void Pattern1() => StartCoroutine("Pattern1Coroutine");

    IEnumerator Pattern1Coroutine()
    {
        animator.SetTrigger("Pattern1");
        GameObject pisatower = transform.GetChild(1).gameObject.transform.GetChild(4).gameObject;
        pisatower.SetActive(false);
        yield return new WaitForSeconds(12f);
        pisatower.transform.position = new Vector2(Player.transform.position.x, -9); 
        pisatower.SetActive(true);
        for (int i = 0; i < 110; i++)
        {
            pisatower.transform.position = new Vector2(Player.transform.position.x, -9);
            yield return new WaitForSeconds(0.05f);
        }
        for(int i = 0; i < 10; i++)
        {
            pisatower.transform.position = new Vector2(pisatower.transform.position.x, pisatower.transform.position.y + 0.3f);
            yield return new WaitForSeconds(0.025f);
        }
        for (int i = 0; i < 10; i++)
        {
            pisatower.transform.position = new Vector2(pisatower.transform.position.x, pisatower.transform.position.y - 0.3f);
            yield return new WaitForSeconds(0.025f);
        }

        for (int i = 0; i < 60; i++)
        {
            pisatower.transform.position = new Vector2(Player.transform.position.x, -9);
            yield return new WaitForSeconds(0.05f);
        }
        for (int i = 0; i < 10; i++)
        {
            pisatower.transform.position = new Vector2(pisatower.transform.position.x, pisatower.transform.position.y + 0.3f);
            yield return new WaitForSeconds(0.025f);
        }
        for (int i = 0; i < 10; i++)
        {
            pisatower.transform.position = new Vector2(pisatower.transform.position.x, pisatower.transform.position.y - 0.3f);
            yield return new WaitForSeconds(0.025f);
        }

        pisatower.SetActive(false);

        yield return new WaitForSeconds(5f);

        GameObject bullet1 = Instantiate(bullet, new Vector2(-1.3f, 4), Quaternion.identity);
        GameObject bullet2 = Instantiate(bullet, new Vector2(-1.3f, 4), Quaternion.identity);
        bullet1.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        bullet2.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

        yield return new WaitForSeconds(1f);

        bullet1.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        bullet2.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

        bullet1.GetComponent<Rigidbody2D>().AddForce(new Vector2(50, 0));
        bullet2.GetComponent<Rigidbody2D>().AddForce(new Vector2(200, 0));

        yield return new WaitForSeconds(3f);

        FinishEvent();
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
        animator.SetTrigger("Pattern4");
        StartCoroutine("CheckAnimationFinished");
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


}
