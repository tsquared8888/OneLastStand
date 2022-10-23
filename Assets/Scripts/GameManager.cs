using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] enemies = new GameObject[2];
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] Text scoreText;
    [SerializeField] AudioClip scoreSnd;
    [SerializeField] AudioClip gameOverSnd;
    [SerializeField] AudioClip wallSnd;
    [SerializeField] GameObject retryButton, playMainButton, playStoryButton, gameOverMainButton, storyButton, mainStoryButton;

    AudioSource audSrc;
    float spawnTimer = 0.0f;
    float spawnMax = 1.5f;
    float stunnedTimer = 0.0f;
    float stunnedMax = 5.0f;
    bool stunned = false;
    bool gameOver = false;
    bool mainMenu = false;
    bool storyMenu = false;
    int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        audSrc = GetComponent<AudioSource>();  
        scoreText.text = "Score: " + score.ToString();
        gameOverScreen.SetActive(false);
        foreach(GameObject enemy in enemies)
        {
            enemy.SetActive(false);
        }
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            mainMenu = true;
            //EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(playMainButton);
        }
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (!mainMenu && !gameOver && !stunned && spawnTimer > spawnMax)
        {
            int index = Random.Range(0, enemies.Length);
            while (enemies[index].activeSelf)
            {
                index = Random.Range(0, enemies.Length);
            }
            enemies[index].SetActive(true);
            spawnTimer = 0.0f;
        }
        else if (gameOver)
        {
            gameOverScreen.SetActive(true);
        }
        else if (stunned)
        {
            stunnedTimer += Time.deltaTime;
            if (stunnedTimer > stunnedMax)
            {
                stunned = false;
            }
        }
        else if (mainMenu)
        {

        }


    }

    public void Nuked()
    {
        foreach(GameObject enemy in enemies)
        {
            if (enemy.activeSelf)
            {
                enemy.GetComponent<NormalEnemy>().SetHealth();
            }
        }
    }

    public void Stunned()
    {
        stunned = true;
        stunnedTimer = 0.0f;
        foreach (GameObject enemy in enemies)
        {
            if (enemy.activeSelf)
            {
                enemy.GetComponent<NormalEnemy>().Stunned();
            }
        }
    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void AddScore()
    {
        score++;
        scoreText.text = "Score: " + score.ToString();
        audSrc.PlayOneShot(scoreSnd);
    }

    public void GameOver()
    {
        gameOver = true;
        audSrc.PlayOneShot(gameOverSnd);
        EventSystem.current.SetSelectedGameObject(retryButton);
    }

    public void WallBreak()
    {
        audSrc.PlayOneShot(wallSnd);
    }

    public void StoryScreenActive()
    {
        EventSystem.current.SetSelectedGameObject(playStoryButton);
    }

    public void MainMenuScreenActive()
    {
        EventSystem.current.SetSelectedGameObject(playMainButton);
    }
}
