using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    Rigidbody2D rb;

    int moneyAmout;
    public Text moneyAmoutText;

    public AudioSource audioSource;
    public AudioSource Fly;
    private bool isJumping;
    public GameManager gameManager;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GetComponent<AudioSource>();
        isJumping = false;
        Time.timeScale = 1;

        rb.linearVelocity = new Vector3(0, 7, 0);
    }

    // Update is called once per frame
    void Update()
    {
        

        moneyAmout = PlayerPrefs.GetInt("MoneyAmout");
        moneyAmoutText.text = "" + moneyAmout.ToString() + "";


        if (Input.GetMouseButtonDown(0) && isJumping == false)
        {
            rb.linearVelocity = new Vector3(0, 7, 0);
            isJumping = false;
            Fly.Play();
            
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isJumping = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "GameOver")
        {
            Time.timeScale = 0;
            gameManager.GameOver();
        }


        if (collision.tag == "StarClam")
        {
            moneyAmout += 1;
            PlayerPrefs.SetInt("MoneyAmout", moneyAmout);
            audioSource.Play();
            
        }
    }
}
