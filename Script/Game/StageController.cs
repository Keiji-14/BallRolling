using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [SerializeField] GameObject[] stage;

    void Start()
    {
        // �X�e�[�W�I����ʂɂđI�����ꂽ�X�e�[�W�𐶐�����
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
