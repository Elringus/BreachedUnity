using UnityEngine;
using System.Collections;

public class LootSpot : MonoBehaviour
{
	public Loot Loot;

	private bool _active;
	public bool Active
	{
		get { return _active; }
		set { _active = value; collider.enabled = value; model.SetActive(value); }
	}

	private GameObject model;
	private Collider collider;
	private Vector3 randomRotation;

	private FlightController flightContoller;

	private void Awake ()
	{
		model = transform.Find("model").gameObject;
		collider = GetComponent<SphereCollider>();
		randomRotation = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
		Active = false;

		flightContoller = new FlightController();
	}

	private void Update ()
	{
		if (Active) model.transform.Rotate(randomRotation, 100 * Time.deltaTime);
	}

	public void RecieveLoot ()
	{
		flightContoller.RecieveLoot(Loot);
		Active = false;
	}
}