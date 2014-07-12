using UnityEngine;
using System.Collections;

public class MainMenuController : BaseController
{
	public void StartNewGame ()
	{
		ServiceLocator.State = State.Reset();
		//State = ServiceLocator.State;
		State.StartedGame = true;
		Navigate(ScreenType.SimpleView);
	}

	public void ContinueGame ()
	{
		if (!State.StartedGame) return;
		Navigate(ScreenType.SimpleView);
	}

	public void ExitGame ()
	{
		Application.Quit();
	}
}