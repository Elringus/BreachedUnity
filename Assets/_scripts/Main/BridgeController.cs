
public class BridgeController : BaseController
{
	public void EndDay ()
	{
		State.CurrentDay++;
		State.CurrentAP = State.MaxAP;

		if (State.CurrentDay > State.TotalDays) 
			State.GameProgress = GameProgressType.GameOver;
	}
}