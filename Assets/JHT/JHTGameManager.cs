using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class JHTGameManager : MonoBehaviour
{
    public static JHTGameManager Instance;
    public UnityEvent OnGameOver;
    public bool gameStart;
    public bool gameOver;
    public int LevelUpPoint1 = 10;
    public int LevelUpPoint2 = 10;
    public GameObject gameOverUI;
    public UnityEvent<float> OnExpUp;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            gameObject.AddComponent<JHTGameManager>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        OnGameOver.AddListener(GameOverUI);
    }

    private void OnDisable()
    {
        OnGameOver.RemoveListener(GameOverUI);
    }

    public void GameOverUI()
    {
        gameOverUI.SetActive(true);
    }

    

}
