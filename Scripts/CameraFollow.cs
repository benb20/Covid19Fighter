using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
	[SerializeField]
	private float xMaximum;

	[SerializeField]
	private float xMinimum;

	[SerializeField]
	private float yMaximum;

	[SerializeField]
	private float yMinimum;

	private Camera cam;

	public bool isDead;

	private Transform target;

	private void Start()
	{
		target = GameObject.Find("Robot").transform;
		cam = GameObject.Find("Camera").GetComponent<Camera>();
		isDead = false;
		if (SceneManager.GetActiveScene().name != "shop")
		{
			PlayerPrefs.SetInt("Stage", SceneManager.GetActiveScene().buildIndex);
		}
	}

	private void LateUpdate()
	{
		base.transform.position = new Vector3(Mathf.Clamp(target.position.x, xMinimum, xMaximum), Mathf.Clamp(target.position.y, yMinimum, yMaximum), base.transform.position.z);
		if (!isDead)
		{
			isExitLevel();
		}
	}

	public void isExitLevel()
	{
		if (!GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(cam), GameObject.Find("Robot").GetComponent<Collider2D>().bounds))
		{
			if (SceneManager.GetActiveScene().name != "shop")
			{
				SceneManager.LoadScene(1);
			}
			else
			{
				SceneManager.LoadScene(PlayerPrefs.GetInt("Stage") + 1);
			}
		}
	}
}
