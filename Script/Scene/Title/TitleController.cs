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

    // �X�e�[�W�I����ʂ�\��
    public void OpenStageSelectWindow()
    {
        titleWindow.SetActive(false);
        stageSelectWindow.SetActive(true);
    }

    // �Q�[�����I������
    public void GameExit()
    {
        Application.Quit();
    }

    // �X�e�[�W�I����ʂɂđI�������{�^���Ɏw�肵�����l����
    public void SelectStage(int selectStageNum)
    {
        GameController.stageNum = selectStageNum;
    }
}
