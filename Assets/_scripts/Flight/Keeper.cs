using UnityEngine;
using System.Collections;

public class Keeper : MonoBehaviour
{
	public float RandomDestinationArea = 30;

	private bool contructingPath;
	private NavMeshAgent navAgent;
	private Vector3 currentDestination;

	private void Awake () 
	{
		navAgent = GetComponent<NavMeshAgent>();
	}

	private void Start ()
	{
		StartCoroutine(SetRandomDestination());
	}

	private void Update () 
	{
		if (!contructingPath && Vector3.Distance(transform.position, currentDestination) < transform.localScale.magnitude)
			StartCoroutine(SetRandomDestination());

		//Debug.DrawLine(transform.position, currentDestination, Color.green);
	}

	private IEnumerator SetRandomDestination ()
	{
		contructingPath = true;
		Vector3? randomDestination = null;
		int grace = 0;

		while (!randomDestination.HasValue)
		{
			var area = Mathf.Clamp(RandomDestinationArea - grace, transform.localScale.magnitude, Mathf.Infinity);
			var randomPoint = Random.insideUnitSphere * area + transform.position;
			NavMeshHit hit;
			NavMesh.SamplePosition(randomPoint, out hit, area, 1);
			var path = new NavMeshPath();
			navAgent.CalculatePath(hit.position, path);
			if (path.status == NavMeshPathStatus.PathComplete) randomDestination = hit.position;
			grace += (int)transform.localScale.magnitude;

			yield return null;
		}

		currentDestination = randomDestination.Value;
		navAgent.SetDestination(randomDestination.Value);
		contructingPath = false;
	}
}