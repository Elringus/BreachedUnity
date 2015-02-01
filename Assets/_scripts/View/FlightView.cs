using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class FlightView : BaseView
{
	public int SectorID;
	public bool GodMode;

	public List<Keeper> Keepers = new List<Keeper>();

	private Vignetting vignetting;
	private CameraGlitch cameraGlitch;

	private DroneController drone;
	private List<Loot> lootList = new List<Loot>();

	protected override void Awake ()
	{
		base.Awake();

		vignetting = Camera.main.GetComponent<Vignetting>();
		cameraGlitch = Camera.main.GetComponent<CameraGlitch>();

		drone = FindObjectOfType<DroneController>();
	}

	protected override void Start ()
	{
		base.Start();

		FlightController.GenerateLoot(SectorID, out lootList);
		var lootSpots = new List<LootSpot>(FindObjectsOfType<LootSpot>());
		lootSpots.Shuffle();
		for (int i = 0; i < lootList.Count; i++)
		{
			lootSpots[i].Loot = lootList[i];
			lootSpots[i].Active = true;
		}
	}

	protected override void Update ()
	{
		base.Update();

		if (Input.GetKeyDown(KeyCode.F12)) GodMode = !GodMode;

		cameraGlitch.enabled = Keepers.Any(k => k.State == KeeperState.Pursuiting);

		Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, drone.EngineMode == EngineMode.Accel ? 90 : drone.EngineMode == EngineMode.Normal ? 70 : 60, Time.deltaTime);
		vignetting.blur = Mathf.Lerp(vignetting.blur, drone.EngineMode == EngineMode.Accel ? 2 : .5f, Time.deltaTime);
		vignetting.blurSpread = Mathf.Lerp(vignetting.blurSpread, drone.EngineMode == EngineMode.Accel ? 10 : .5f, Time.deltaTime);
		vignetting.intensity = Mathf.Lerp(vignetting.intensity, drone.EngineMode == EngineMode.Accel ? 5 : 3, Time.deltaTime);
	}

	public void ExitFlightMode ()
	{
		SwitchView(ViewType.Map);
	}

	private void OnGUI ()
	{
		if (GlobalConfig.RELEASE_TYPE == ReleaseType.alpha) 
			GUI.Box(new Rect(Screen.width - 150, 0, 150, 25), "GodMode (F12): " + (GodMode ? "<color=green>ON</color>" : "<color=red>OFF</color>"));
	}
}