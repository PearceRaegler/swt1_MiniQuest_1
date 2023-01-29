using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public HealthBar healthBar;
    public int maxHealth = 5;
    public int currentHealth;
    public float speed;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI timerText;
    public GameObject winTextObject;

    private Rigidbody rb;
    private int count;
    private float startTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        count = 0;
        startTime = Time.time;

        SetCountText();
        winTextObject.SetActive(false);
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();

        if(count >= 8)
        {
            winTextObject.SetActive(true);
        }
    }

    void Update()
    {
        float time = Time.time - startTime;
        string minutes = ((int)time / 60).ToString();
        string seconds = (time % 60).ToString("f2");
        timerText.text = minutes + ":" + seconds;
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Wall"))
        {
            TakeDamage(1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;

            SetCountText();
        }
        // Health Recovery System

        // If player collides with health pack
        if (other.gameObject.CompareTag("Health"))
        {
            // Remove the pack from the game world
            other.gameObject.SetActive(false);

            // Recover 1 health point
            TakeDamage(-1);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        if(currentHealth < 1)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}