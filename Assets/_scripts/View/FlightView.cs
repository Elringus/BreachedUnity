using UnityEngine;
using System.Collections;
using simul;
using System;

public class FlightView : BaseView
{
	public float MaxAbberation = 6;

	private trueSKY sky;
	private Vignetting vignetting;

	protected override void Awake ()
	{
		base.Awake();

		sky = GameObject.Find("trueSky").GetComponent<trueSKY>();
		vignetting = Camera.main.GetComponent<Vignetting>();
	}

	protected override void Update ()
	{
		base.Update();

		sky.time += Input.GetAxis("Mouse ScrollWheel") * 10 * Time.deltaTime;

		float curTime = sky.time - (float)Math.Truncate(sky.time);
		if (curTime > .5f) curTime = 1 - curTime;
		vignetting.blurDistance = MaxAbberation * curTime;
	}
}