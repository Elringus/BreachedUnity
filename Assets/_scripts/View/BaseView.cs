using UnityEngine;

public abstract class BaseView : MonoBehaviour
{
	protected static IState State;
	protected static ILogger Logger;
	protected static IText Text;

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
		Application.LoadLevel("scn_" + to);
	}
}