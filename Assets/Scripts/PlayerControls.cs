using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    private CameraController maincam;
    private bool invincible = false;
    public float speed;
    private float moveinput;
    private Rigidbody2D rb;
    public Transform groundcheck;
    public float jumpforce;
    public float groundradius;
    private bool facingright = false;
    private bool wasgrounded = true;
    private bool isjumping = false;
    public AudioSource As;
    public AudioSource As2;
    public AudioClip hit;
    public AudioClip jump;
    public AudioClip thud;
    public AudioClip heal;


    public Life HP;

    private bool slow = false;
    public float slowratio = 5;

    [SerializeField]
    private bool isgrounded;
    public LayerMask whatisground;
    public int life = 4;
    void Start()
    {
        maincam = GameObject.Find("Main Camera").GetComponent<CameraController>();
        rb = GetComponent<Rigidbody2D>();
        for (int i = 0; i < 6; i++)
            transform.GetChild(i).gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);

    }

    void Update()
    {
        if (isgrounded == true && wasgrounded == false)
        {
            wasgrounded = true;
            for (int i = 0; i < 5; i++)
                transform.GetChild(i).gameObject.SetActive(false);
            StartCoroutine("LandCoroutine");
        }

        if (transform.GetChild(0).gameObject.activeSelf == false && transform.GetChild(1).gameObject.activeSelf == false && transform.GetChild(2).gameObject.activeSelf == false && transform.GetChild(3).gameObject.activeSelf == false && transform.GetChild(4).gameObject.activeSelf == false && transform.GetChild(5).gameObject.activeSelf == false)
            transform.GetChild(0).gameObject.SetActive(true);

    }
    private void FixedUpdate()
    {
        if (transform.position.y < -6)
            transform.position = new Vector2(transform.position.x, -3.4f);
        moveinput = Input.GetAxis("Horizontal");
        if ((transform.position.x >= 3.9f && moveinput > 0) || (transform.position.x <= -3.9f && moveinput < 0))
            moveinput = 0;
        if(!slow)
            rb.velocity = new Vector2(moveinput * speed, rb.velocity.y);
        else
        {

            rb.velocity = new Vector2(moveinput * speed - slowratio, rb.velocity.y);
        }

        isgrounded = Physics2D.OverlapCircle(groundcheck.position, groundradius, whatisground);

        if (!isgrounded)
        {
            wasgrounded = false;
            transform.GetChild(0).gameObject.SetActive(false);
            if (!isjumping)
                transform.GetChild(4).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(4).gameObject.SetActive(false);
            if (Input.GetKey(KeyCode.UpArrow) && !isjumping)
                StartCoroutine("JumpCoroutine");
        }


        if (facingright == false && moveinput > 0)
            Flip();
        if (facingright == true && moveinput < 0)
            Flip();
    }

    IEnumerator JumpCoroutine()
    {
        isjumping = true;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        As.PlayOneShot(jump);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(true);
        rb.velocity = Vector2.up * jumpforce;
        yield return new WaitForSeconds(0.4f);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(4).gameObject.SetActive(true);
        isjumping = false;
    }

    IEnumerator LandCoroutine()
    {
        transform.GetChild(5).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        As.PlayOneShot(thud);
        transform.GetChild(5).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
    }

    void Flip()
    {
        facingright = !facingright;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!invincible)
        {
            if (collision.tag != "heal")
            {
                As2.PlayOneShot(hit);
                StartCoroutine("DamageCoroutine");
                life -= 1;
                maincam.Shake();
                HP.Damage();
                if (life <= 0)
                    EventManager.Instance.GameOver();
            }
        }
        if(collision.tag == "heal")
        {
            As.PlayOneShot(heal);
            Destroy(collision.gameObject);
            life += 1;
            HP.Heal();
        }
    }

    IEnumerator DamageCoroutine()
    {
        invincible = true;
        for (int j = 0; j < 4; j++) 
        {
            for (int i = 0; i < 6; i++)
                transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().color = new Color(0.25f, 1, 1);
            yield return new WaitForSeconds(0.15f);
            for (int i = 0; i < 6; i++)
                transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            yield return new WaitForSeconds(0.45f);
        }
        invincible = false;
    }

    public void Slow(bool trigger)
    {
        if (trigger)
        {
            slow = true;
        }
        else
        {
            slow = false;
        }
    }
}
