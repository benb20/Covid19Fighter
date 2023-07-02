using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	public GameObject pauseMenu;

	public GameObject shadow;

	public bool isPaused;

	private void Start()
	{
		pauseMenu.SetActive(value: false);
		shadow.SetActive(value: false);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Pause"))
		{
			if (isPaused)
			{
				ResumeGame();
			}
			else
			{
				PauseGame();
			}
		}
	}

	public void PauseGame()
	{
		pauseMenu.SetActive(value: true);
		shadow.SetActive(value: true);
		Time.timeScale = 0f;
		isPaused = true;
	}

	public void ResumeGame()
	{
		pauseMenu.SetActive(value: false);
		shadow.SetActive(value: false);
		Time.timeScale = 1f;
		isPaused = false;
	}

	public void MainMenu()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene(0);
	}
}
