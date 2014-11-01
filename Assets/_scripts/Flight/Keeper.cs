using UnityEngine;
using System.Collections;

public class Keeper : MonoBehaviour
{
	[HideInInspector]
	public Transform Transform;

	public float RandomDestinationArea = 30;
	public float PursuitRange = 50;

	private KeeperState _state;
	public KeeperState State
	{
		get { return _state; }
		set 
		{
			switch (value)
			{
				case KeeperState.Pursuiting:
					navAgent.SetDestination(drone.Transform.position);
					_state = value;
					break;
				case KeeperState.Stalking:
					if (_state == KeeperState.ConstructingPath) break;
					if (currentDestination == Vector3.zero || Vector3.Distance(Transform.position, currentDestination) < Transform.localScale.magnitude)
					{
						StartCoroutine(SetRandomDestination());
						break;
					}
					else navAgent.SetDestination(currentDestination);
					_state = value;
					break;
				case KeeperState.ConstructingPath:
					_state = value;
					break;
			}
		}
	}

	private NavMeshAgent navAgent;
	private Vector3 currentDestination;

	private float pursuitTimer;
	private DroneController drone;

	private void Awake () 
	{
		Transform = transform;
		navAgent = GetComponent<NavMeshAgent>();
		drone = FindObjectOfType<DroneController>();
	}

	private void Start ()
	{
		State = KeeperState.Stalking;
	}

	private void Update () 
	{
		State = Vector3.Distance(Transform.position, drone.Transform.position) < PursuitRange ? KeeperState.Pursuiting : KeeperState.Stalking;
	}

	private IEnumerator SetRandomDestination ()
	{
		State = KeeperState.ConstructingPath;
		Vector3? randomDestination = null;
		int grace = 0;

		while (!randomDestination.HasValue)
		{
			var area = Mathf.Clamp(RandomDestinationArea - grace, Transform.localScale.magnitude, Mathf.Infinity);
			var randomPoint = Random.insideUnitSphere * area + Transform.position;
			NavMeshHit hit;
			NavMesh.SamplePosition(randomPoint, out hit, area, 1);
			var path = new NavMeshPath();
			navAgent.CalculatePath(hit.position, path);
			if (path.status == NavMeshPathStatus.PathComplete) randomDestination = hit.position;
			grace += (int)Transform.localScale.magnitude;

			yield return null;
		}

		currentDestination = randomDestination.Value;
		navAgent.SetDestination(randomDestination.Value);
		_state = State = KeeperState.Stalking;
	}
}