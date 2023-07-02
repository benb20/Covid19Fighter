using System;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
	[SerializeField]
	private float speed;

	private Rigidbody2D rbody;

	private Vector2 direction;

	public AudioSource coinCollect;

	public HealthBar healthBar;

	private void Start()
	{
		healthBar = GameObject.Find("HealthBarPlayer").GetComponent<HealthBar>();
		rbody = GetComponent<Rigidbody2D>();
		coinCollect = GameObject.Find("coinsound").GetComponent<AudioSource>();
	}

	private void FixedUpdate()
	{
		rbody.velocity = direction * speed;
		if (speed > 0f)
		{
			speed -= 0.03f;
			speed = Math.Abs(speed);
			if ((double)speed < 0.03)
			{
				speed = 0f;
			}
		}
	}

	public void Initialize(Vector2 direction)
	{
		this.direction = direction;
	}

	public void replenishHealth()
	{
		if (GameObject.FindWithTag("Player").GetComponent<RobotPlayer>().currentHealth + 20f > PlayerPrefs.GetFloat("MaxHealth"))
		{
			GameObject.FindWithTag("Player").GetComponent<RobotPlayer>().currentHealth = PlayerPrefs.GetFloat("MaxHealth");
		}
		else
		{
			GameObject.FindWithTag("Player").GetComponent<RobotPlayer>().currentHealth += 20f;
		}
		healthBar.SetHealth(GameObject.FindWithTag("Player").GetComponent<RobotPlayer>().currentHealth);
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		bool flag = GameObject.Find("BrownChest1") != null && GameObject.Find("BrownChest1").GetComponent<Chest>().isOpening;
		if (collision.gameObject.tag == "Player" && !flag)
		{
			if (base.gameObject.tag == "goldcoin")
			{
				coinCollect.Play();
				ScoreScript.scorevalue++;
				UnityEngine.Object.Destroy(base.gameObject);
			}
			if (base.gameObject.tag == "silvercoin")
			{
				coinCollect.Play();
				ScoreScript.scorevalue += 5;
				UnityEngine.Object.Destroy(base.gameObject);
			}
			if (base.gameObject.tag == "gem")
			{
				coinCollect.Play();
				ScoreScript.scorevalue += 15;
				UnityEngine.Object.Destroy(base.gameObject);
			}
			if (base.gameObject.tag == "heart")
			{
				replenishHealth();
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}
}
