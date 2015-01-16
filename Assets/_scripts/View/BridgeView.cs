using UnityEngine;

public class BridgeView : BaseView
{
	private BridgeController bridgeController;

	protected override void Awake ()
	{
		base.Awake();

		bridgeController = new BridgeController();
	}
}