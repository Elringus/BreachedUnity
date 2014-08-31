using UnityEngine;
using UnityEngine.UI;

public abstract class BaseView : MonoBehaviour
{
	protected static IState State;
	protected static ILogger Logger;
	protected static IText Text;

	private Transform uiCanvas;

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
		var versionText = AddUIElement("text_version").GetComponent<Text>();
		versionText.text = string.Format("Breached {0}\nver. {1}.{2}.{3}", 
			GlobalConfig.RELEASE_TYPE, GlobalConfig.VERSION_MAJOR, GlobalConfig.VERSION_MIDDLE, GlobalConfig.VERSION_MINOR);
	}

	protected virtual void Update ()
	{

	}

	protected void SwitchView (ViewType to)
	{
		Application.LoadLevel("scn_" + to);
	}

	protected GameObject AddUIElement (string prefabName)
	{
		if (!uiCanvas) uiCanvas = GameObject.Find("ui-canvas").transform;

		GameObject uiElement = Instantiate(Resources.Load(prefabName)) as GameObject;
		uiElement.transform.SetParent(uiCanvas, false);

		return uiElement;
	}
}