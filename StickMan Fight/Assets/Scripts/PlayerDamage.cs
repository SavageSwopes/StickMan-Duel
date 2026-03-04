using System.Collections;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [Header("Health System")]
    //public int damage;
    [SerializeField] private int lives = 3;
    [SerializeField] private GameObject heart1;
    [SerializeField] private GameObject heart2;
    [SerializeField] private GameObject heart3;
    public static PlayerDamage Instance; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("UI System")]
    [SerializeField] private GameObject heartsUI;
    //[SerializeField] private GameObject gameOverUI;
    private void Start()
    {
        heartsUI.SetActive(true);
        //gameOverUI.SetActive(false);
    }
    private void Awake()
    { // Pattern to ensure a single instance of LifeSystem
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Instance = this;
        }
    }
    private void LoseLife()
    { // Method to reduce lives
        lives--;
        Debug.Log("Lives remaining: " + lives);
        if (lives <= 0)
        {
            Debug.Log("Game Over!");
            gameObject.SetActive(false);

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Square")
        {
            LoseLife();
        }
    }

    private void Update()
    {
        //Check lives and update hearts UI
        if (lives >= 3)
        {
            heart1.SetActive(true);
            heart2.SetActive(true);
            heart3.SetActive(true);
        }
        else if (lives == 2)
        {
            heart3.SetActive(true);
            heart2.SetActive(true);
            heart1.SetActive(false);
        }
        else if (lives == 1)
        {
            heart3.SetActive(true);
            heart2.SetActive(false);
            heart1.SetActive(false);
        }
        else if (lives <= 0)
        { // Trigger game over
            heart3.SetActive(false);
            //Debug.Log("Game Over");
            //GameOver();
        }
    }
    //Game over method
    /*private void GameOver()
    {
        heartsUI.SetActive(false);
        gameOverUI.SetActive(true);
    }*/
}
    

