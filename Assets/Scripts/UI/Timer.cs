using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
	// Components
	private TextMeshProUGUI timerText;

	// Variables
	public static bool timerStart;
	public float timer;

	void Start()
	{
		timerText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
	}

	void OnEnable()
	{
		timer = 10f;
	}

	void Update()
	{
		if (timerStart)
		{
			timer -= Time.deltaTime;

			if ((int)timer >= 60)
			{
				if (((int)timer - 60) < 10)
				{
					timerText.text = "Mind-Control \n 1:0" + ((int)timer - 60).ToString(); // https://forum.unity.com/threads/linebreaks-in-string-variables.5784/
				}
				else
				{
					timerText.text = "Mind-Control \n 1:" + ((int)timer - 60).ToString();
				}
			}
			else
			{
				if ((int)timer < 10)
				{
					timerText.text = "Mind-Control \n 0:0" + ((int)timer).ToString();
				}
				else
				{
					timerText.text = "Mind-Control \n 0:" + ((int)timer).ToString();
				}
			}
		}
	}
}
