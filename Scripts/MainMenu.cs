using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public void enterGame()
	{
		PlayerPrefs.SetInt("UpgradedShot", 0);
		PlayerPrefs.SetInt("UpgradedHealth", 0);
		PlayerPrefs.SetFloat("MaxHealth", 100f);
		PlayerPrefs.SetFloat("Health", 100f);
		PlayerPrefs.SetInt("Stage", 0);
		PlayerPrefs.SetInt("CovidCount", 0);
		PlayerPrefs.SetInt("Level", 1);
		PlayerPrefs.SetFloat("DmgMultiplier", 1f);
		PlayerPrefs.SetFloat("HealthMultiplier", 1f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
		ScoreScript.scorevalue = 0;
	}

	public void mainMenu()
	{
		SceneManager.LoadScene(0);
	}

	public void quitGame()
	{
		Application.Quit();
	}
}
