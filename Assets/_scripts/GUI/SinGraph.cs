using UnityEngine;

public class SinGraph : MonoBehaviour
{
	public float Speed = 5;
	public float Amplitude = 1;

	private const float X_START = -6.29f;
	private const float X_END = -3.92f;
	private const float Y_LOW = -.6f;
	private const float Y_HIGH = -.3f;

	private LineRenderer lineRenderer;

	private void Awake () 
	{
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.SetVertexCount(50);
	}

	private void Update () 
	{
		for (int i = 0; i < 50; i++)
			lineRenderer.SetPosition(i, new Vector3(X_START + i / 50f * (X_END - X_START), Mathf.Sin(Time.time * Speed + i) / 6.6f * Amplitude - .45f, 1));
	}
}