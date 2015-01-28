using UnityEngine;
using System.Collections;

public class BolideView : BaseView
{
	private const float VFX_TRANSITION_SPEED = 2;

	private Vignetting vignetting;
	private ContrastEnhance contrast;
	private float vigGlitch;
	private float targetVig;

	protected override void Awake ()
	{
		base.Awake();

		vignetting = GameObject.Find("_camera-back").GetComponent<Vignetting>();
		vignetting.blur = 50;
		vignetting.blurSpread = 50;
		contrast = GameObject.Find("_camera-back").GetComponent<ContrastEnhance>();
	}

	protected override void Start ()
	{
		base.Start();

		contrast.intensity = 20;
		StartCoroutine(GlitchTrigger());
	}

	protected override void Update ()
	{
		base.Update();

		targetVig = Mathf.Sin(Time.time / 2) * 3 + vigGlitch;
		if (vigGlitch != 0) vigGlitch = Mathf.Lerp(vigGlitch, 0, Time.deltaTime * Mathf.Clamp(State.CurrentDay / State.TotalDays, .1f, 1));

		vignetting.chromaticAberration = Mathf.Lerp(vignetting.chromaticAberration, targetVig, Time.deltaTime / 3);
		vignetting.blur = Mathf.Lerp(vignetting.blur, -VFX_TRANSITION_SPEED / 2, Time.deltaTime * VFX_TRANSITION_SPEED);
		vignetting.blurSpread = Mathf.Lerp(vignetting.blurSpread, -VFX_TRANSITION_SPEED / 2, Time.deltaTime * VFX_TRANSITION_SPEED);

		contrast.blurSpread = Mathf.Abs(Mathf.Sin(Time.time / 2)) * 7 + 1;
		contrast.intensity = Mathf.Lerp(contrast.intensity, 0, Time.deltaTime);
	}

	private IEnumerator GlitchTrigger ()
	{
		while (true)
		{
			if (Random.Range(0, (State.TotalDays - State.CurrentDay + 1) * 20) == 0)
			{
				vigGlitch = Random.Range(-State.CurrentDay * 12.5f, State.CurrentDay * 12.5f); // chrom. abberation from -100 to 100
				if (State.CurrentDay >= 3) contrast.intensity = State.CurrentDay * 2;
			}

			yield return new WaitForSeconds(.1f);
		}
	}
}