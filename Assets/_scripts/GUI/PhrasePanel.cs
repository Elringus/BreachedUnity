using UnityEngine;
using UnityEngine.UI;

public class PhrasePanel : MonoBehaviour
{
	public float ShowTime = 10;
	public Vector2 MinPhraseSpawnPoint = new Vector2(-180, 280);
	public Vector2 MaxPhraseSpawnPoint = new Vector2(340, -300);

	private RectTransform rectTrans;
	private Image image;
	private Text text;
	private Vector2 currentDest;
	private float timer;

	private void Awake () 
	{
		rectTrans = GetComponent<RectTransform>();
		image = GetComponent<Image>();
		image.color = Color.clear;
		text = GetComponentInChildren<Text>();
		text.color = Color.clear;
	}

	private void Update () 
	{

		rectTrans.anchoredPosition = Vector3.Lerp(rectTrans.anchoredPosition, currentDest, Time.deltaTime / ShowTime);

		image.color = Color.Lerp(image.color, timer > 0 ? Color.white : Color.clear, Time.deltaTime);
		text.color = Color.Lerp(text.color, timer > 0 ? Color.white : Color.clear, Time.deltaTime);

		timer = timer > 0 ? timer - Time.deltaTime : 0;
	}

	public void ShowPhrase (string phraseText)
	{
		rectTrans.anchoredPosition = new Vector2(-25, 240);
		currentDest = new Vector2(-25, 0);

		text.text = phraseText;

		timer = ShowTime;
	}

	private Vector2 RandomWithinShowRect ()
	{
		return new Vector2(Random.Range(MinPhraseSpawnPoint.x, MaxPhraseSpawnPoint.x),
			Random.Range(MinPhraseSpawnPoint.y, MaxPhraseSpawnPoint.y));
	}
}