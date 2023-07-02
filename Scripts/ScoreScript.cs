using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
	public static int scorevalue;

	private Text score;

	private void Start()
	{
		score = GetComponent<Text>();
	}

	private void Update()
	{
		score.text = string.Concat(scorevalue);
	}
}
