using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static int stageNum;
    public static bool isClear;
    public static bool isGameOver;
    public static bool isGameClear;

    [Header("Canvas")]
    [SerializeField] GameObject gameWindow;
    [SerializeField] GameObject clearWindow;
    [SerializeField] GameObject gameOverWindow;

    void Start()
    {
        // ゲーム開始時にクリア判定をfalseにする
        isClear = false;
        isGameOver = false;
        isGameClear = false;
        clearWindow.SetActive(false);
    }

    void Update()
    {
        Clear();
        GameOver();
    }

    private void Clear()
    {
        // ゴールするまでreturnをかえす
        if (isClear)
        {
            gameWindow.SetActive(false);
            clearWindow.SetActive(true);
        }
    }

    private void GameOver()
    {
        if (isGameOver)
        {
            gameWindow.SetActive(false);
            gameOverWindow.SetActive(true);
        }
    }
}
