using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 20; // ��������

    [SerializeField] VirtualStickGUI virtualStick;
    private Rigidbody rb;

    void Start()
    {
        // Rigidbody ���擾
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float moveHorizontal;
        float moveVertical;

        // �J�[�\���L�[�̓��͂��擾
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        // ���͂��擾
        //moveHorizontal = virtualStick.m_InputValue.x;
        //moveVertical = virtualStick.m_InputValue.y;


        // �J�[�\���L�[�̓��͂ɍ��킹�Ĉړ�������ݒ�
        var movement = new Vector3(moveHorizontal, 0, moveVertical);

        // Ridigbody �ɗ͂�^���ċʂ𓮂���
        rb.AddForce(movement * speed);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            GameController.isClear = true;
            Debug.Log("Goal");
        }

        if (other.gameObject.CompareTag("OutArea"))
        {
            GameController.isGameOver = true;
            Debug.Log("GameOver");
        }
    }
}