using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private CameraController maincam;
    public GameObject explosion;
    private Rigidbody2D rb;

    private void Start()
    {
        maincam = GameObject.Find("Main Camera").GetComponent<CameraController>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "bullet" && collision.gameObject.tag!="explosion")
        {
            StartCoroutine("HitCoroutine");
        }
    }

    IEnumerator HitCoroutine()
    {
        maincam.Shake();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.GetChild(0).gameObject.SetActive(false);
        GameObject Explosion = Instantiate(explosion, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.05f);
        Explosion.transform.GetChild(0).gameObject.SetActive(false);
        Explosion.transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        Explosion.transform.GetChild(1).gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        Destroy(Explosion);
        Destroy(gameObject);
    }
}
