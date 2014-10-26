using UnityEngine;
using System.Collections;
using simul;
using System;

public class FlightView : BaseView
{
	public float MaxAbberation = 6;

	private trueSKY sky;
	private Vignetting vignetting;
	private ReliefTerrain RT;

	protected override void Awake ()
	{
		base.Awake();

		sky = GameObject.Find("trueSky").GetComponent<trueSKY>();
		vignetting = Camera.main.GetComponent<Vignetting>();
		RT = FindObjectOfType<ReliefTerrain>();
		RT.BumpGlobalCombined = new Texture2D(64, 64);
	}

	protected override void Update ()
	{
		base.Update();

		sky.speed += Input.GetAxis("Mouse ScrollWheel") * 10000 * Time.deltaTime;

		float curTime = sky.time - (float)Math.Truncate(sky.time);
		if (curTime > .5f) curTime = 1 - curTime;
		vignetting.blurDistance = MaxAbberation * curTime;
	}

	public void ExitFlightMode ()
	{
		SwitchView(ViewType.SimpleView);
	}
}