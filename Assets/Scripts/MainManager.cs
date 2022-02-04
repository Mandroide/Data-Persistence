using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour, ISaveable
{
    public static MainManager Instance { get; private set; }
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    [Header("Current Score")]
    public Text ScoreText;
    [SerializeField] private Text NameText;
    public SaveData currentScore { get; set; }

    [Header("Highest Score")]
    [SerializeField] private Text highestScoreText;
    [SerializeField] private Text highestNameText;
    public SaveData savedHighestScore { get; set; }

    public GameObject GameOverText;
    
    private bool m_Started = false;
    
    private bool m_GameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        currentScore.score += point;
        ScoreText.text = $"Score : {currentScore.score}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        checkHighestScore();
    }

    private void checkHighestScore()
    {
        if (currentScore.score > savedHighestScore.score)
        {
            SaveScore();
        }
    }

    private void SaveScore()
    {
        // Update display information of highest score
        highestScoreText.text = ScoreText.text;
        highestNameText.text = NameText.text;
        // Update saved highest score
        savedHighestScore.score = currentScore.score;
        savedHighestScore.name = currentScore.name;
        IEnumerable<MainManager> a_saveables = new List<MainManager>() { this };
        SaveDataManager.SaveJsonData(a_saveables);
    }

    void ISaveable.PopulateSaveData(SaveData a_SaveData)
    {
        a_SaveData.name = savedHighestScore.name;
        a_SaveData.score = savedHighestScore.score;
    }

    void ISaveable.LoadFromSaveData(SaveData a_SaveData)
    {
        savedHighestScore.name = a_SaveData.name;
        savedHighestScore.score = a_SaveData.score;
    }

    private void Awake()
    {
        LoadData();
    }

    private void LoadData()
    {
        currentScore = new SaveData(MenuUI.Instance.savedCurrentScore.name);
        NameText.text = $"{NameText.text} {currentScore.name}";
        ScoreText.text = $"{ScoreText.text} {currentScore.score}";

        // Display information of highest score
        savedHighestScore = MenuUI.Instance.savedHighestScore;
        highestNameText.text = $"{highestNameText.text} {savedHighestScore.name}";
        highestScoreText.text = $"{highestScoreText.text} {savedHighestScore.score}";
    }
}
