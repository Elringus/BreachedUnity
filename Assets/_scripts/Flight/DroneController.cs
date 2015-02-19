using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DroneController : MonoBehaviour
{
	[HideInInspector]
	public Transform Transform;
	[HideInInspector]
	public List<Loot> CollectedLoot = new List<Loot>();
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
	[HideInInspector]
	public EngineMode EngineMode;

	[Header("Speed")]
	public float NormalMoveSpeed = 70;
	public float FallSpeed = 50;
	[Range(.00f, .99f)]
	public float SpeedLossByLootCharge = .2f;

	[Header("Acceleration")]
	public float AccelMoveSpeed = 170;
	public float AccelSteeringDamp = 5;
	public float AccelRegenRate = .1f;
	public float AccelBurnRate = .5f;

	[Header("Control")]
	public float HorControlDamping = 1;
	public float VerControlDamping = 15;
	public float MaxHorAccel = 500;
	public float MaxVerAccel = 30;
	public float SteeringStart = 50;
	public float SteeringSpeed = 10;
	public float SkewAmount = 15;
	public float CameraEasing = 3;

	private int _lootCharges;
	private float accelCharge = 1;
	private bool accelBlock;
	private float curSteerSpeed;

	private Transform lookCamera;
	private CharacterController charController;

	private void Awake ()
	{
		Transform = transform;
		lookCamera = Camera.main.transform;
		charController = GetComponent<CharacterController>();
		LootCharges = ServiceLocator.State.LootCharges;

		curSteerSpeed = SteeringSpeed;
	}

	private void Update ()
	{
		if (Input.GetKeyDown(KeyCode.LeftControl)) EngineMode = EngineMode == EngineMode.Freeze ? EngineMode.Normal : EngineMode.Freeze;

		if (EngineMode != EngineMode.Freeze)
		{
			#region MOVING
			EngineMode = (Input.GetMouseButton(1) || Input.GetKey(KeyCode.Space)) ? EngineMode.Stop :
			 (Input.GetMouseButton(0) || Input.GetKey(KeyCode.LeftShift)) && accelCharge > 0 && !accelBlock ? EngineMode.Accel : EngineMode.Normal;

			if (EngineMode != EngineMode.Stop) charController.Move(Transform.forward * (EngineMode == EngineMode.Accel ? AccelMoveSpeed : NormalMoveSpeed) * Time.deltaTime);
			if (!charController.isGrounded) charController.Move(Vector3.down * FallSpeed * Time.deltaTime);
			#endregion

			#region ROTATING
			float horDelta = 0;
			float verDelta = 0;

			horDelta = (Input.mousePosition.x - Screen.width / 2f) / HorControlDamping;
			verDelta = (Input.mousePosition.y - Screen.height / 2f) / -VerControlDamping;

			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) horDelta = -MaxHorAccel;
			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) horDelta = MaxHorAccel;
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) verDelta = -MaxVerAccel;
			if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) verDelta = MaxVerAccel;

			curSteerSpeed = Mathf.Lerp(curSteerSpeed, 
				EngineMode == EngineMode.Accel ? SteeringSpeed / AccelSteeringDamp : EngineMode == EngineMode.Stop ? SteeringSpeed + AccelSteeringDamp * 2 : SteeringSpeed,
				Time.deltaTime);

			if (Mathf.Abs(horDelta) > SteeringStart)
			{
				Transform.Rotate(new Vector3(0, Mathf.Clamp(horDelta, -MaxHorAccel, MaxHorAccel) * curSteerSpeed / 100 * Time.deltaTime));
				Transform.rotation = Quaternion.Lerp(Transform.rotation, Quaternion.Euler(0, Transform.eulerAngles.y,
					horDelta < 0 ? SkewAmount * (Mathf.Abs(horDelta) / MaxHorAccel) : -SkewAmount * (Mathf.Abs(horDelta) / MaxHorAccel)), curSteerSpeed * Time.deltaTime);
			}
			else Transform.rotation = Quaternion.Lerp(Transform.rotation, Quaternion.Euler(0, Transform.eulerAngles.y, 0), curSteerSpeed * Time.deltaTime);
			#endregion

			#region CAMERA
			lookCamera.position = Vector3.Lerp(lookCamera.position, Transform.position, CameraEasing * Time.deltaTime);
			lookCamera.rotation = Quaternion.Lerp(lookCamera.rotation, Quaternion.Euler(verDelta, Transform.eulerAngles.y, Transform.eulerAngles.z), CameraEasing * Time.deltaTime);
			#endregion
		}

		if (EngineMode == EngineMode.Accel) accelCharge -= accelCharge <= 0 ? 0 : AccelBurnRate * Time.deltaTime;
		else accelCharge += accelCharge >= 1 ? 0 : AccelRegenRate * Time.deltaTime * (EngineMode == EngineMode.Stop ? 2 : 1);

		if (EngineMode == EngineMode.Accel && accelCharge <= 0 && !accelBlock) { EngineMode = EngineMode.Normal; accelBlock = true; }
		if (accelBlock && (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.LeftShift))) accelBlock = false;
	}

	private void OnGUI ()
	{
		GUILayout.Box("Loot charges: " + LootCharges);
		//GUILayout.Box("Accel charge: " + accelCharge.ToString("P0"));
	}
}