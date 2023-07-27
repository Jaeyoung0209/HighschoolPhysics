using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    private CameraController maincam;
    public GameObject self;

    private void Start()
    {
        maincam = GameObject.Find("Main Camera").GetComponent<CameraController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject != self)
            StartCoroutine("ShakeCoroutine");
    }

    IEnumerator ShakeCoroutine()
    {
        maincam.Shake();
        //self.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
