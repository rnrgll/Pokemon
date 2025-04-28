using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public UnityEvent OnGameOver;
    public bool gameStart;
    public bool gameOver;
    public int LevelUpPoint1 = 10;
    public int LevelUpPoint2 = 10;
    public GameObject gameOverUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            gameObject.AddComponent<GameManager>();
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
