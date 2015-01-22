using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class HorizonView : BaseView
{
	[Tooltip("Will show a new phrase every set seconds.")]
	public float PhraseShowFrequency = 15;
	public float FirstPhraseDelay = 3;

	private Dictionary<Phrase, bool> phrases = new Dictionary<Phrase, bool>();
	private PhrasePanel phrasePanel;

	protected override void Awake ()
	{
		base.Awake();

		phrasePanel = FindObjectOfType<PhrasePanel>();

		foreach (var phrase in HorizonController.GetPhrases())
			phrases.Add(phrase, false);

		InvokeRepeating("NewPhrase", FirstPhraseDelay, PhraseShowFrequency);
	}

	private void NewPhrase ()
	{
		var unshownPhrases = new List<Phrase>();
		foreach (var phrase in phrases)
			if (!phrase.Value) unshownPhrases.Add(phrase.Key);

		if (unshownPhrases.Count == 0)
		{
			phrases.Clear();
			foreach (var phrase in HorizonController.GetPhrases())
				phrases.Add(phrase, false);
			NewPhrase();
			return;
		}

		var selectedPhrase = unshownPhrases[Random.Range(0, unshownPhrases.Count)];
		phrasePanel.ShowPhrase(ServiceLocator.Text.Get(selectedPhrase.ID));
		phrases[selectedPhrase] = true;
	}
}