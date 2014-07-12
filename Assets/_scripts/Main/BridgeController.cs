using UnityEngine;
using System.Collections;

public class BridgeController : BaseController
{
	public void EndDay ()
	{
		State.CurrentDay++;
		State.CurrentAP = State.MaxAP;
	}
}