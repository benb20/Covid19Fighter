using UnityEngine;

public class GroundCheck : MonoBehaviour
{
	[SerializeField]
	private LayerMask platformLayerMask;

	public bool isGrounded;

	private void OnTriggerStay2D(Collider2D collider)
	{
		if (collider.tag == "Ground" || collider.tag == "ice")
		{
			isGrounded = collider != null && ((1 << collider.gameObject.layer) & (int)platformLayerMask) != 10;
		}
	}

	private void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.tag == "Ground" || collider.tag == "ice")
		{
			isGrounded = false;
		}
	}
}
