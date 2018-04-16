using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
	// Components
	private Transform self;
	private GameObject player;
	private NavMeshAgent agent;
	private Animator anim;
	private Renderer enemyGun0, enemyGun1;

	// Scripts
	private MindControl GetMindControl;
	private AudioEventController audioEventController;

	// Variables
	private Coroutine coroutine;
	private Coroutine patrolCoroutine;
	private Coroutine coroutineDistracted;

	//public bool mindControl;
	public bool distracted;

	public float patrolSpeed = 0.25f;
	public float searchSpeed = 1f;
	public float combatSpeed = 1.25f;
	private float distanceTarget = 10f;

	public bool searching;
	public bool searchWait;
	public bool combatStart;
	public bool canShoot;
	public bool shot;

	private int currentPatrol = 0;

	void Start()
	{
		self = transform;
		player = GameObject.Find("Player");
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();

		GetMindControl = transform.GetComponent<MindControl>();
		audioEventController = GetComponent<AudioEventController>();

		enemyGun0 = transform.Find("Armature/Hips/Spine/Chest/EnemyGun (0)").GetComponent<Renderer>();
		enemyGun1 = transform.Find("Armature/Hips/Spine/Chest/Shoulder.R/UpperArm.r/LowerArm.R/Hand.R/EnemyGun (1)").GetComponent<Renderer>();

		WeaponDraw(false);
		AgentMovement("patrol"); // https://docs.unity3d.com/ScriptReference/AI.NavMeshAgent-destination.html      
	}

	void Update()
	{
		if (!GetMindControl.mindControl) // Not mindcontrolled
		{
			if (combatStart)
			{
				Combat();
			}
			else if (distracted)
			{
				Distracted(null); // Distracted by object
			}
			else if (!searching)
			{
				Patrol();
			}
		}
	}

	void Patrol()
	{
		WeaponDraw(false);
		AgentMovement("patrol");

		Transform[] patrolPoints = transform.parent.Find("PatrolPoints").GetComponentsInChildren<Transform>(); // https://answers.unity.com/questions/763732/getcomponentinparent-how-does-it-work.html

		int patrolPointsMax = transform.parent.Find("PatrolPoints").transform.childCount;

		if (patrolPointsMax > 1)
		{
			if (currentPatrol == patrolPointsMax)
			{
				currentPatrol = 0;
			}

			agent.destination = patrolPoints[currentPatrol + 1].position; // +1 First Transform is parent

			if (Vector3.Distance(self.position, patrolPoints[currentPatrol + 1].position) < 1f)
			{
				currentPatrol++;
			}
		}
		else
		{
			if (Vector3.Distance(self.position, patrolPoints[currentPatrol + 1].position) > 1f)
			{
				agent.destination = patrolPoints[currentPatrol + 1].position;
			}
			else if (Vector3.Distance(self.position, patrolPoints[currentPatrol + 1].position) <= 1f)
			{
				WeaponDraw(true);
				AgentMovement("aim");
			}
		}
	}

	public void Searching(Vector3 lastSeen) // Called from EnemyFieldOfView script
	{
		searching = true; // Patrol -> Searching      
		audioEventController.PlaySFX("alert0");

		WeaponDraw(true);
		AgentMovement("searching");
		AgentDestination(lastSeen);

		if (Vector3.Distance(self.position, lastSeen) > 10f)
		{
			StartCoroutine(SearchWait(0.5f)); // Wait 2f before -> Combat         
		}
		else
		{
			searchWait = true; // Searching -> Combat
		}

		patrolCoroutine = StartCoroutine(PatrolTimer(20f));
	}

	public void Combat()
	{
		WeaponDraw(true);
		AgentMovement("combat");
		AgentDestination(player.transform.position);

		if (!player.GetComponent<AudioSource>().isPlaying)
		{
			player.GetComponent<AudioSource>().Play();
		}

		StopCoroutine(patrolCoroutine);
	}

	public void Shoot()
	{
		transform.LookAt(player.transform);

		var damageOverlay = GameObject.Find("ScreenFlash").transform.Find("Damage").gameObject;

		if (Vector3.Distance(self.position, player.transform.position) < 15f)
		{
			shot = true;

			transform.LookAt(player.transform);

			audioEventController.PlaySFX("gun0");
			player.GetComponent<Animator>().SetTrigger("Hit");

			player.GetComponent<PlayerHealth>().PlayerDamage();

			damageOverlay.SetActive(false);
			damageOverlay.SetActive(true);

			StartCoroutine(ShootTimer(2f));
		}
	}

	public IEnumerator ShootTimer(float time)
	{
		anim.SetTrigger("RangedGun");

		yield return new WaitForSeconds(time);

		shot = false;
	}

	public void Distracted(GameObject target)
	{
		AgentMovement("aim");
		WeaponDraw(true);

		if (target != null)
		{
			transform.LookAt(target.transform.position); // Mind-Control distract         
		}

		if (coroutineDistracted == null)
		{
			coroutineDistracted = StartCoroutine(DistractionTimer(10f));
		}
	}

	IEnumerator DistractionTimer(float time)
	{
		yield return new WaitForSeconds(time);

		distracted = false;

		BacktoPatrol();

		coroutineDistracted = null;
	}

	void AgentDestination(Vector3 target)
	{
		if (Vector3.Distance(self.position, player.transform.position) > 25f) // Too far -> Patrol
		{
			BacktoPatrol();
		}
		else if (!combatStart)
		{
			if (Vector3.Distance(self.position, target) > 2f) // Move to Last Seen
			{
				agent.destination = target;
			}
			else // Stop, wait
			{
				AgentMovement("aim");
				transform.LookAt(player.transform);
			}
		}
		else
		{
			if (Vector3.Distance(self.position, target) > distanceTarget) // Move to Player
			{
				agent.destination = target;
			}
			else // Stop, wait
			{
				AgentMovement("aim");
				transform.LookAt(player.transform);

				if (coroutine == null) // Restart coroutine https://answers.unity.com/questions/1029332/restart-a-coroutine.html
				{
					if ((int)distanceTarget == 10) // Float -> Int https://social.msdn.microsoft.com/Forums/vstudio/en-US/c633db6d-fc3c-4c83-be8d-90f9640bbbf4/how-can-i-convert-float-to-int?forum=csharpgeneral
					{
						coroutine = StartCoroutine(MoveTimer(5f));
					}
					else if ((int)distanceTarget == 5)
					{
						coroutine = StartCoroutine(MoveTimer(10f));
					}
				}

			}
		}
	}

	public IEnumerator MoveTimer(float time)
	{
		yield return new WaitForSeconds(2f);

		if (!canShoot)
		{
			canShoot = true;
		}

		yield return new WaitForSeconds(time);

		if ((int)distanceTarget == 10)
		{
			distanceTarget = 5f;
		}
		else if ((int)distanceTarget == 5)
		{
			distanceTarget = 2f;
		}

		coroutine = null;
	}

	public void AgentMovement(string movement)
	{
		switch (movement)
		{
			case "patrol":
				anim.SetInteger("Animation", 1);
				agent.isStopped = false;
				agent.speed = patrolSpeed;
				break;
			case "searching":
				anim.SetInteger("Animation", 4);
				agent.isStopped = false;
				agent.speed = searchSpeed;
				break;
			case "combat":
				anim.SetInteger("Animation", 2);
				agent.isStopped = false;
				agent.speed = combatSpeed;
				break;
			case "aim":
				anim.SetInteger("Animation", 3);
				agent.isStopped = true;
				break;
			case "stop":
				anim.SetInteger("Animation", 0);
				agent.isStopped = true;
				break;
			default:
				anim.SetInteger("Animation", 0);
				agent.isStopped = true;
				break;
		}
	}

	public void BacktoPatrol()
	{
		if (player.GetComponent<AudioSource>().isPlaying)
		{
			player.GetComponent<AudioSource>().Stop();
		}

		AgentMovement("patrol");

		searching = false;
		searchWait = false;
		combatStart = false;
		canShoot = false;
		shot = false;

		distanceTarget = 10f;
	}

	public void WeaponDraw(bool draw)
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
		yield return new WaitForSeconds(time);

		BacktoPatrol();

		patrolCoroutine = null;
	}

	IEnumerator SearchWait(float time)
	{
		yield return new WaitForSeconds(time);

		searchWait = true;
	}
}
