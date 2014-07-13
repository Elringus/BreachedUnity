
public class BridgeController : BaseController
{
	public bool EndDay ()
	{
		if (State.CurrentDay > State.TotalDays) return false;

		State.CurrentDay++;
		State.CurrentAP = State.MaxAP;

		if (State.CurrentDay > State.TotalDays) 
			State.GameProgress = GameProgressType.GameOver;

		return true;
	}
}