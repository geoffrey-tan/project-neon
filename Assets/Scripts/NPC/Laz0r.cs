using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laz0r : MonoBehaviour
{
	// GameObjects
	private GameObject laser;
	private GameObject player;
	private Transform enemyGun1;

	// Variables
	public bool sight;

	void Start()
	{
		laser = transform.Find("Lasers").gameObject;
		enemyGun1 = transform.Find("Armature/Hips/Spine/Chest/Shoulder.R/UpperArm.r/LowerArm.R/Hand.R/EnemyGun (1)/Laser");
		player = GameObject.Find("Player/CameraPos");
	}

	void Update()
	{
		if (sight)
		{
			laser.GetComponent<LineRenderer>().enabled = true;

			laser.GetComponent<LineRenderer>().SetPosition(0, enemyGun1.position);
			laser.GetComponent<LineRenderer>().SetPosition(1, player.transform.position);
		}
		else
		{
			laser.GetComponent<LineRenderer>().enabled = false;
		}
	}
}
