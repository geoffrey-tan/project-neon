using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
	// Components
	private Slider healthBar;
	private Image healthBarColor;

	// Scripts
	private PlayerCameraController GetPlayerCamera;

	// Variables
	public float health;
	public bool mindControl;

	void Start()
	{
		health = 100f;

		healthBar = GameObject.Find("Health Bar").GetComponent<Slider>();
		healthBarColor = GameObject.Find("Health Bar").transform.Find("Fill Area/Fill").GetComponent<Image>();

		healthBar.value = health;

		GetPlayerCamera = Camera.main.GetComponent<PlayerCameraController>();
	}

	public void PlayerDamage()
	{
		if (GetPlayerCamera.lookAt.name != "CameraPos")
		{
			GetPlayerCamera.lookAt.parent.GetComponent<MindControl>().Mindcontrol(false);
		}

		if (health > 20)
		{
			health -= 40f;
			healthBar.value = health;
		}
		else if (health <= 20)
		{
			var random = Random.Range(1, 4); // https://docs.unity3d.com/ScriptReference/Random.Range.html

			if (random == 1 && health > 1)
			{
				health = 1;
				healthBar.value = health;
			}
			else
			{
				PlayerDies();
			}
		}
	}

	public void PlayerDies()
	{
		//transform.GetComponent<PlayerController>().enabled = false;
		healthBarColor.color = Color.clear;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Safespot"))
		{
			EnemyAI.safeSpot = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Safespot"))
		{
			EnemyAI.safeSpot = false;
		}
	}
}
