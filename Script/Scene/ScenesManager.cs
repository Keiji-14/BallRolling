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

    // タイトル画面に遷移するコルーチン呼び出し
    public void TitleScene()
    {
        StartCoroutine(ChangeTitleScene());
    }

    // タイトル画面に遷移
    private IEnumerator ChangeTitleScene()
    {
        yield return new WaitUntil(() => fadeController.isFinishFadeOut);
        yield return new WaitForSeconds(loadingTime);
        loadUi.SetActive(true);
        async = SceneManager.LoadSceneAsync("TitleScene");

        StartCoroutine(Loading());
    }

    // ゲーム画面に遷移するコルーチン呼び出し
    public void GameScene()
    {
        fadeController.StartFadeOut();
        StartCoroutine(ChangeGameScene());
    }

    // ゲーム画面に遷移
    private IEnumerator ChangeGameScene()
    {
        yield return new WaitUntil(() => fadeController.isFinishFadeOut);
        yield return new WaitForSeconds(loadingTime);
        loadUi.SetActive(true);
        async = SceneManager.LoadSceneAsync("GameScene");

        StartCoroutine(Loading());
    }

    // 次のステージに移行するコルーチン呼び出し
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

    // ゲーム画面に遷移
    private IEnumerator ChangeNextGameScene()
    {
        yield return new WaitUntil(() => fadeController.isFinishFadeOut);
        yield return new WaitForSeconds(loadingTime);
        loadUi.SetActive(true);
        async = SceneManager.LoadSceneAsync("GameScene");

        StartCoroutine(Loading());
    }

    // 画面遷移時にロードを挿む
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
