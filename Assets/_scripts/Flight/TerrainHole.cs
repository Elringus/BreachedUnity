using UnityEngine;
using System.Collections;

public class TerrainHole : MonoBehaviour
{
	private void OnTriggerEnter (Collider colli)
	{
		if (colli.CompareTag("Player")) 
			Physics.IgnoreLayerCollision(8, 9, true); 
	}

	private void OnTriggerExit (Collider colli)
	{
		if (colli.CompareTag("Player"))
			Physics.IgnoreLayerCollision(8, 9, false); 
	}
}