using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : class, new()
{
    static Singleton<T> instance;

    public static T Instance
    {
        get
        {
            return instance as T;
        }
    }

    protected virtual void Awake()
    {
        instance = this;

    }
}


public interface IPattern
{
    void Signup();
    void FinishEvent();
}


public class EventManager : Singleton<EventManager>
{
    private Dictionary<int, List<System.Action>> eventmap = new Dictionary<int, List<System.Action>>();
    public Text timer;
    public float elapsedtime = 0;
    private bool timergoing = false;
    private System.TimeSpan timeplaying;
    public int phase = 15;
    private int previousphase = 14;
    private GameObject maincam;
    public GameObject blind;
    private GameObject Player;
    private bool debuffed = false;
    public GameObject Heal;
    private bool giveheal = true;
    public bool phasechange = false;

    public void Apply(int pattern, List<System.Action> actionlist)
    {
        eventmap.Add(pattern, actionlist);
    }

    private void Start() => StartCoroutine("StartCoroutine");

    IEnumerator StartCoroutine()
    {
        blind.SetActive(false);
        Player = GameObject.Find("Player").gameObject;
        maincam = GameObject.Find("Main Camera").gameObject;
        DontDestroyOnLoad(this.gameObject);
        timer.text = "00:00:00";
        timergoing = true;
        elapsedtime = 0f;
        StartCoroutine("UpdateTime");
        yield return new WaitForSeconds(2);
        Finished();
    }

    IEnumerator UpdateTime()
    {
        while (timergoing)
        {
            elapsedtime += Time.deltaTime;
            timeplaying = System.TimeSpan.FromSeconds(elapsedtime);
            timer.text = timeplaying.ToString("mm':'ss':'ff");

            yield return null;
        }

    }

    private void EndCounter()
    {
        timergoing = false;
    }

    public void Finished() => StartCoroutine("FinishCoroutine");

    IEnumerator FinishCoroutine()
    {
        yield return new WaitForSeconds(2.5f);
        Finish();
    }

    private void Finish()
    {


        if (phase >= 15 || phasechange)
        {
            phasechange = true;
            if (!debuffed)
            {
                int temp = Random.Range(0, 10);
                if (temp <= 1)
                {
                    int temp2 = Random.Range(0, 3);
                    if (temp2 == 0)
                    {
                        eventmap[0][0].Invoke();
                        StartCoroutine("NewtonDebuff");
                    }
                    else if (temp2 == 1)
                    {
                        eventmap[1][0].Invoke();
                        StartCoroutine("GalileoDebuff");
                    }
                    else
                    {
                        eventmap[2][0].Invoke();
                        StartCoroutine("OhmDebuff");
                    }
                    debuffed = true;
                    giveheal = false;
                    return;
                }
            }
            while (true)
            {
                int temp2 = Random.Range(0, 15);
                if (temp2 != previousphase && temp2 % 5 != 0)
                {
                    phase = temp2;
                    previousphase = phase;
                    break;
                }
            }
        }




        eventmap[(int)(phase / 5)][phase % 5].Invoke();
        if (phase % 5 == 0 || Player.GetComponent<PlayerControls>().life >= 4)
            giveheal = false;
        else
            giveheal = true;
        debuffed = false;
        if (giveheal)
        {
            int healtemp = Random.Range(0, 3);
            if (healtemp == 0)
                StartCoroutine("HealCoroutine", new Vector2(0, -0.6f));
            else if (healtemp == 1)
                StartCoroutine("HealCoroutine", new Vector2(2.5f, -0.6f));
            else
                StartCoroutine("HealCoroutine", new Vector2(-2.5f, -0.6f));
        }
        if(!phasechange)
            phase += 1;
    }

    IEnumerator HealCoroutine(Vector2 position)
    {
        GameObject heal = Instantiate(Heal, position, Quaternion.identity);
        yield return new WaitForSeconds(5);
        Destroy(heal);
    }

    IEnumerator NewtonDebuff()
    {
        yield return new WaitForSeconds(10);

        Player.GetComponent<PlayerControls>().speed -= 4;
        yield return new WaitForSeconds(Random.Range(7, 11));
        Player.GetComponent<PlayerControls>().speed += 4;
    }

    IEnumerator GalileoDebuff()
    {
        yield return new WaitForSeconds(10);
        int temp = Random.Range(0, 3);
        if (temp == 0)
        {
            maincam.transform.Rotate(0, 0, 90);
            yield return new WaitForSeconds(Random.Range(8,12));
            maincam.transform.Rotate(0, 0, -90);
        }
        else if(temp == 1)
        {
            maincam.transform.Rotate(0, 0, -90);
            yield return new WaitForSeconds(Random.Range(8, 12));
            maincam.transform.Rotate(0, 0, 90);
        }
        else if(temp == 2)
        {
            maincam.transform.Rotate(0, 0, 180);
            yield return new WaitForSeconds(Random.Range(8, 12));
            maincam.transform.Rotate(0, 0, -180);
        }
    }

    IEnumerator OhmDebuff()
    {
        yield return new WaitForSeconds(10);
        blind.SetActive(true);
        blind.transform.localScale = new Vector2(120, 120);
        for(int i = 0; i < 40; i++)
        {
            blind.transform.localScale = new Vector2(blind.transform.localScale.x - 2.215f, blind.transform.localScale.y - 2.215f);
            yield return new WaitForSeconds(0.025f);
        }

        yield return new WaitForSeconds(Random.Range(9, 11));

        for(int i = 0; i<40; i++)
        {
            blind.transform.localScale = new Vector2(blind.transform.localScale.x + 2.215f, blind.transform.localScale.y + 2.215f);
            yield return new WaitForSeconds(0.025f);
        }

        blind.SetActive(false);
    }

    public void GameOver()
    {
        EndCounter();
        SceneManager.LoadScene("GameOverScene");
    }
}
