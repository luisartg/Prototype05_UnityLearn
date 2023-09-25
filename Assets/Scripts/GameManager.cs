using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject pausedGameScreen;
    public GameObject gameOverScreen;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public List<GameObject> targets;
    public GameObject slash;

    private float spawnRate = 1.0f;
    private int score = 0;
    [SerializeField]
    private int lives = 3;
    private bool gameActive = false;
    private bool gamePaused = false;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateLivesText();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForPause();
        CheckForSlash();
    }

    private void CheckForPause()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (gameActive)
            {
                UpdatePauseStateTo(!gamePaused);
            }
        }
    }

    private void UpdatePauseStateTo(bool paused)
    {
        gamePaused = paused;
        if (paused)
        {
            Time.timeScale = 0;
            audioSource.Pause();
        }
        else
        {
            Time.timeScale = 1;
            audioSource.Play();
        }
        pausedGameScreen.SetActive(paused);
    }

    private void CheckForSlash()
    {
        if (gameActive && Input.GetMouseButtonDown(0))
        {
            Instantiate(slash);
        }
    }

    IEnumerator SpawnTarget()
    {
        while (gameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targets.Count);
            Instantiate(targets[index]);
        }
    }

    private void UpdateLivesText()
    {
        livesText.text = $"LIVES: {lives}";
    }

    private void GameOver()
    {
        gameActive = false;
        gameOverScreen.SetActive(true);
    }

    public void AddToScore(int points)
    {
        score += points;
        scoreText.text = $"SCORE: {score}";
    }

    public void RemoveLifeBy(int livesLost)
    {
        lives -= livesLost;
        if (lives <= 0)
        {
            lives = 0;
            GameOver();
        }
        UpdateLivesText();
    }

    public bool IsGameActive()
    {
        return gameActive && !gamePaused;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame(int difficulty)
    {
        spawnRate /= difficulty;
        gameActive = true;
        StartCoroutine(SpawnTarget());
        AddToScore(0);
    }
}
