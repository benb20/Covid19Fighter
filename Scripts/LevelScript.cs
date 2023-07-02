using UnityEngine;
using UnityEngine.UI;

public class LevelScript : MonoBehaviour
{
	private Text score;

	private void Start()
	{
		score = GetComponent<Text>();
	}

	private void Update()
	{
		score.text = string.Concat(PlayerPrefs.GetInt("Level"));
	}
}
