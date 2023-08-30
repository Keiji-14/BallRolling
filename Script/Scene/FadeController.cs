using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// フェードインとフェードアウトの処理
/// </summary>
public class FadeController : MonoBehaviour
{
	private float red;
	private float green;
	private float blue;
	private float alfa;

	public float fadeSpeed;
	public bool isFadeIn = false;
	public bool isFadeOut = false;
	public bool isFinishFadeIn = false;
	public bool isFinishFadeOut = false;

	Image fadeImage;

	void Start()
	{
		fadeImage = GetComponent<Image>();
		red = fadeImage.color.r;
		green = fadeImage.color.g;
		blue = fadeImage.color.b;
		alfa = fadeImage.color.a;

		isFadeIn = true;
	}

	void Update()
	{
		if (isFadeIn)
		{
			StartFadeIn();
		}

		if (isFadeOut)
		{
			StartFadeOut();
		}
	}

	// フェードインを行う処理
	public void StartFadeIn()
	{
		isFadeIn = true;
		isFinishFadeIn = false;
		alfa -= fadeSpeed;
		SetAlpha();
		if (alfa <= 0)
		{
			isFadeIn = false;
			isFinishFadeIn = true;
			fadeImage.enabled = false;
		}
	}

	// フェードアウトを行う処理
	public void StartFadeOut()
	{
		isFadeOut = true;
		isFinishFadeOut = false;
		fadeImage.enabled = true;
		alfa += fadeSpeed;
		SetAlpha();
		if (alfa >= 1)
		{
			isFadeOut = false;
			isFinishFadeOut = true;
		}
	}

	void SetAlpha()
	{
		fadeImage.color = new Color(red, green, blue, alfa);
	}
}
