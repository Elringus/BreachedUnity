using UnityEngine;

public abstract class BaseView : MonoBehaviour
{
	protected IState State;
	protected ILogger Logger;

	protected virtual void Awake ()
	{
		ServiceLocator.Initialize();

		State = ServiceLocator.State;
		Logger = ServiceLocator.Logger;
	}

	protected virtual void Start ()
	{

	}

	protected virtual void Update ()
	{

	}

	protected virtual void OnGUI ()
	{
		GUI.Label(new Rect(10, Screen.height - 40, 150, 80), string.Format("Breached {0}\nver. {1}.{2}.{3}",
			GlobalConfig.RELEASE_TYPE, GlobalConfig.VERSION_MAJOR, GlobalConfig.VERSION_MIDDLE, GlobalConfig.VERSION_MINOR));
	}

	protected void SwitchView (ViewType to)
	{
		switch (to)
		{
			case ViewType.StateEditor:
				Application.LoadLevel(GlobalConfig.STATE_EDITOR_SCENE);
				break;
			case ViewType.MainMenu:
				Application.LoadLevel(GlobalConfig.MAIN_MENU_SCENE);
				break;
			case ViewType.SimpleView:
				Application.LoadLevel(GlobalConfig.SIMPLE_VIEW_SCENE);
				break;
			case ViewType.Intro:
				Application.LoadLevel(GlobalConfig.INTRO_SCENE);
				break;
			case ViewType.Bridge:
				Application.LoadLevel(GlobalConfig.BRIDGE_SCENE);
				break;
		}
	}
}