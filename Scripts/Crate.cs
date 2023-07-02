using UnityEngine;

public class Crate : MonoBehaviour
{
	[SerializeField]
	private GameObject goldCoinPrefab;

	public void spawncoin()
	{
		Object.Instantiate(goldCoinPrefab, base.transform.position, base.transform.rotation);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.transform.tag == "bullet")
		{
			Object.Destroy(base.gameObject);
			spawncoin();
		}
	}
}
