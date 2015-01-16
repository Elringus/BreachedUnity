using UnityEngine;

public class FPS : MonoBehaviour
{
	public bool ShowFPS = true;

	private float fps;
	private float accum;
	private float timeleft = .5f;
	private int frames;

	private void Awake ()
	{
		ShowFPS = GlobalConfig.RELEASE_TYPE == ReleaseType.alpha;
	}

	private void Update () 
	{
		if (Input.GetKeyDown(KeyCode.F11)) 
			ShowFPS = !ShowFPS;

		timeleft -= Time.deltaTime;
		accum += Time.timeScale / Time.deltaTime;
		++frames;

		if (timeleft <= 0)
		{
			fps = Mathf.Ceil(accum / frames);
			timeleft = .5f;
			accum = 0;
			frames = 0;
		}
	}

	private void OnGUI ()
	{
		GUI.Box(new Rect(0, 0, 50, 25), fps.ToString());
	}
}