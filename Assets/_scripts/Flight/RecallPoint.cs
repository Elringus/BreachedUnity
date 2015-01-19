using UnityEngine;
using System.Collections;

public class RecallPoint : MonoBehaviour
{
	public float RecallingProgress;
	public float RecallingTime = 10;

	private bool _active;
	public bool Active
	{
		get { return _active; }
		set { _active = value; myCollider.enabled = value; model.SetActive(value); }
	}

	private GameObject model;
	private Collider myCollider;
	private FlightController flightContoller;
	private FlightView flightView;
	private DroneController drone;
	private bool droneIn;

	private void Awake () 
	{
		model = transform.Find("model").gameObject;
		myCollider = GetComponent<SphereCollider>();
		flightContoller = new FlightController();
		drone = FindObjectOfType<DroneController>();
		flightView = FindObjectOfType<FlightView>();

		Active = true;
	}

	private void Update () 
	{
		if (Active)
		{
			model.transform.localScale = Vector3.Lerp(model.transform.localScale, new Vector3(model.transform.localScale.x, 20 + Mathf.Sin(Time.time) * 10, model.transform.localScale.z), Time.deltaTime);
			model.transform.Rotate(new Vector3(0, 1), 50 * (1 + RecallingProgress) * Time.deltaTime);
			if (droneIn && drone.EngineMode == EngineMode.Stop) RecallingProgress += Time.deltaTime;
			if (RecallingProgress >= RecallingTime) RecallDrone();
		}
	}

	private void OnGUI ()
	{
		if (Active && droneIn)
			GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 15, 200, 25), "Recalling drone: " + (RecallingProgress / RecallingTime).ToString("P0"));
	}

	private void OnTriggerEnter (Collider colli)
	{
		if (colli.CompareTag("Player")) droneIn = true;
	}

	private void OnTriggerExit (Collider colli)
	{
		if (colli.CompareTag("Player"))
		{
			droneIn = false;
			RecallingProgress = 0;
		}
	}

	private void RecallDrone ()
	{
		foreach (var loot in drone.CollectedLoot)
			FlightController.RecieveLoot(loot);
		flightView.ExitFlightMode();
		Active = false;
	}
}