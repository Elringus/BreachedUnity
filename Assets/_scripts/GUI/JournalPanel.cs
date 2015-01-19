using UnityEngine;
using UnityEngine.UI;

public class JournalPanel : MonoBehaviour
{
	private Button previousButton;
	private Button nextButton;
	private static Text journalText;
	private static int currentJournalDay;

	static JournalPanel ()
	{
		Events.DayEnded += (c, e) => { currentJournalDay = ServiceLocator.State.CurrentDay; UpdateJournal(); };
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

	private static void UpdateJournal ()
	{
		journalText.text = BridgeController.GetJournalTextForDay(currentJournalDay);
	}
}