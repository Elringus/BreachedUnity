
public class MapController : BaseController
{
	public static bool EnterSector ()
	{
		if (State.CurrentAP < State.EnterSectorAPCost) return false;

		State.CurrentAP -= State.EnterSectorAPCost;
		return true;
	}
}