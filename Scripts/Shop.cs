using UnityEngine;

public class Shop : MonoBehaviour
{
	public HealthBar healthBar;

	private GameObject dmgObject;

	private GameObject hlthObject;

	private GameObject rplObject;

	private void Start()
	{
		if (PlayerPrefs.GetInt("UpgradedShot") == 1)
		{
			Object.Destroy(GameObject.FindGameObjectWithTag("shotUpgrade"));
		}
		if (PlayerPrefs.GetFloat("MaxHealth") == 150f)
		{
			Object.Destroy(GameObject.FindGameObjectWithTag("healthUpgrade"));
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			if (base.gameObject.tag == "shotUpgrade" && (Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Shop")) && ScoreScript.scorevalue >= 50)
			{
				PlayerPrefs.SetInt("UpgradedShot", 1);
				PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
				PlayerPrefs.SetFloat("DmgMultiplier", PlayerPrefs.GetFloat("DmgMultiplier") + 0.5f);
				ScoreScript.scorevalue -= 50;
				Object.Destroy(base.gameObject);
			}
			if (base.gameObject.tag == "healthUpgrade" && (Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Shop")) && ScoreScript.scorevalue >= 30)
			{
				PlayerPrefs.SetFloat("MaxHealth", 150f);
				PlayerPrefs.SetFloat("Health", 150f);
				PlayerPrefs.SetFloat("HealthMultiplier", PlayerPrefs.GetFloat("HealthMultiplier") + 0.4f);
				healthBar.SetMaxHealth(150f);
				healthBar.SetHealth(150f);
				PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
				ScoreScript.scorevalue -= 30;
				Object.Destroy(base.gameObject);
			}
			if (base.gameObject.tag == "healthReplenish" && (Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Shop")) && ScoreScript.scorevalue >= 10)
			{
				PlayerPrefs.SetFloat("Health", PlayerPrefs.GetFloat("MaxHealth"));
				healthBar.SetHealth(PlayerPrefs.GetFloat("Health"));
				ScoreScript.scorevalue -= 10;
				Object.Destroy(base.gameObject);
			}
		}
	}
}
