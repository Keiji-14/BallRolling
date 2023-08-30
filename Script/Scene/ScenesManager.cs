using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    [SerializeField] float loadingTime;
    [Header("LoadingUI")]
    [SerializeField] GameObject loadUi;
    [SerializeField] Slider slider;
    
    [Header("GetComponent")]
    [SerializeField] FadeController fadeController;
    [SerializeField] StageController stageController;

    private AsyncOperation async;

    // �^�C�g����ʂɑJ�ڂ���R���[�`���Ăяo��
    public void TitleScene()
    {
        StartCoroutine(ChangeTitleScene());
    }

    // �^�C�g����ʂɑJ��
    private IEnumerator ChangeTitleScene()
    {
        yield return new WaitUntil(() => fadeController.isFinishFadeOut);
        yield return new WaitForSeconds(loadingTime);
        loadUi.SetActive(true);
        async = SceneManager.LoadSceneAsync("TitleScene");

        StartCoroutine(Loading());
    }

    // �Q�[����ʂɑJ�ڂ���R���[�`���Ăяo��
    public void GameScene()
    {
        fadeController.StartFadeOut();
        StartCoroutine(ChangeGameScene());
    }

    // �Q�[����ʂɑJ��
    private IEnumerator ChangeGameScene()
    {
        yield return new WaitUntil(() => fadeController.isFinishFadeOut);
        yield return new WaitForSeconds(loadingTime);
        loadUi.SetActive(true);
        async = SceneManager.LoadSceneAsync("GameScene");

        StartCoroutine(Loading());
    }

    // ���̃X�e�[�W�Ɉڍs����R���[�`���Ăяo��
    public void NextGameScene()
    {
        stageController.NextStage();
        fadeController.StartFadeOut();

        if (GameController.isGameClear)
        {
            StartCoroutine(ChangeTitleScene());
        }
        else
        {
            StartCoroutine(ChangeNextGameScene());
        }
    }

    // �Q�[����ʂɑJ��
    private IEnumerator ChangeNextGameScene()
    {
        yield return new WaitUntil(() => fadeController.isFinishFadeOut);
        yield return new WaitForSeconds(loadingTime);
        loadUi.SetActive(true);
        async = SceneManager.LoadSceneAsync("GameScene");

        StartCoroutine(Loading());
    }

    // ��ʑJ�ڎ��Ƀ��[�h��}��
    private IEnumerator Loading()
    {
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            slider.value = async.progress;
            if (async.progress >= 0.9f)
            {
                yield return new WaitForSeconds(1.0f);
                async.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
