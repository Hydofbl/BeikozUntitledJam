using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject box;
    [SerializeField] private GameManager gameManager;

    public bool isBoxModeOn;
    public bool isGameEnd;
    public bool onTheDoor;

    private PlayerMovement playerMovement;

    private void Awake()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        isBoxModeOn = false;
        isGameEnd = false;
        onTheDoor = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !playerMovement.onAir)
        {
            isBoxModeOn = !isBoxModeOn;
            InOut();
        }

        // Restart
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // Next Level
        if (Input.GetKeyDown(KeyCode.E) && gameManager.isGateOpen && onTheDoor)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }

        if (onTheDoor && !gameManager.doorText.activeInHierarchy)
        {
            gameManager.doorText.SetActive(true);
        }

        if (!onTheDoor && gameManager.doorText.activeInHierarchy)
        {
            gameManager.doorText.SetActive(false);
        }
    }

    void InOut()
    {
        if (isBoxModeOn)
        {
            box.SetActive(true);
            body.SetActive(false);
        }
        else
        {
            body.SetActive(true);
            box.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Time.timeScale = 0;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            gameManager.isGameEnd = true;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            // GameOver Screen + Restart / MainMenu button
            // inGame Restart (R)
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Key"))
        {
            Destroy(other.gameObject);
            gameManager.neededKey--;
        }
        else if (other.gameObject.CompareTag("Door"))
        {
            onTheDoor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Door"))
        {
            onTheDoor = false;
        }
    }
}
