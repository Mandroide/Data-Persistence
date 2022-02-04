using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour, ISaveable
{
    public static MenuUI Instance { get; private set; }
    [SerializeField] private UnityEngine.UI.Button _buttonStart;
    [SerializeField] private TMPro.TMP_InputField _inputFieldName;
    public SaveData savedCurrentScore { get; private set; }

    [Header("Highest Score")]
    [SerializeField] private UnityEngine.UI.Text _scoreText;
    [SerializeField] private UnityEngine.UI.Text _nameText;
    public SaveData savedHighestScore { get; private set; }
    
    private readonly int _sceneBuildIndex = 1;
    private readonly int _nameMinLength = 3;

    public TMPro.TMP_InputField inputFieldName { get { return _inputFieldName; } }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            savedHighestScore = new SaveData();
            savedCurrentScore = new SaveData();
            DontDestroyOnLoad(gameObject);
            LoadHighestScore();
        }
    }

    private void LoadHighestScore()
    {
        IEnumerable<MenuUI> a_saveables = new List<MenuUI>() { this };
        SaveDataManager.LoadJsonData(a_saveables);

        _nameText.text = $"{_nameText.text} {savedHighestScore.name}";
        _scoreText.text = $"{_scoreText.text} {savedHighestScore.score}";
    }

    private void Update()
    {
        if (_buttonStart.interactable && Input.GetKeyDown(KeyCode.Return))
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneBuildIndex);
        savedCurrentScore.name = inputFieldName.text;
    }

    public void EnableStartButton(string name)
    {
        if (_buttonStart != null)
        {
            _buttonStart.interactable = !string.IsNullOrEmpty(name) && name.Length >= _nameMinLength;
        }
    }


    void ISaveable.PopulateSaveData(SaveData a_SaveData)
    {
        throw new System.NotImplementedException();
    }

    void ISaveable.LoadFromSaveData(SaveData a_SaveData)
    {
        savedHighestScore.name = a_SaveData.name;
        savedHighestScore.score = a_SaveData.score;
    }
}
