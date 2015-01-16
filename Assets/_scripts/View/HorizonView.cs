using UnityEngine;

public class HorizonView : BaseView
{
	private HorizonController horizonController;

	protected override void Awake ()
	{
		base.Awake();

		horizonController = new HorizonController();
	}
}