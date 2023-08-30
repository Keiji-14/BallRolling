using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [SerializeField] GameObject[] stage;

    void Start()
    {
        // ステージ選択画面にて選択されたステージを生成する
        Instantiate(stage[GameController.stageNum], Vector3.zero, Quaternion.identity);
    }

    public void NextStage()
    {
        GameController.stageNum += 1;

        if (stage.Length <= GameController.stageNum)
        {
            GameController.isGameClear = true;
        }
    }
}
