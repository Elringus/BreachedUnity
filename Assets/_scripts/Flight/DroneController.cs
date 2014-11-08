using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DroneController : MonoBehaviour
{
	[HideInInspector]
	public Transform Transform;

	private int _lootCharges;
	public int LootCharges
	{
		get { return _lootCharges; }
		set
		{
			_lootCharges = value;
			if (value != ServiceLocator.State.LootCharges) 
				NormalMoveSpeed *= 1 - SpeedLossByLootCharge;
		}
	}
	[Range(.01f, .99f)]
	public float SpeedLossByLootCharge = .2f;

	public float AccelMoveSpeed = 170;
	public float NormalMoveSpeed = 70;
	public float FallSpeed = 50;

	public float GeneralEasing = 3;

	public float HorControlDamping = 1;
	public float VerControlDamping = 15;
	public float MaxHorAccel = 500;
	public float MaxVerAccel = 30;
	public float SteeringLimit = 50;
	public float SteeringSpeed = 10;
	public float SteeringSkew = 15;

	[HideInInspector]
	public EngineMode EngineMode;

	public float AccelRegenRate = .1f;
	public float AccelBurnRate = .5f;
	private float accelCharge = 1;
	private bool accelBlock;

	private Transform lookCamera;
	private CharacterController charController;

	private void Awake ()
	{
		Transform = transform;
		lookCamera = Camera.main.transform;
		charController = GetComponent<CharacterController>();
		LootCharges = ServiceLocator.State.LootCharges;
	}

	private void Update ()
	{
		if (Input.GetKeyDown(KeyCode.LeftControl)) EngineMode = EngineMode == EngineMode.Freeze ? EngineMode.Normal : EngineMode.Freeze;

		if (EngineMode != EngineMode.Freeze)
		{
			EngineMode = (Input.GetMouseButton(1) || Input.GetKey(KeyCode.Space)) ? EngineMode.Stop :
						 (Input.GetMouseButton(0) || Input.GetKey(KeyCode.LeftShift)) && accelCharge > 0 && !accelBlock ? EngineMode.Accel : EngineMode.Normal;

			if (EngineMode != EngineMode.Stop) charController.Move(Transform.forward * (EngineMode == EngineMode.Accel ? AccelMoveSpeed : NormalMoveSpeed) * Time.deltaTime);
			if (!charController.isGrounded) charController.Move(Vector3.down * FallSpeed * Time.deltaTime);

			float horDelta = 0;
			float verDelta = 0;

			horDelta = (Input.mousePosition.x - (float)Screen.width / 2f) / HorControlDamping;
			verDelta = (Input.mousePosition.y - (float)Screen.height / 2f) / -VerControlDamping;

			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) horDelta = -MaxHorAccel;
			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) horDelta = MaxHorAccel;
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) verDelta = -MaxVerAccel;
			if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) verDelta = MaxVerAccel;

			if (Mathf.Abs(horDelta) > SteeringLimit)
			{
				Transform.Rotate(new Vector3(0, Mathf.Clamp(horDelta, -MaxHorAccel, MaxHorAccel) * Time.deltaTime / (EngineMode == EngineMode.Stop ? SteeringSpeed / 2 : SteeringSpeed), 0));
				Transform.rotation = Quaternion.Lerp(Transform.rotation, Quaternion.Euler(0, Transform.eulerAngles.y, horDelta < 0 ? SteeringSkew : -SteeringSkew), GeneralEasing * Time.deltaTime);
			}
			else Transform.rotation = Quaternion.Lerp(Transform.rotation, Quaternion.Euler(0, Transform.eulerAngles.y, 0), GeneralEasing * Time.deltaTime);

			lookCamera.position = Vector3.Lerp(lookCamera.position, Transform.position, GeneralEasing * Time.deltaTime);
			lookCamera.rotation = Quaternion.Lerp(lookCamera.rotation, Quaternion.Euler(verDelta, Transform.eulerAngles.y, Transform.eulerAngles.z), GeneralEasing * Time.deltaTime);
		}

		if (EngineMode == EngineMode.Accel) accelCharge -= accelCharge <= 0 ? 0 : AccelBurnRate * Time.deltaTime;
		else accelCharge += accelCharge >= 1 ? 0 : AccelRegenRate * Time.deltaTime * (EngineMode == EngineMode.Stop ? 2 : 1);

		if (EngineMode == EngineMode.Accel && accelCharge <= 0 && !accelBlock) { EngineMode = EngineMode.Normal; accelBlock = true; }
		if (accelBlock && (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.LeftShift))) accelBlock = false;
	}

	private void OnGUI ()
	{
		GUILayout.Box("Loot charges: " + LootCharges);
		GUILayout.Box("Accel charge: " + accelCharge.ToString("P0"));
	}
}