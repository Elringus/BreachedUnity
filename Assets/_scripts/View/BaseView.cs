using UnityEngine;

public abstract class BaseView : MonoBehaviour
{
	protected static IState State;
	protected static ILogger Logger;
	protected static IText Text;

	protected static readonly string STATE_EDITOR_SCENE = "scn_StateEditor";
	protected static readonly string SIMPLE_VIEW_SCENE = "scn_SimpleView";

	protected static readonly string MAIN_MENU_SCENE = "scn_MainMenu";
	protected static readonly string INTRO_SCENE = "scn_Intro";

	protected static readonly string BRIDGE_SCENE = "scn_Bridge";
	protected static readonly string WORKSHOP_SCENE = "scn_Workshop";
	protected static readonly string MAP_SCENE = "scn_Map";
	protected static readonly string HORIZON_SCENE = "scn_Horizon";

	protected static readonly string SECTOR_1_SCENE = "scn_Sector1";
	protected static readonly string SECTOR_2_SCENE = "scn_Sector2";
	protected static readonly string SECTOR_3_SCENE = "scn_Sector3";
	protected static readonly string SECTOR_4_SCENE = "scn_Sector4";

	static BaseView ()
	{
		Initialize();

		State = ServiceLocator.State;
		Logger = ServiceLocator.Logger;
		Text = ServiceLocator.Text;
	}

	private static void Initialize ()
	{
		ServiceLocator.Logger = new UnityLogger();
		ServiceLocator.State = FileState.Load();
		ServiceLocator.Text = new GoogleText();

		//if (ServiceLocator.State.VersionMiddle < GlobalConfig.VERSION_MIDDLE || 
		//	ServiceLocator.State.VersionMajor < GlobalConfig.VERSION_MAJOR)
		//{
		//	ServiceLocator.State.Reset(true);
		//	Initialize();
		//	ServiceLocator.Logger.LogWarning("The saved state is outdated and will be reseted!");
		//	return;
		//}
	}

	protected virtual void Awake ()
	{
		Events.LogHandlersCount();
	}

	protected virtual void Start ()
	{

	}

	protected virtual void Update ()
	{

	}

	protected void SwitchView (ViewType to)
	{
		switch (to)
		{
			case ViewType.StateEditor:
				Application.LoadLevel(STATE_EDITOR_SCENE);
				break;
			case ViewType.MainMenu:
				Application.LoadLevel(MAIN_MENU_SCENE);
				break;
			case ViewType.SimpleView:
				Application.LoadLevel(SIMPLE_VIEW_SCENE);
				break;
			case ViewType.Intro:
				Application.LoadLevel(INTRO_SCENE);
				break;
			case ViewType.Bridge:
				Application.LoadLevel(BRIDGE_SCENE);
				break;
		}
	}
}