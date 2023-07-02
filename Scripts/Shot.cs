using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Shot : MonoBehaviour
{
	[SerializeField]
	private float speed;

	private Rigidbody2D rbody;

	private Vector2 direction;

	private GameObject Enemy;

	private Camera cam;

	private void Start()
	{
		rbody = GetComponent<Rigidbody2D>();
		cam = GameObject.Find("Camera").GetComponent<Camera>();
	}

	private void FixedUpdate()
	{
		rbody.velocity = direction * speed;
		isOutSideCamera();
	}

	private void OnBecameInvisible()
	{
		Object.Destroy(base.gameObject);
	}

	public void Initialize(Vector2 direction)
	{
		this.direction = direction;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == 10)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void isOutSideCamera()
	{
		if (!GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(cam), base.gameObject.GetComponent<Collider2D>().bounds))
		{
			Object.Destroy(base.gameObject);
		}
	}
}
