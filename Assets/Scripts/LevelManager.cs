using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    enum MapType
    {
        BaseMap,
        AnomalyMap
    }
    public GameObject player;
    public GameObject[] anomalies;
    public Button startButton;
    public Button yesButton;
    public Button noButton;
    public Button backButton;
    public Canvas startCanvas;
    public Canvas gameCanvas;
    public Canvas winCanvas;

    private MapType mapSelected;
    public int goalScore = 5;
    public int currentScore = 0;
    private bool goalReached = false;
    public int level = 1;
    private int randomIndex = 0;
   

    public Text scoreText;


    private static T ChooseRandomMapType<T>()
    {
        Array values = Enum.GetValues(typeof(T));
        int rIndex = UnityEngine.Random.Range(0, values.Length);
        return (T)values.GetValue(rIndex);
    }

    private void LoadRandomAnomaly()
    {
        if (anomalies.Length == 0)
        {
            Debug.Log("No anomalies assigned to loader");
            return;
        }

        randomIndex = UnityEngine.Random.Range(0, anomalies.Length);
        anomalies[randomIndex].SetActive(true);
    }

    public void LoadNextMap()
    {
        goalReached = false;
        mapSelected = ChooseRandomMapType<MapType>();
        Debug.Log("Current Map Type: " + mapSelected);
        anomalies[randomIndex].SetActive(false);

        if (mapSelected == MapType.BaseMap)
        {
            anomalies[randomIndex].SetActive(false);
        }
        else if (mapSelected == MapType.AnomalyMap)
        {
            LoadRandomAnomaly();
        }
        startCanvas.gameObject.SetActive(false);
        gameCanvas.gameObject.SetActive(true);
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }

    public void YesButton()
    {
        if (mapSelected != MapType.AnomalyMap)
        {
            ResetScore();
            LoadNextMap();
        }
        else
        {
            currentScore += 1;
            Debug.Log("Yes - Current score: " + currentScore);
            UpdateScoreText();
            if (CheckWinCondition() == true)
            {
                if (level == 1)
                {
                    SceneManager.LoadScene("Level2");
                }
                else if (level == 2)
                {
                    SceneManager.LoadScene("Level3");
                }
                else
                {
                    gameCanvas.gameObject.SetActive(false);
                    winCanvas.gameObject.SetActive(true);
                }
                return;
            }
            LoadNextMap();
        }
    }

    public void NoButton()
    {
        if (mapSelected != MapType.BaseMap)
        {
            ResetScore();
            LoadNextMap();
        }
        else
        {
            currentScore += 1;
            Debug.Log("No - Current score: " + currentScore);
            UpdateScoreText();
            if (CheckWinCondition() == true)
            {
                if (level == 1)
                {
                    SceneManager.LoadScene("Level2");
                }
                else if (level == 2)
                {
                    SceneManager.LoadScene("Level3");
                }
                else
                {
                    gameCanvas.gameObject.SetActive(false);
                    winCanvas.gameObject.SetActive(true);
                }
                return;
            }

            LoadNextMap();
        }
    }

    public void BackButton()
    {
        SceneManager.LoadScene("Level1");
    }

    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreText();
    }

    private bool CheckWinCondition()
    {
        if (currentScore == goalScore)
        {
            goalReached = true;
            ResetScore();
            Debug.Log("After win - Current score: " + currentScore);
        }

        return goalReached;
    }
}
