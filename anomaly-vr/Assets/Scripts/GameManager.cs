using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    enum MapType
    {
        BaseMap,
        AnomalyMap
    }

    public string[] sceneNames;
    public Button startButton;
    public Button yesButton;
    public Button noButton;

    private MapType mapSelected;
    public int goalScore = 5;
    public int currentScore = 0;

    public TextMeshProUGUI scoreText;
    private static T ChooseRandomMapType<T>()
    {
        Array values = Enum.GetValues(typeof(T));
        int randomIndex = UnityEngine.Random.Range(0, values.Length);
        return (T)values.GetValue(randomIndex);
    }

    private void LoadRandomAnomalyScene()
    {
        if (sceneNames.Length == 0)
        {
            Debug.Log("No scenes assigned to loader");
            return;
        }

        int randomIndex = UnityEngine.Random.Range(2, sceneNames.Length);
        string sceneToLoad = sceneNames[randomIndex];

        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadNextMap()
    {
        mapSelected = ChooseRandomMapType<MapType>();
        Debug.Log("Current Map Type: " + mapSelected);

        if (mapSelected == MapType.BaseMap)
        {
            SceneManager.LoadScene(1);
        }
        else if (mapSelected == MapType.AnomalyMap)
        {
            LoadRandomAnomalyScene();
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + currentScore;
    }

    public void YesButton()
    {
        if (mapSelected != MapType.AnomalyMap)
        {
            currentScore = 0;
            UpdateScoreText();
            SceneManager.LoadScene(0);
        }
        else
        {
            currentScore += 1;
            UpdateScoreText();
            LoadNextMap();
        }
    }

    public void NoButton()
    {
        if (mapSelected != MapType.BaseMap)
        {
            currentScore = 0;
            UpdateScoreText();
            SceneManager.LoadScene(0);
        }
        else
        {
            currentScore += 1;
            UpdateScoreText();
            LoadNextMap();
        }
    }
}
