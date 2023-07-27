using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public Button StartButton;
    public Button ControlsButton;
    public Button BackButton;
    public GameObject image;
    public Image ControlImage;
    private AudioSource As;
    public AudioClip Hover;
    public AudioClip Click;

    void Start()
    {
        As = GetComponent<AudioSource>();
        ControlImage.enabled = false;
        ControlImage.gameObject.SetActive(false);
        BackButton.enabled = false;
        StartButton.onClick.AddListener(StartBtn);
        ControlsButton.onClick.AddListener(ControlBtn);
        BackButton.onClick.AddListener(BackBtn);
    }

    void StartBtn()
    {
        As.PlayOneShot(Click);
        SceneManager.LoadScene("SampleScene");
    }

    void ControlBtn()
    {
        As.PlayOneShot(Click);
        ControlImage.gameObject.SetActive(true);
        image.SetActive(false);
        BackButton.enabled = true;
        ControlImage.enabled = true;
    }

    void BackBtn()
    {
        As.PlayOneShot(Click);
        ControlImage.gameObject.SetActive(false);
        image.SetActive(true);
        BackButton.enabled = false;
        ControlImage.enabled = false;
    }

    public void HoverStart()
    {
        As.PlayOneShot(Hover);
        StartButton.gameObject.GetComponent<Image>().color = Color.black;
    }
    public void NotHoverStart() => StartButton.gameObject.GetComponent<Image>().color = Color.white;
    public void HoverControl()
    {
        As.PlayOneShot(Hover);
        ControlsButton.gameObject.GetComponent<Image>().color = Color.black;
    }
    public void NotHoverControl() => ControlsButton.gameObject.GetComponent<Image>().color = Color.white;
    public void HoverBack()
    {
        As.PlayOneShot(Hover);
        BackButton.gameObject.GetComponent<Image>().color = Color.black;
    }
    public void NotHoverBack() => BackButton.gameObject.GetComponent<Image>().color = Color.white;
} 
