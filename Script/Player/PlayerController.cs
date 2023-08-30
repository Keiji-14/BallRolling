using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 20; // 動く速さ

    [SerializeField] VirtualStickGUI virtualStick;
    private Rigidbody rb;

    void Start()
    {
        // Rigidbody を取得
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float moveHorizontal;
        float moveVertical;

        // カーソルキーの入力を取得
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        // 入力を取得
        //moveHorizontal = virtualStick.m_InputValue.x;
        //moveVertical = virtualStick.m_InputValue.y;


        // カーソルキーの入力に合わせて移動方向を設定
        var movement = new Vector3(moveHorizontal, 0, moveVertical);

        // Ridigbody に力を与えて玉を動かす
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