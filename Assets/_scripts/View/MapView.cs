using UnityEngine;

public class MapView : BaseView
{
	private MapController mapController;

	protected override void Awake ()
	{
		base.Awake();

		mapController = new MapController();
	}
}