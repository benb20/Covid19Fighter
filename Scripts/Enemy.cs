using System.Collections;
using Pathfinding;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
	public GameObject goldCoinPrefab;

	public GameObject silverCoinPrefab;

	public GameObject heartPrefab;

	public GameObject batteryPrefab;

	private Rigidbody2D rbody;

	private Animator animator;

	[SerializeField]
	protected float currentHealth;

	public HealthBar healthBar;

	public float maxHealth;

	[SerializeField]
	protected float enemyDamage;

	[SerializeField]
	public int chaseDistance;

	public Transform player;

	private float spawnDelay = 10f;

	private float nextSpawnTime;

	private void Start()
	{
		rbody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
		base.transform.parent.gameObject.GetComponent<AIPath>().canMove = false;
		base.transform.parent.gameObject.GetComponent<AIDestinationSetter>().target = player;
		GetComponent<Enemy>().player = GameObject.FindWithTag("Player").GetComponent<Transform>();
		chaseDistance = 40;
		goldCoinPrefab = Resources.Load("Prefabs/Golden Coin") as GameObject;
		silverCoinPrefab = Resources.Load("Prefabs/Silver Coin") as GameObject;
		heartPrefab = Resources.Load("Prefabs/Heart") as GameObject;
		batteryPrefab = Resources.Load("Prefabs/battery") as GameObject;
		maxHealth *= PlayerPrefs.GetFloat("HealthMultiplier");
		enemyDamage *= PlayerPrefs.GetFloat("DmgMultiplier");
		currentHealth = maxHealth;
		healthBar.SetMaxHealth(maxHealth);
		healthBar.SetHealth(currentHealth);
	}

	public void Update()
	{
		if (Vector3.Distance(player.transform.position, base.transform.position) <= (float)chaseDistance && currentHealth > 0f)
		{
			base.transform.parent.gameObject.GetComponent<AIPath>().canMove = true;
		}
		if (base.tag == "boss")
		{
			if (currentHealth <= 500f)
			{
				base.transform.parent.gameObject.GetComponent<AIPath>().maxSpeed = 4f;
				spawnDelay = 5f;
			}
			else if (currentHealth <= 1000f)
			{
				spawnDelay = 7f;
				base.transform.parent.gameObject.GetComponent<AIPath>().maxSpeed = 3f;
			}
			if (shouldSpawn() && base.transform.parent.gameObject.GetComponent<AIPath>().canMove)
			{
				Spawn();
			}
		}
	}

	private bool shouldSpawn()
	{
		return Time.time >= nextSpawnTime;
	}

	public void spawngoldcoin(float i)
	{
		Object.Instantiate(goldCoinPrefab, base.transform.position + new Vector3(0f, i + 0.1f, 0f), base.transform.rotation);
	}

	public void spawnsilvercoin()
	{
		Object.Instantiate(silverCoinPrefab, base.transform.position, base.transform.rotation);
	}

	public void spawnheart(float i)
	{
		Object.Instantiate(heartPrefab, base.transform.position + new Vector3(0f, i + 0.1f, 0f), base.transform.rotation);
	}

	public void spawnbattery(float i)
	{
		Object.Instantiate(batteryPrefab, base.transform.position + new Vector3(0f, i + 0.1f, 0f), base.transform.rotation);
	}

	public void TakeDamage(float damageAmount)
	{
		currentHealth -= damageAmount;
		healthBar.SetHealth(currentHealth);
		if (!(currentHealth <= 0f))
		{
			return;
		}
		rbody.velocity = Vector3.zero;
		PlayerPrefs.SetInt("CovidCount", PlayerPrefs.GetInt("CovidCount") + 1);
		if (base.tag == "boss")
		{
			StartCoroutine(Fin());
			return;
		}
		GameObject.FindGameObjectWithTag("enemycount").GetComponent<AliveCount>().covidCurrentCount--;
		Object.Destroy(base.gameObject);
		float num;
		for (num = 1f; num <= (float)PlayerPrefs.GetInt("Level"); num += 1f)
		{
			spawngoldcoin(num);
		}
		if (GameObject.FindWithTag("Player").GetComponent<RobotPlayer>().currentHealth <= 40f)
		{
			spawnheart(num + 1f);
		}
		int num2 = Random.Range(0, 100);
		if (GameObject.FindWithTag("Player").GetComponent<RobotPlayer>().currentHealth >= 100f && num2 <= 33)
		{
			spawnbattery(num + 1f);
		}
	}

	private IEnumerator Fin()
	{
		base.transform.parent.gameObject.GetComponent<AIPath>().canMove = false;
		animator.Play("reddeath");
		yield return new WaitForSeconds(1.3f);
		SceneManager.LoadScene(6);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.transform.tag == "bullet")
		{
			float damageStat = GameObject.FindWithTag("Player").GetComponent<RobotPlayer>().damageStat;
			TakeDamage(damageStat);
			Object.Destroy(collision.gameObject);
		}
		if (collision.transform.tag == "Player" && !GameObject.FindWithTag("Player").GetComponent<RobotPlayer>().isDead)
		{
			if (base.transform.position.x - GameObject.FindWithTag("Player").GetComponent<Transform>().position.x > 0f)
			{
				GameObject.FindWithTag("Player").GetComponent<RobotPlayer>().onRight = false;
			}
			else
			{
				GameObject.FindWithTag("Player").GetComponent<RobotPlayer>().onRight = true;
			}
			GameObject.FindWithTag("Player").GetComponent<RobotPlayer>().TakeDamage(enemyDamage);
			GameObject.FindWithTag("Player").GetComponent<RobotPlayer>().knockbackCount = GameObject.FindWithTag("Player").GetComponent<RobotPlayer>().knockbackLength;
		}
	}

	private void Spawn()
	{
		nextSpawnTime = Time.time + spawnDelay;
		GameObject.FindGameObjectWithTag("enemycount").GetComponent<AliveCount>().covidTotalCount++;
		GameObject.FindGameObjectWithTag("enemycount").GetComponent<AliveCount>().covidCurrentCount++;
		if (currentHealth <= 1000f)
		{
			Object.Instantiate(Resources.Load("Prefabs/BlueCoronaVirus Variant"), base.transform.position, base.transform.rotation);
		}
		else
		{
			Object.Instantiate(Resources.Load("Prefabs/CoronaVirus"), base.transform.position, base.transform.rotation);
		}
	}
}
