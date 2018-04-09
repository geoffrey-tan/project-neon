using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
	private Slider healthBar;
	private Image healthBarColor;

	public float health;

	void Start()
	{
		health = 100f;

		healthBar = GameObject.Find("Health Bar").GetComponent<Slider>();
		healthBarColor = GameObject.Find("Health Bar").transform.Find("Fill Area/Fill").GetComponent<Image>();

		healthBar.value = health;
	}

	public void PlayerDamage()
	{
		if (health > 20)
		{
			health -= 40f;
			healthBar.value = health;
			Debug.Log("HP: " + health + "/100");
		}
		else if (health <= 20)
		{

			var random = Random.Range(1, 4); // https://docs.unity3d.com/ScriptReference/Random.Range.html

			if (random == 1 && health > 1)
			{
				Debug.Log("Survived!");
				health = 1;
				healthBar.value = health;

				Debug.Log("HP: " + health + "/100");
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
		Debug.Log("You Died");
	}
}