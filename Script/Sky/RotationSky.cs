using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSky : MonoBehaviour
{
    public float rotateSpeed;

    public Material sky;

    float rotationRepeatValue;

    // スカイドームを回転させる処理
    void Update()
    {
        rotationRepeatValue = Mathf.Repeat(sky.GetFloat("_Rotation") + rotateSpeed, 360f);
        sky.SetFloat("_Rotation", rotationRepeatValue);
    }
}
