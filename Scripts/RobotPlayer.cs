using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RobotPlayer : MonoBehaviour
{
	private Rigidbody2D rbody;

	[SerializeField]
	private float movementSpeed;

	[Range(1f, 30f)]
	public float jumpVelocity;

	[SerializeField]
	private GameObject bulletPrefab;

	[SerializeField]
	private GameObject upgradedBulletPrefab;

	[SerializeField]
	public bool isRight = true;

	private bool standingShooting;

	private bool movingShooting;

	private bool jumpShoot;

	private Animator animator;

	[SerializeField]
	private Transform shotPosition;

	public HealthBar healthBar;

	public float currentHealth;

	[SerializeField]
	public float damageStat;

	public AudioSource shotSound;

	private bool isIce;

	public bool isDead;

	public bool onRight;

	private bool fired;

	public float knockbackCount;

	public float knockbackLength = 0.1f;

	private float shootDelay = 0.68f;

	private float nextShootTime;

	private Renderer rend;

	private Color c;

	private void Start()
	{
		Physics2D.IgnoreLayerCollision(8, 11, ignore: false);
		rbody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		rend = GetComponent<Renderer>();
		c = rend.material.color;
		currentHealth = PlayerPrefs.GetFloat("Health");
		isDead = false;
		healthBar.SetMaxHealth(PlayerPrefs.GetFloat("MaxHealth"));
		healthBar.SetHealth(currentHealth);
		if (PlayerPrefs.GetInt("UpgradedShot") == 1)
		{
			damageStat += 50f;
		}
	}

	private void Update()
	{
		if (SceneManager.GetActiveScene().name != "shop" && SceneManager.GetActiveScene().buildIndex != 5 && GameObject.FindGameObjectWithTag("enemycount").GetComponent<AliveCount>().covidCurrentCount == 0)
		{
			GameObject.FindGameObjectWithTag("enemycount").GetComponent<AliveCount>().count.color = Color.green;
			GameObject.Find("countinfo").GetComponent<Text>().color = Color.green;
			Object.Destroy(GameObject.FindGameObjectWithTag("boundary").GetComponent<BoxCollider2D>());
		}
		float axis = Input.GetAxis("Horizontal");
		if (isGrounded())
		{
			animator.SetBool("grounded", value: true);
			fired = false;
		}
		else
		{
			animator.SetBool("grounded", value: false);
		}
		if (isDead)
		{
			return;
		}
		handleInputs(axis);
		if (Input.GetButtonDown("Jump") && isGrounded())
		{
			animator.SetBool("jumping", value: true);
			if (!isIce)
			{
				rbody.velocity = Vector2.up * jumpVelocity;
				return;
			}
			rbody.AddForce(new Vector2(rbody.velocity.x, 36f * jumpVelocity));
			rbody.drag = 5.5f;
		}
	}

	private void FixedUpdate()
	{
		float axis = Input.GetAxis("Horizontal");
		if (!isDead)
		{
			Movement(axis);
			Flip(axis);
		}
	}

	private void Movement(float horizontal)
	{
		if (knockbackCount <= 0f)
		{
			if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("shooting"))
			{
				if (!isIce)
				{
					rbody.velocity = new Vector2(horizontal * movementSpeed, rbody.velocity.y);
				}
				else
				{
					rbody.AddForce(new Vector2(horizontal * movementSpeed, rbody.velocity.y));
				}
			}
		}
		else if (currentHealth != 0f)
		{
			if (onRight)
			{
				rbody.velocity = new Vector2(12f, 3f);
			}
			else
			{
				rbody.velocity = new Vector2(-12f, 3f);
			}
			knockbackCount -= 0.05f;
		}
		animator.SetFloat("movingspeed", Mathf.Abs(horizontal));
	}

	private void Flip(float horizontal)
	{
		if ((horizontal < 0f && !isRight) || (horizontal > 0f && isRight))
		{
			isRight = !isRight;
			Vector3 localScale = base.transform.localScale;
			localScale.x *= -1f;
			base.transform.localScale = localScale;
		}
	}

	private void checkGround()
	{
		animator.SetBool("jumping", value: false);
	}

	private bool canShoot()
	{
		return Time.time >= nextShootTime;
	}

	private void handleInputs(float horizontal)
	{
		if ((Input.GetKeyDown(KeyCode.Mouse0) || Input.GetButtonDown("Fire1")) && horizontal != 0f && isGrounded() && canShoot())
		{
			nextShootTime = Time.time + shootDelay;
			animator.SetTrigger("runandshoot");
		}
		if ((Input.GetKeyDown(KeyCode.Mouse0) || Input.GetButtonDown("Fire1")) && horizontal == 0f && isGrounded())
		{
			animator.SetTrigger("shooting");
		}
		if ((Input.GetKeyDown(KeyCode.Mouse0) || Input.GetButtonDown("Fire1")) && !isGrounded() && !fired)
		{
			fired = true;
			animator.SetTrigger("jumpshoot");
		}
	}

	private bool isGrounded()
	{
		return base.transform.Find("GroundCheck").GetComponent<GroundCheck>().isGrounded;
	}

	public void shoot(int value)
	{
		if ((isGrounded() || value != 1) && (!isGrounded() || value != 0))
		{
			return;
		}
		if (!isRight)
		{
			shotSound.Play();
			if (PlayerPrefs.GetInt("UpgradedShot") == 1)
			{
				Object.Instantiate(upgradedBulletPrefab, shotPosition.position, Quaternion.Euler(new Vector3(0f, 0f, 0f))).GetComponent<Shot>().Initialize(Vector2.right);
			}
			else
			{
				Object.Instantiate(bulletPrefab, shotPosition.position, Quaternion.Euler(new Vector3(0f, 0f, 0f))).GetComponent<Shot>().Initialize(Vector2.right);
			}
		}
		else
		{
			shotSound.Play();
			if (PlayerPrefs.GetInt("UpgradedShot") == 1)
			{
				Object.Instantiate(upgradedBulletPrefab, shotPosition.position, Quaternion.Euler(new Vector3(0f, 0f, -180f))).GetComponent<Shot>().Initialize(Vector2.left);
			}
			else
			{
				Object.Instantiate(bulletPrefab, shotPosition.position, Quaternion.Euler(new Vector3(0f, 0f, -180f))).GetComponent<Shot>().Initialize(Vector2.left);
			}
		}
	}

	public void TakeDamage(float damageAmount)
	{
		currentHealth -= damageAmount;
		healthBar.SetHealth(currentHealth);
		PlayerPrefs.SetFloat("Health", currentHealth);
		if (currentHealth <= 0f)
		{
			isDead = true;
			StartCoroutine(Dying());
		}
		else
		{
			StartCoroutine(IFrames());
		}
	}

	private IEnumerator IFrames()
	{
		Physics2D.IgnoreLayerCollision(8, 11, ignore: true);
		c.a = 0.5f;
		rend.material.color = c;
		yield return new WaitForSeconds(1.5f);
		Physics2D.IgnoreLayerCollision(8, 11, ignore: false);
		c.a = 1f;
		rend.material.color = c;
	}

	public IEnumerator Dying()
	{
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().isDead = true;
		isDead = true;
		animator.Play("robotdeath");
		rbody.velocity = Vector3.zero;
		yield return new WaitForSeconds(2f);
		Object.Destroy(base.gameObject);
		SceneManager.LoadScene(2);
	}

	public IEnumerator Speed()
	{
		movementSpeed += 7f;
		rend.material.color = Color.yellow;
		yield return new WaitForSeconds(10f);
		rend.material.color = Color.white;
		movementSpeed -= 7f;
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "ice")
		{
			isIce = true;
			rbody.drag = 0.2f;
		}
		else if (collision.gameObject.tag == "Ground")
		{
			isIce = false;
			rbody.drag = 0f;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "water")
		{
			Object.Destroy(base.gameObject);
			SceneManager.LoadScene(2);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "boundary")
		{
			if (GameObject.FindGameObjectWithTag("enemycount").GetComponent<AliveCount>().covidCurrentCount != 0)
			{
				StartCoroutine(Flicker());
			}
		}
		else if (collision.gameObject.tag == "battery")
		{
			StartCoroutine(Speed());
			Object.Destroy(collision.gameObject);
		}
	}

	public IEnumerator Flicker()
	{
		GameObject.FindGameObjectWithTag("enemycount").GetComponent<AliveCount>().count.color = Color.red;
		GameObject.Find("countinfo").GetComponent<Text>().color = Color.red;
		rend.material.color = Color.red;
		yield return new WaitForSeconds(1f);
		GameObject.FindGameObjectWithTag("enemycount").GetComponent<AliveCount>().count.color = Color.black;
		GameObject.Find("countinfo").GetComponent<Text>().color = Color.black;
		rend.material.color = Color.white;
	}
}
