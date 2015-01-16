using UnityEngine;

public class WorkshopView : BaseView
{
	private WorkshopController workshopController;

	protected override void Awake ()
	{
		base.Awake();

		workshopController = new WorkshopController();
	}
}