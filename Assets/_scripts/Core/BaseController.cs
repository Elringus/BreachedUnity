using UnityEngine;

public abstract class BaseController
{
	protected IState State;

	public BaseController ()
	{
		ServiceLocator.Initialize();

		State = ServiceLocator.State;
	}

	public void Navigate (ScreenType to)
	{
		switch (to)
		{
			case ScreenType.StateEditor:
				Application.LoadLevel(GlobalConfig.STATE_EDITOR_SCENE);
				break;
			case ScreenType.MainMenu:
				Application.LoadLevel(GlobalConfig.MAIN_MENU_SCENE);
				break;
			case ScreenType.SimpleView:
				Application.LoadLevel(GlobalConfig.SIMPLE_VIEW_SCENE);
				break;
		}
	}
}