using UnityEngine;
using UnityEngine.UI;

public class CovidCounter : MonoBehaviour
{
	private Text score;

	private void Start()
	{
		score = GetComponent<Text>();
	}

	private void Update()
	{
		score.text = "You killed " + PlayerPrefs.GetInt("CovidCount") + " viruses";
	}
}
