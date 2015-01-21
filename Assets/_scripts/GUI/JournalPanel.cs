using UnityEngine;
using UnityEngine.UI;

public class JournalPanel : MonoBehaviour
{
	private Button previousButton;
	private Button nextButton;
	private Text journalText;
	private int currentJournalDay;

	private void OnEnable ()
	{
		Events.DayEnded += OnDayEnded;
	}

	private void OnDisable ()
	{
		Events.DayEnded -= OnDayEnded;
	}

	private void Awake () 
	{
		currentJournalDay = ServiceLocator.State.CurrentDay;

		journalText = transform.Find("text_journal").GetComponent<Text>();
		previousButton = transform.Find("button_previous").GetComponent<Button>();
		previousButton.OnClick(() => { 
			currentJournalDay = Mathf.Clamp(currentJournalDay - 1, 1, ServiceLocator.State.CurrentDay); 
			UpdateJournal(); 
		});
		nextButton = transform.Find("button_next").GetComponent<Button>();
		nextButton.OnClick(() =>
		{
			currentJournalDay = Mathf.Clamp(currentJournalDay + 1, 1, ServiceLocator.State.CurrentDay);
			UpdateJournal();
		});

		UpdateJournal();
	}

	private void OnDayEnded ()
	{
		currentJournalDay = ServiceLocator.State.CurrentDay;
		UpdateJournal();
	}

	private void UpdateJournal ()
	{
		journalText.text = BridgeController.GetJournalTextForDay(currentJournalDay);
	}
}