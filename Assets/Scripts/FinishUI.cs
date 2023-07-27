using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinishUI : MonoBehaviour
{
    public Button ReButton;
    public Image Envelopeflap;
    public Image Envelopebody;
    public Image card;
    public Image positive;
    public Image negative;
    private AudioSource As;
    public AudioClip Hover;
    public AudioClip Click;
    public AudioClip Drumroll;
    [SerializeField]
    private float score;
    private List<Image> letters = new List<Image>();
    void Start()
    {
        As = GetComponent<AudioSource>();
        score = GameObject.Find("EventManager").GetComponent<EventManager>().elapsedtime/60;
        for (int i = 5; i > -1; i--)
            letters.Add(transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(i).gameObject.GetComponent<Image>());
        for (int i = 0; i < letters.Count; i++)
            letters[i].enabled = false;
        positive.enabled = false;
        negative.enabled = false;
        for (int i = 0; i<19; i+= 3)
        {
            if(score < i)
            {
                letters[i / 3 - 1].enabled = true;
                if (score >= i - 1)
                    positive.enabled = true;
                else if (score < i - 2)
                    negative.enabled = true;
                break;
            }
        }

        ReButton.interactable = false;
        ReButton.onClick.AddListener(ReBtn);
        StartCoroutine("UICoroutine");
    }

    IEnumerator UICoroutine()
    {
        yield return new WaitForSeconds(2);
        for (int i = 0; i < 40; i++)
        {
            Envelopeflap.transform.position = new Vector2(Envelopeflap.transform.position.x, Envelopeflap.transform.position.y + 2.5f);
            yield return new WaitForSeconds(0.025f);
        }
        yield return new WaitForSeconds(1);

        As.PlayOneShot(Drumroll);
        for (int i = 0; i<40; i++)
        {
            card.transform.position = new Vector2(card.transform.position.x, card.transform.position.y + 6.25f);
            yield return new WaitForSeconds(0.025f);
        }

        yield return new WaitForSeconds(3);

        for(int i = 0; i< 40; i++)
        {
            card.transform.position = new Vector2(card.transform.position.x, card.transform.position.y + 3.25f);
            yield return new WaitForSeconds(0.025f);
        }

        yield return new WaitForSeconds(1.5f);

        for(int i = 0; i<40; i++)
        {
            ReButton.gameObject.GetComponent<Image>().color = new Color(ReButton.gameObject.GetComponent<Image>().color.r, ReButton.gameObject.GetComponent<Image>().color.g, ReButton.gameObject.GetComponent<Image>().color.b, ReButton.gameObject.GetComponent<Image>().color.a + 6.375f);
            yield return new WaitForSeconds(0.05f);
        }

        ReButton.interactable = true;

    }

    void ReBtn()
    {
        As.PlayOneShot(Click);
        SceneManager.LoadScene("SampleScene");
    }

    public void HoverReBtn()
    {
        ReButton.gameObject.GetComponent<Image>().color = Color.black;
        As.PlayOneShot(Hover);
    }
    public void NotHoverReBtn() => ReButton.gameObject.GetComponent<Image>().color = Color.white;
}
