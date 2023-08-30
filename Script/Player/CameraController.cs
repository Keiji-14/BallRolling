using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player; // 玉のオブジェクト

    private Vector3 offset; // 玉からカメラまでの距離

    void Start()
    {
        Debug.Log("Start");
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        // ゲームオーバーになるまでカメラを追従する
        if (!GameController.isGameOver)
        {
            transform.position = player.transform.position + offset;
        }
    }
}
