using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
	// Statics
	public static bool safeSpot; // Enemy ignore's player
	public static int enemiesInCombat; // Is player in combat

	// Components
	private Transform self;
	private GameObject player;
	private NavMeshAgent agent;
	private Animator anim;
	private Renderer enemyGun0, enemyGun1;

	// Scripts
	private MindControl GetMindControl;
	private AudioEventController audioEventController;
	private EnemyFieldOfView GetEnemyField;

	// Variables
	public GameObject distractTarget;

	private Coroutine coroutine;
	private Coroutine patrolCoroutine;
	private Coroutine coroutineDistracted;

	private Vector3 playerPosition;
	public Vector3 searchTarget;

	public float patrolSpeed = 1f;
	public float searchSpeed = 2.5f;
	public float combatSpeed = 5f;
	private float distanceTarget = 10f;

	public bool distracted;
	public bool isStunned;
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
		GetEnemyField = GetComponent<EnemyFieldOfView>();

		enemyGun0 = transform.Find("Armature/Hips/Spine/Chest/EnemyGun (0)").GetComponent<Renderer>();
		enemyGun1 = transform.Find("Armature/Hips/Spine/Chest/Shoulder.R/UpperArm.r/LowerArm.R/Hand.R/EnemyGun (1)").GetComponent<Renderer>();

		WeaponDraw(false);
	}

	void Update()
	{
		if (safeSpot && !distracted)
		{
			BacktoPatrol();
		}

		if (!GetMindControl.mindControl && !isStunned) // Not mindcontrolled
		{
			if (combatStart)
			{
				Combat();
			}
			else if (distracted)
			{
				AgentDestination(searchTarget);
			}
			else if (searching)
			{
				AgentDestination(searchTarget);
			}
			else if (!searching)
			{
				Patrol();
			}
		}

		// https://answers.unity.com/questions/36255/lookat-to-only-rotate-on-y-axis-how.html
		playerPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
	}

	void Patrol()
	{
		WeaponDraw(false);
		AgentMovement("patrol"); // https://docs.unity3d.com/ScriptReference/AI.NavMeshAgent-destination.html  

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

		searchTarget = lastSeen;

		WeaponDraw(true);
		AgentMovement("searching");
		AgentDestination(lastSeen);

		if (Vector3.Distance(self.position, lastSeen) > 10f)
		{
			StartCoroutine(SearchWait(0.5f)); // Wait 2f before -> Combat         
		}
		else
		{
			enemiesInCombat++;

			searchWait = true;
			combatStart = true; // Searching -> Combat
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

		if (patrolCoroutine != null)
		{
			StopCoroutine(patrolCoroutine);
		}
	}

	public void Shoot()
	{
		transform.LookAt(playerPosition);

		var damageOverlay = GameObject.Find("ScreenFlash").transform.Find("Damage").gameObject;

		if (Vector3.Distance(self.position, player.transform.position) < 15f)
		{
			shot = true;

			transform.LookAt(playerPosition);

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

	public void Distracted(GameObject target = null, bool distractObject = false)
	{
		distracted = true;

		if (target != null && !distractObject)
		{
			AgentMovement("aim");
			WeaponDraw(true);

			transform.LookAt(target.transform.position); // Mind-Control distract           
		}
		else if (distractObject)
		{
			AgentMovement("searching");
			AgentDestination(target.transform.position);
		}

		if (coroutineDistracted == null)
		{
			coroutineDistracted = StartCoroutine(DistractionTimer(10f, target, distractObject));
		}
	}

	IEnumerator DistractionTimer(float time, GameObject target = null, bool distractObject = false)
	{
		if (target != null && !distractObject)
		{
			target.GetComponent<EnemyFieldOfView>().targetMask = LayerMask.GetMask("Nothing");
			GetEnemyField.targetMask = LayerMask.GetMask("Nothing");

			StartCoroutine(DistractionShoot(target));
		}

		yield return new WaitForSeconds(time);

		if (target != null && !distractObject)
		{
			target.GetComponent<EnemyFieldOfView>().targetMask = LayerMask.GetMask("Player");
		}

		GetEnemyField.targetMask = LayerMask.GetMask("Player");

		distracted = false;

		BacktoPatrol();

		coroutineDistracted = null;
	}

	IEnumerator DistractionShoot(GameObject target)
	{
		audioEventController.PlaySFX("gun1", target);

		anim.SetTrigger("RangedGun");

		yield return new WaitForSeconds(2f);

		if (distracted)
		{
			StartCoroutine(DistractionShoot(target));
		}
	}

	public void AgentDestination(Vector3 target)
	{
		if (!combatStart)
		{
			if (Vector3.Distance(self.position, target) > 2f) // Move to Last Seen
			{
				agent.destination = target;
			}
			else // Stop, wait
			{
				AgentMovement("aim");
			}
		}
		else if (Vector3.Distance(self.position, player.transform.position) > 25f) // Too far -> Patrol
		{
			BacktoPatrol();
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
				transform.LookAt(playerPosition);

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
			case "stunned":
				anim.SetInteger("Animation", 5);
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
		if (safeSpot)
		{
			enemiesInCombat = 0;
		}
		else if (enemiesInCombat > 0)
		{
			enemiesInCombat--;
		}

		if (player.GetComponent<AudioSource>().isPlaying && enemiesInCombat == 0 && PlayerHealth.alive)
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

	public IEnumerator PatrolTimer(float time, bool stunned = false)
	{
		if (stunned)
		{
			isStunned = true;
			AgentMovement("stunned");
		}

		yield return new WaitForSeconds(time);

		isStunned = false;

		BacktoPatrol();
	}

	IEnumerator SearchWait(float time)
	{
		yield return new WaitForSeconds(time);

		searchWait = true;
	}
}
