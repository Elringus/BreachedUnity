using UnityEngine;

public class GameWinView : BaseView
{
	protected override void Update ()
	{
		base.Update();

		if (Input.GetMouseButtonDown(0)) { State.Reset(); SwitchView(ViewType.MainMenu); }
	}
}