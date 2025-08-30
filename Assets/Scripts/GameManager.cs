using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public Button backButton;

    private MapType mapSelected;
    public int goalScore = 5;
    public int currentScore = 0;
    private bool goalReached = false;

    public Text scoreText;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject scoreObj = GameObject.FindWithTag("ScoreText");
        if (scoreObj != null)
        {
            scoreText = scoreObj.GetComponent<Text>();
            UpdateScoreText();
        }

        GameObject startObj = GameObject.FindWithTag("StartButton");
        if (startObj != null)
        {
            startButton = startObj.GetComponent<Button>();
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(LoadNextMap);
        }

        GameObject yesObj = GameObject.FindWithTag("YesButton");
        if (yesObj != null)
        {
            yesButton = yesObj.GetComponent<Button>();
            yesButton.onClick.RemoveAllListeners();
            yesButton.onClick.AddListener(YesButton);
        }

        GameObject noObj = GameObject.FindWithTag("NoButton");
        if (noObj != null)
        {
            noButton = noObj.GetComponent<Button>();
            noButton.onClick.RemoveAllListeners();
            noButton.onClick.AddListener(NoButton);
        }

        GameObject backObj = GameObject.FindWithTag("BackButton");
        if (backObj != null)
        {
            backButton = backObj.GetComponent<Button>();
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(BackButton);
        }
    }

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

        int randomIndex = UnityEngine.Random.Range(3, sceneNames.Length);
        string sceneToLoad = sceneNames[randomIndex];
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadNextMap()
    {
        goalReached = false;
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
            SceneManager.LoadScene(0);
        }
        else
        {
            currentScore += 1;
            Debug.Log("Yes - Current score: " + currentScore);
            UpdateScoreText();
            if (CheckWinCondition() == true)
            {
                SceneManager.LoadScene(2);
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
            SceneManager.LoadScene(0);
        }
        else
        {
            currentScore += 1;
            Debug.Log("No - Current score: " + currentScore);
            UpdateScoreText();
            if (CheckWinCondition() == true)
            {
                SceneManager.LoadScene(2);
                return;
            }
            
            LoadNextMap();
        }
    }

    public void BackButton()
    {
        SceneManager.LoadScene(0);
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
