using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TitleController : MonoBehaviour
{
    [SerializeField] GameObject titleWindow;
    [SerializeField] GameObject stageSelectWindow;

    [SerializeField] GameObject ClearWindow;

    void Start()
    {
        titleWindow.SetActive(true);
        stageSelectWindow.SetActive(false);

        if (GameController.isGameClear)
        {
            ClearWindow.SetActive(true);
        }
    }

    // ステージ選択画面を表示
    public void OpenStageSelectWindow()
    {
        titleWindow.SetActive(false);
        stageSelectWindow.SetActive(true);
    }

    // ゲームを終了する
    public void GameExit()
    {
        Application.Quit();
    }

    // ステージ選択画面にて選択したボタンに指定した数値を代入
    public void SelectStage(int selectStageNum)
    {
        GameController.stageNum = selectStageNum;
    }
}
