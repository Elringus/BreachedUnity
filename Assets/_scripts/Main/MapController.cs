
public class MapController : BaseController
{
	public bool EnterSector ()
	{
		if (State.CurrentAP < State.EnterSectorAPCost) return false;

		State.CurrentAP -= State.EnterSectorAPCost;
		return true;
	}
}