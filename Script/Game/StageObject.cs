using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObject : MonoBehaviour
{
    private Vector3 basePos;

    [SerializeField] float moveRange;
    [SerializeField] float speed;
    [SerializeField] bool isAction;
    [SerializeField] StageObjectType stageObjectType;

    enum StageObjectType
    {
        FrontMove,
        RightMove,
    }

    void Start()
    {
        // ŠJŽnŽž‚ÌˆÊ’u‚ð•Û‘¶
        basePos = transform.position;
    }

    void Update()
    {
        switch (stageObjectType)
        {
            case StageObjectType.FrontMove:
                FrontMove();
                break;
            case StageObjectType.RightMove:
                RightMove();
                break;
        }    
    }

    private void FrontMove()
    {
        if (transform.position.z < basePos.z - moveRange)
        {
            isAction = true;
        }
        else if (transform.position.z > basePos.z + moveRange)
        {
            isAction = false;
        }

        if (isAction)
        {
            transform.position += Vector3.forward * speed;
        }
        else
        {
            transform.position -= Vector3.forward * speed;
        }
    }

    private void RightMove()
    {
        if (transform.position.x < basePos.x - moveRange)
        {
            isAction = true;
        }
        else if (transform.position.x > basePos.x + moveRange)
        {
            isAction = false;
        }

        if (isAction)
        {
            transform.position += Vector3.right * speed;
        }
        else
        {
            transform.position -= Vector3.right * speed;
        }
    }
}
