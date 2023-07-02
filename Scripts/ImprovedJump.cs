using UnityEngine;

public class ImprovedJump : MonoBehaviour
{
	[SerializeField]
	public float fallMultiplier = 2.5f;

	[SerializeField]
	public float lowJumpMultiplier = 5f;

	private Rigidbody2D rbody;

	private void Awake()
	{
		rbody = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if (rbody.velocity.y < 0f)
		{
			rbody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.deltaTime;
		}
		else if (rbody.velocity.y > 0f && !Input.GetButton("Jump"))
		{
			rbody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.deltaTime;
		}
	}
}
