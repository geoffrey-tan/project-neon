using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerHealth : MonoBehaviour
{
	public static GameObject player;

	// Components
	private Slider healthBar;
	private Image healthBarColor;
	private Animator anim;

	// Scripts
	private PlayerCameraController GetPlayerCamera;

	// Variables
	public float health;
	public bool mindControl;
	public bool invincible;

	void Start()
	{
		player = gameObject;
		anim = GetComponent<Animator>();

		health = 100f;

		healthBar = GameObject.Find("Health Bar").GetComponent<Slider>();
		healthBarColor = GameObject.Find("Health Bar").transform.Find("Fill Area/Fill").GetComponent<Image>();

		healthBar.value = health;

		GetPlayerCamera = Camera.main.GetComponent<PlayerCameraController>();

		if (DataSave.lastCheckpoint != new Vector3(0, 0, 0))
		{
			player.transform.position = DataSave.lastCheckpoint;
		}
	}

	public static void PlayerControl(bool toggle)
	{
		player.GetComponent<PlayerAbility>().enabled = toggle;
		player.GetComponent<ThirdPersonCharacter>().enabled = toggle;
		player.GetComponent<ThirdPersonUserControl>().enabled = toggle;
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
			else if (!invincible)
			{
				invincible = true;
				PlayerDies();
			}
		}
	}

	public void PlayerDies()
	{
		PlayerControl(false);
		anim.SetTrigger("Death");
		healthBarColor.color = Color.clear;

		EnemyAI.safeSpot = true;

		StartCoroutine(RestartLevel());
	}

	IEnumerator RestartLevel()
	{
		yield return new WaitForSeconds(5f);

		string sceneName = SceneManager.GetActiveScene().name;
		SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Safespot"))
		{
			DataSave.lastCheckpoint = other.transform.position;
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
