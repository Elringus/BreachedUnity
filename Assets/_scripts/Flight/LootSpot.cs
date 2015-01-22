using UnityEngine;
using System.Collections;

public class LootSpot : MonoBehaviour
{
	private Loot _loot;
	public Loot Loot
	{
		get { return _loot; }
		set { 
			_loot = value;
			model.renderer.material.SetColor("_EmissionColor",
				value.LootType == LootType.MineralA ? Color.blue :
				value.LootType == LootType.MineralB ? Color.green :
				value.LootType == LootType.MineralC ? Color.yellow :
				Color.white);
		}
	}

	private bool _active;
	public bool Active
	{
		get { return _active; }
		set { _active = value; myCollider.enabled = value; model.SetActive(value); }
	}

	public float HarvestProgress;
	public float HarvestTime = 5;

	private GameObject model;
	private Collider myCollider;
	private Vector3 randomRotation;

	private DroneController drone;
	private bool droneIn;

	private void Awake ()
	{
		model = transform.Find("model").gameObject;
		myCollider = GetComponent<SphereCollider>();
		randomRotation = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
		Active = false;

		drone = FindObjectOfType<DroneController>();
	}

	private void Update ()
	{
		if (Active)
		{
			model.transform.Rotate(randomRotation, 100 * (1 + HarvestProgress) * Time.deltaTime);
			if (droneIn && drone.EngineMode == EngineMode.Stop) HarvestProgress += Time.deltaTime;
			if (HarvestProgress >= HarvestTime) RecieveLoot();
		}
	}

	private void OnGUI ()
	{
		if (Active && droneIn)
			GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 15, 200, 25), "Harvesting: " + (HarvestProgress / HarvestTime).ToString("P0"));
	}

	private void OnTriggerEnter (Collider colli)
	{
		if (drone.LootCharges > 0 && colli.CompareTag("Player")) droneIn = true;
	}

	private void OnTriggerExit (Collider colli)
	{
		if (colli.CompareTag("Player")) droneIn = false;
	}

	public void RecieveLoot ()
	{
		drone.CollectedLoot.Add(Loot);
		drone.LootCharges--;
		Active = false;
	}
}