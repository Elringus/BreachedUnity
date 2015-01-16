using UnityEngine;

public class GameFailView : BaseView
{
	protected override void Update ()
	{
		base.Update();

		if (Input.GetMouseButtonDown(0)) { State.Reset(); SwitchView(ViewType.MainMenu); }
	}
}