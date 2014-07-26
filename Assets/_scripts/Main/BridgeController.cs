
public class BridgeController : BaseController
{
	public bool EndDay ()
	{
		if (State.CurrentDay > State.TotalDays) return false;

		State.CurrentDay++;
		State.CurrentAP = State.MaxAP;

		if (State.CurrentDay > State.TotalDays) 
			State.GameStatus = GameStatus.GameOver;

		foreach (var artifact in State.Artifacts.FindAll(x => x.Status == ArtifactStatus.Analyzing))
		{
			artifact.Status = ArtifactStatus.Analyzed;
			State.Wiring += artifact.Wiring;
			State.Alloy += artifact.Alloy;
			State.Chips += artifact.Chips;
		}

		return true;
	}
}