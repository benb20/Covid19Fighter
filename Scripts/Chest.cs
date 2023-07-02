using System.Collections;
using UnityEngine;

public class Chest : MonoBehaviour
{
	[SerializeField]
	private GameObject goldCoinPrefab;

	[SerializeField]
	private GameObject silverCoinPrefab;

	[SerializeField]
	private GameObject gemPrefab;

	[SerializeField]
	private Transform coinPosition;

	private Animator animator;

	private float speed;

	private Rigidbody2D rbody;

	private Vector2 direction;

	private int numberOfCoins;

	private int numberOfSilvers;

	private int numberOfGolds;

	private bool isGem;

	public bool isOpening;

	private bool empty;

	private void Start()
	{
		animator = GetComponent<Animator>();
		numberOfSilvers = 4;
		numberOfGolds = Random.Range(2, 3);
		if (100 - Random.Range(1, 100) < 30)
		{
			isGem = true;
		}
		empty = false;
	}

	private void FixedUpdate()
	{
		rbody = GetComponent<Rigidbody2D>();
	}

	public void spawncoins()
	{
		StartCoroutine("GetCoins");
	}

	private IEnumerator GetCoins()
	{
		bool isRight = GameObject.FindWithTag("Player").GetComponent<RobotPlayer>().isRight;
		isOpening = true;
		Physics2D.IgnoreLayerCollision(8, 9, ignore: true);
		if (!isRight)
		{
			for (int l = 0; l < numberOfGolds; l++)
			{
				Object.Instantiate(goldCoinPrefab, coinPosition.position, Quaternion.Euler(new Vector3(0f, 0f, 0f))).GetComponent<Collectibles>().Initialize(Vector2.right);
				yield return new WaitForSeconds(0.5f);
			}
			for (int l = 0; l < numberOfSilvers; l++)
			{
				Object.Instantiate(silverCoinPrefab, coinPosition.position, Quaternion.Euler(new Vector3(0f, 0f, 0f))).GetComponent<Collectibles>().Initialize(Vector2.right);
				yield return new WaitForSeconds(0.5f);
			}
			if (isGem)
			{
				Object.Instantiate(gemPrefab, coinPosition.position, Quaternion.Euler(new Vector3(0f, 0f, 0f))).GetComponent<Collectibles>().Initialize(Vector2.right);
			}
		}
		else
		{
			for (int l = 0; l < numberOfGolds; l++)
			{
				Object.Instantiate(goldCoinPrefab, coinPosition.position, Quaternion.Euler(new Vector3(0f, 0f, 0f))).GetComponent<Collectibles>().Initialize(Vector2.left);
				yield return new WaitForSeconds(0.5f);
			}
			for (int l = 0; l < numberOfSilvers; l++)
			{
				Object.Instantiate(silverCoinPrefab, coinPosition.position, Quaternion.Euler(new Vector3(0f, 0f, 0f))).GetComponent<Collectibles>().Initialize(Vector2.left);
				yield return new WaitForSeconds(0.5f);
			}
			if (isGem)
			{
				Object.Instantiate(gemPrefab, coinPosition.position, Quaternion.Euler(new Vector3(0f, 0f, 0f))).GetComponent<Collectibles>().Initialize(Vector2.left);
			}
		}
		Physics2D.IgnoreLayerCollision(8, 9, ignore: false);
		isOpening = false;
		empty = true;
		animator.Play("brownemptyopen");
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player" && !empty)
		{
			animator.Play("brownfullopen");
		}
	}
}
