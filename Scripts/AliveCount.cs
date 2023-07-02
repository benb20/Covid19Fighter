using UnityEngine;
using UnityEngine.UI;

public class AliveCount : MonoBehaviour
{
	public Text count;

	public int covidTotalCount;

	public int covidCurrentCount;

	private void Start()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("covid");
		covidTotalCount = array.Length;
		covidCurrentCount = covidTotalCount;
		count = GetComponent<Text>();
	}

	private void Update()
	{
		count.text = covidCurrentCount + "/" + covidTotalCount;
	}
}
