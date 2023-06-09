using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;// this was used 
using UnityEngine.SceneManagement;// this was used 
using UnityEngine.UI;// this was used for the text
// Andrew Gonzalez notes.
public class GameManager : MonoBehaviour
{
    public List<GameObject> targets;
    public TextMeshProUGUI scoreText; // score board 
    public TextMeshProUGUI gameOverText; // when you lose
    public Button restartButton; // if click it restes the game restart the game
    public GameObject titleScreen;
    public bool isGameActive;
    private int score;
    private float spawnRate = 1.0f;

    IEnumerator SpawnTarget()
    {
        while(isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targets.Count);
            Instantiate(targets[index]);
        }
    }
    public void UpdateScore(int scoreToAdd) // add points 
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score; // score board 
    }

    public void GameOver()
    {
        restartButton.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        isGameActive = false;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame(int difficulty)
    {
        isGameActive = true; // start the game
        score = 0; // start with no points 
        spawnRate /= difficulty; // makes the spawn rate faster depending on which button you click 

        StartCoroutine(SpawnTarget());
        UpdateScore(0);
        // if you click on the Easy, medium or hard then all the other stuff disapears
        titleScreen.gameObject.SetActive(false);  
    }
}
