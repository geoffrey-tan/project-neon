using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
	// gameObjects
	private Transform self;
	private GameObject player;
	private Transform patrolPos0, patrolPos1;
	private MindControl GetMindControl;

	// Components
	private Animator anim;
	private NavMeshAgent agent;
	private AudioEventController audioEventController;
	private Renderer enemyGun0, enemyGun1;

	// Variables
	private Coroutine coroutine;
	private Coroutine patrolCoroutine;
	private Coroutine coroutineDistracted;

	public bool distracted;

	private int arrived = 1;
	private int moveCloser;
	private float mindControlDuration = 30f;

	public float patrolSpeed = 0.25f;
	public float searchSpeed = 1f;
	public float combatSpeed = 1.25f;

	public bool searching;
	public bool searchWait;
	public bool combatStart;
	public bool shot;
	public bool canShoot;
	public bool mindControl;

	void Start()
	{
		self = transform;
		player = GameObject.Find("Player");

		patrolPos0 = transform.parent.Find("PatrolPos (0)").transform; // https://answers.unity.com/questions/763732/getcomponentinparent-how-does-it-work.html
		patrolPos1 = transform.parent.Find("PatrolPos (1)").transform;

		//enemyGun0 = transform.Find("EnemyGun (0)").GetComponent<Renderer>();
		//enemyGun1 = transform.Find("EnemyGun (1)").GetComponent<Renderer>();
		enemyGun0 = transform.Find("Armature/Hips/Spine/Chest/EnemyGun (0)").GetComponent<Renderer>();
		enemyGun1 = transform.Find("Armature/Hips/Spine/Chest/Shoulder.R/UpperArm.r/LowerArm.R/Hand.R/EnemyGun (1)").GetComponent<Renderer>();

		enemyGun0.enabled = true;
		enemyGun1.enabled = false;

		GetMindControl = transform.GetComponent<MindControl>();

		agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
		audioEventController = GetComponent<AudioEventController>();

		agent.destination = patrolPos0.position; // https://docs.unity3d.com/ScriptReference/AI.NavMeshAgent-destination.html
		anim.SetInteger("Animation", 1);
	}

	void Update()
	{
		if (!GetMindControl.mindControl)
		{
			if (combatStart)
			{
				Combat(); // Chase player    
			}
			else if (!searching & !distracted)
			{
				Debug.Log("I am patrolling");
				Patrol(); // Patrol points
			}
			else
			{
				Debug.Log("I am distracted");
				Distracted();
			}
		}

	}

	void Distracted()
	{


		if (coroutineDistracted == null)
		{
			coroutineDistracted = StartCoroutine(DistractionTimer(10f));
		}


	}

	IEnumerator DistractionTimer(float time)
	{
		AgentMovement(false);

		yield return new WaitForSeconds(time);

		Debug.Log("Timer done!");

		distracted = false;

		arrived = 1;
		agent.destination = patrolPos0.position;

		coroutineDistracted = null;

	}

	void Patrol()
	{
		WeaponDraw(false);

		anim.SetInteger("Animation", 1);

		agent.isStopped = false;
		agent.speed = patrolSpeed;

		if (Vector3.Distance(self.position, patrolPos0.position) < 1f && arrived == 1)
		{
			arrived = 0;
			agent.destination = patrolPos1.position;
		}
		else if (Vector3.Distance(self.position, patrolPos1.position) < 1f && arrived == 0)
		{
			arrived = 1;
			agent.destination = patrolPos0.position;
		}

	}

	void AgentMovement(bool yes)
	{
		if (yes)
		{
			agent.isStopped = false;
		}
		else
		{
			anim.SetInteger("Animation", 3);
			agent.isStopped = true;
		}
	}

	public void Searching(Vector3 position)
	{
		WeaponDraw(true);

		anim.SetInteger("Animation", 4);

		Debug.Log("I see you!");

		audioEventController.PlaySFX("alert0");

		searching = true; // Patrol -> Searching

		agent.speed = searchSpeed;

		if (Vector3.Distance(self.position, position) > 10f)
		{
			Debug.Log("Not sure");
			StartCoroutine(SearchWait(0.5f)); // Wait 2f before -> Combat
		}
		else
		{
			Debug.Log("Pretty sure");
			searchWait = true; // Searching -> Combat
		}

		patrolCoroutine = StartCoroutine(PatrolTimer(20f));

		// Follow
		if (Vector3.Distance(self.position, player.transform.position) > 25f) // Too far -> Patrol
		{
			BacktoPatrol();
		}
		else if (Vector3.Distance(self.position, position) > 2f) // Move to Last Seen
		{
			agent.isStopped = false;
			agent.destination = position;
		}
		else // Stop, wait
		{
			anim.SetInteger("Animation", 3);

			agent.isStopped = true;
		}
	}

	public void Combat()
	{
		WeaponDraw(true);

		anim.SetInteger("Animation", 2);

		StopCoroutine(patrolCoroutine);

		Debug.Log("Combat Started!");

		if (!player.GetComponent<AudioSource>().isPlaying)
		{
			player.GetComponent<AudioSource>().Play();
		}

		agent.speed = combatSpeed;

		// Follow
		if (Vector3.Distance(self.position, player.transform.position) > 25f) // Too far -> Patrol
		{
			BacktoPatrol();
		}

		if (moveCloser == 1)
		{
			Debug.Log("Moving closer!");

			if (Vector3.Distance(self.position, player.transform.position) > 5f) // Move to Player
			{

				agent.isStopped = false;
				agent.destination = player.transform.position;
			}
			else // Stop, wait
			{
				anim.SetInteger("Animation", 3);

				transform.LookAt(player.transform);

				if (coroutine == null) // https://answers.unity.com/questions/1029332/restart-a-coroutine.html
				{
					coroutine = StartCoroutine(MoveTimer(10f));

				}


				agent.isStopped = true;
			}
		}
		else if (moveCloser == 2)
		{
			Debug.Log("Moving even closer!");

			if (Vector3.Distance(self.position, player.transform.position) > 2f) // Move to Player
			{


				agent.isStopped = false;
				agent.destination = player.transform.position;
			}
			else // Stop, wait
			{
				transform.LookAt(player.transform);

				anim.SetInteger("Animation", 3);

				agent.isStopped = true;
			}
		}
		else
		{
			if (Vector3.Distance(self.position, player.transform.position) > 10f) // Move to Player
			{


				agent.isStopped = false;
				agent.destination = player.transform.position;
			}
			else // Stop, wait
			{
				transform.LookAt(player.transform);

				if (coroutine == null) // https://answers.unity.com/questions/1029332/restart-a-coroutine.html
				{
					coroutine = StartCoroutine(MoveTimer(5f));
				}
				anim.SetInteger("Animation", 3);

				agent.isStopped = true;
			}
		}
	}

	public IEnumerator MoveTimer(float time)
	{
		//timer
		Debug.Log("Timer Started! " + time);

		yield return new WaitForSeconds(2f);

		if (!canShoot)
		{
			Debug.Log("I can shoot now!");
			canShoot = true;
		}


		yield return new WaitForSeconds(time);

		if (moveCloser == 0)
		{
			moveCloser = 1;
		}
		else
		{
			moveCloser = 2;
		}

		coroutine = null;
	}

	public void Shoot()
	{
		if (Vector3.Distance(self.position, player.transform.position) < 15f)
		{
			transform.LookAt(player.transform);

			anim.SetTrigger("RangedGun");
			player.GetComponent<Animator>().SetTrigger("Hit");

			StartCoroutine(ShootTimer(2f));

			audioEventController.PlaySFX("gun0");

			GameObject.Find("ScreenFlash").transform.Find("Damage").gameObject.SetActive(false);
			GameObject.Find("ScreenFlash").transform.Find("Damage").gameObject.SetActive(true);

			player.GetComponent<PlayerHealth>().PlayerDamage();
		}
	}

	IEnumerator ShootTimer(float time)
	{
		yield return new WaitForSeconds(time);

		Debug.Log("I can shoot again");

		shot = false;
	}

	void MindControl()
	{
		//WeaponDraw(true);

		//speed = 1f;
		//agent.speed = speed;

		//// Follow
		//if (Vector3.Distance(self.position, player.transform.position) > 2f) // Follow Player
		//{
		//	anim.SetInteger("Animation", 1);

		//	agent.isStopped = false;
		//	agent.destination = player.transform.position;

		//}
		//else
		//{
		//	anim.SetInteger("Animation", 0);

		//	agent.isStopped = true;
		//}
		// Attack
		//StartCoroutine(MindControlTimer(mindControlDuration));
	}

	public void BacktoPatrol()
	{
		if (player.GetComponent<AudioSource>().isPlaying)
		{
			player.GetComponent<AudioSource>().Stop();
		}

		anim.SetInteger("Animation", 1);

		agent.isStopped = false;

		searching = false;
		searchWait = false;
		combatStart = false;
		shot = false;
		canShoot = false;

		arrived = 0;
		agent.destination = patrolPos1.position;

		Debug.Log("I lost you!");
	}

	void WeaponDraw(bool draw)
	{
		if (!draw)
		{
			enemyGun0.enabled = true;
			enemyGun1.enabled = false;
		}
		else
		{
			enemyGun0.enabled = false;
			enemyGun1.enabled = true;
		}
	}

	IEnumerator PatrolTimer(float time)
	{
		Debug.Log("Patrolling in " + time + " seconds");

		yield return new WaitForSeconds(time);

		BacktoPatrol();
	}

	IEnumerator MindControlTimer(float time)
	{
		yield return new WaitForSeconds(time);

		mindControl = false;
	}

	IEnumerator SearchWait(float time)
	{
		yield return new WaitForSeconds(time);

		searchWait = true;
	}
}
