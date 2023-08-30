using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player; // �ʂ̃I�u�W�F�N�g

    private Vector3 offset; // �ʂ���J�����܂ł̋���

    void Start()
    {
        Debug.Log("Start");
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        // �Q�[���I�[�o�[�ɂȂ�܂ŃJ������Ǐ]����
        if (!GameController.isGameOver)
        {
            transform.position = player.transform.position + offset;
        }
    }
}