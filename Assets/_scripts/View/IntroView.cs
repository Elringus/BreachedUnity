using UnityEngine;

public class IntroView : BaseView
{
	protected override void Update ()
	{
		base.Update();

		if (Input.GetMouseButtonDown(0)) SwitchView(ViewType.Bridge);
	}
}