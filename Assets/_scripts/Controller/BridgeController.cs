using System.Linq;

public class BridgeController : BaseController
{
	public static bool EndDay ()
	{
		if (State.CurrentDay > State.TotalDays) return false;

		State.CurrentDay++;
		State.CurrentAP = State.MaxAP;

		if (State.CurrentDay > State.TotalDays) 
			State.GameStatus = GameStatus.GameOver;

		//foreach (var artifact in State.Artifacts.FindAll(x => x.Status == ArtifactStatus.Analyzing))
		//{
		//	artifact.Status = ArtifactStatus.Analyzed;
		//	State.Wiring += artifact.Wiring;
		//	State.Alloy += artifact.Alloy;
		//	State.Chips += artifact.Chips;
		//}

		Events.RaiseDayEnded();

		return true;
	}

	public static string GetJournalTextForDay (int day)
	{
		string text = "";
		foreach (var record in State.JournalRecords.Where(r => r.AssignedDay == day))
			text += Text.Get(record.ID) + "\n";

		return text;
	}
}