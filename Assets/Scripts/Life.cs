using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    private List<GameObject> hearts = new List<GameObject>();
    void Start()
    {
        for (int i = 3; i > -1; i--)
            hearts.Add(transform.GetChild(i).gameObject);
    }

    public void Damage()
    {
        for(int i = 0; i < hearts.Count; i++)
        {
            if (hearts[i].transform.GetChild(0).gameObject.activeSelf == true)
            {
                hearts[i].transform.GetChild(0).gameObject.SetActive(false);
                hearts[i].transform.GetChild(1).gameObject.SetActive(true);
                break;
            }
        }
    }

    public void Heal()
    {
        for(int i = hearts.Count-1; i > -1; i--)
        {
            if (hearts[i].transform.GetChild(0).gameObject.activeSelf == false)
            {
                hearts[i].transform.GetChild(0).gameObject.SetActive(true);
                hearts[i].transform.GetChild(1).gameObject.SetActive(false);
                break;
            }
        }
    }
}
