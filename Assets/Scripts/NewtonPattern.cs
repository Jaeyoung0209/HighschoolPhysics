using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewtonPattern : MonoBehaviour, IPattern
{
    private int patternnumber = 0;
    private GameObject Player;
    public GameObject smallapple;
    public GameObject bigapple;
    public GameObject Square;
    public GameObject fbdbox;
    public GameObject bullet;

    private List<System.Action> actionlist = new List<System.Action>();
    private Animator animator;


    private void Awake()
    {
        Player = GameObject.Find("Player");
        actionlist.Add(NewtonStartEvent);
        actionlist.Add(Pattern1);
        actionlist.Add(Pattern2);
        actionlist.Add(Pattern3);
        actionlist.Add(Pattern4);
        animator = GetComponent<Animator>();
        Signup();
    }

    public void NewtonStartEvent()
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
        GameObject squareinst = Instantiate(Square, Player.transform.position, Quaternion.identity);
        
        squareinst.transform.localScale = new Vector2(2, 20);

        StartCoroutine("Pattern2Coroutine", squareinst);
    }

    IEnumerator Pattern2Coroutine(GameObject square)
    {
        List<GameObject> fbdboxes = new List<GameObject>();
        for (int j = 0; j < 5; j++)
        {
            square.SetActive(true);
            for (int i = 0; i < 30; i++)
            {
                square.transform.position = Player.transform.position;
                yield return new WaitForSeconds(0.05f);
            }
            square.SetActive(false);
            fbdboxes.Add(Instantiate(fbdbox, new Vector2(Player.transform.position.x, 7.5f), Quaternion.identity));
            yield return new WaitForSeconds(1.5f);
        }

        yield return new WaitForSeconds(3);

        GameObject walls = transform.GetChild(12).gameObject;
        walls.SetActive(true);

        GameObject cannon = transform.GetChild(7).gameObject;
        cannon.SetActive(true);
        GameObject cannon2 = transform.GetChild(8).gameObject;
        
        cannon.transform.position = new Vector2(-7, 2.5f);
        cannon2.SetActive(true);
        cannon2.transform.position = new Vector2(7, 2.5f);
        for(int i = 0; i < 40; i++)
        {
            cannon.transform.position = new Vector2(cannon.transform.position.x + 0.1f, 2.5f);
            cannon2.transform.position = new Vector2(cannon2.transform.position.x - 0.1f, 2.5f);
            yield return new WaitForSeconds(0.025f);
        }

        List<GameObject> bullets = new List<GameObject>();

        for(int i = 0; i < 25; i++)
        {
            if (i % 2 == 0)
            {
                bullets.Add(Instantiate(bullet, new Vector2(-1, 3.3f), Quaternion.identity));
                bullets[i].GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(0, 350), 0));
            }
            else
            {
                bullets.Add(Instantiate(bullet, new Vector2(1, 3.3f), Quaternion.identity));
                bullets[i].GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-350, 0), 0));
            }
            
            yield return new WaitForSeconds(0.5f);
        }

        for (int i = 0; i < 40; i++)
        {
            cannon.transform.position = new Vector2(cannon.transform.position.x - 0.1f, 2.5f);
            cannon2.transform.position = new Vector2(cannon2.transform.position.x + 0.1f, 2.5f);
            yield return new WaitForSeconds(0.025f);
        }

        cannon.SetActive(false);
        cannon2.SetActive(false);
        walls.SetActive(false);

        fbdboxes.Add(square);
        for (int i = 0; i < fbdboxes.Count; i++)
            Destroy(fbdboxes[i]);

        yield return new WaitForSeconds(2);

        FinishEvent();
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
    public void Pattern3()
    {
        StartCoroutine("AppleRain");
    }

    public void Pattern4()
    {
        animator.SetTrigger("Pattern4");
        StartCoroutine("CheckAnimationFinished");
    }

    IEnumerator AppleRain()
    {
        List<GameObject> applerain = new List<GameObject>();
        for (int i = 0; i < 20; i++)
        {
            if(i % 3 == 0)
            {
                applerain.Add(Instantiate(smallapple, new Vector2(Player.transform.position.x + Random.Range(-0.05f, 0.05f), 6), Quaternion.identity));
            }
            else
                applerain.Add(Instantiate(smallapple, new Vector2(Random.Range(-4, 4), 6), Quaternion.identity));
            yield return new WaitForSeconds(Random.Range(0.35f, 0.65f));
        }
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < applerain.Count; i++)
            Destroy(applerain[i]);

        animator.SetTrigger("Pattern3");
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

    public void Stop() => animator.SetTrigger("Stop");
}
