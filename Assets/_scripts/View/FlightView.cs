using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using simul;
using System;

public class FlightView : BaseView
{
	public int SectorID;
	public float MaxAbberation = 6;
	public bool GodMode;

	public List<Keeper> Keepers = new List<Keeper>();

	private trueSKY sky;
	private Vignetting vignetting;
	private CameraGlitch cameraGlitch;
	//private ReliefTerrain RT;

	private FlightController flightController;
	private DroneController drone;
	private List<Loot> lootList = new List<Loot>();

	protected override void Awake ()
	{
		base.Awake();

		sky = GameObject.Find("trueSky").GetComponent<trueSKY>();
		vignetting = Camera.main.GetComponent<Vignetting>();
		cameraGlitch = Camera.main.GetComponent<CameraGlitch>();
		//RT = FindObjectOfType<ReliefTerrain>();
		//RT.BumpGlobalCombined = new Texture2D(64, 64);

		flightController = new FlightController();
		drone = FindObjectOfType<DroneController>();
	}

	protected override void Start ()
	{
		base.Start();

		flightController.GenerateLoot(SectorID, out lootList);
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

		sky.speed += Input.GetAxis("Mouse ScrollWheel") * 10000 * Time.deltaTime;

		//float curTime = sky.time - (float)Math.Truncate(sky.time);
		//if (curTime > .5f) curTime = 1 - curTime;
		//vignetting.blurDistance = MaxAbberation * curTime;

		cameraGlitch.enabled = Keepers.Any(k => k.State == KeeperState.Pursuiting);

		Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, drone.EngineMode == EngineMode.Accel ? 90 : drone.EngineMode == EngineMode.Normal ? 70 : 60, Time.deltaTime);
		vignetting.blur = Mathf.Lerp(vignetting.blur, drone.EngineMode == EngineMode.Accel ? 2 : .5f, Time.deltaTime);
		vignetting.blurSpread = Mathf.Lerp(vignetting.blurSpread, drone.EngineMode == EngineMode.Accel ? 10 : .5f, Time.deltaTime);
		vignetting.intensity = Mathf.Lerp(vignetting.intensity, drone.EngineMode == EngineMode.Accel ? 5 : 3, Time.deltaTime);
	}

	public void ExitFlightMode ()
	{
		SwitchView(ViewType.SimpleView);
	}

	private void OnGUI ()
	{
		GUI.Box(new Rect(Screen.width - 150, 0, 150, 25), "GodMode (F12): " + (GodMode ? "<color=green>ON</color>" : "<color=red>OFF</color>"));
	}
}