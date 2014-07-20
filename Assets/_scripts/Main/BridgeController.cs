
public class BridgeController : BaseController
{
	public bool EndDay ()
	{
		if (State.CurrentDay > State.TotalDays) return false;

		State.CurrentDay++;
		State.CurrentAP = State.MaxAP;

		if (State.CurrentDay > State.TotalDays) 
			State.GameProgress = GameStatus.GameOver;

		foreach (var artifact in State.Artifacts.FindAll(x => x.ArtifactStatus == ArtifactStatus.Analyzing))
		{
			artifact.ArtifactStatus = ArtifactStatus.Analyzed;
			State.Wiring += artifact.Wiring;
			State.Alloy += artifact.Alloy;
			State.Chips += artifact.Chips;
		}

		return true;
	}
}