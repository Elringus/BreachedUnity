using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseView : MonoBehaviour
{
	public static readonly Color32 DEFAULT_TEXT_COLOR = new Color32(186, 255, 129, 255);
	public ViewType ActiveView
	{
		get { return (ViewType)Enum.Parse(typeof(ViewType), Application.loadedLevelName.Replace("scn_", string.Empty), true); }
	}

	protected static IState State { get { return ServiceLocator.State; } }
	protected static ILogger Logger { get { return ServiceLocator.Logger; } }
	protected static IText Text { get { return ServiceLocator.Text; } }

	static BaseView ()
	{
		Initialize();

		// disable quests invoking if we don't have the text provider
		if (Text.GetType() != typeof(NullText))
		{
			Events.StateUpdated += () =>
			{
				foreach (var quest in State.QuestRecords.Where(q => q.Status == QuestStatus.NotStarted))
					QuestController.StartQuest(quest);
				foreach (var record in State.JournalRecords.Where(r => r.Check()))
					record.AssignedDay = State.CurrentDay;
			};
		}
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
		if (GlobalConfig.RELEASE_TYPE == ReleaseType.alpha)
		{
			if (Input.GetKeyDown(KeyCode.F1)) SwitchView(ViewType.Sector1);
			if (Input.GetKeyDown(KeyCode.F2)) SwitchView(ViewType.Sector2);
			if (Input.GetKeyDown(KeyCode.F3)) SwitchView(ViewType.Sector3);
			if (Input.GetKeyDown(KeyCode.F4)) SwitchView(ViewType.Sector4);

			if (Input.GetKeyDown(KeyCode.F5)) SwitchView(ViewType.MainMenu);
		}
	}

	public void SwitchView (ViewType to)
	{
		Application.LoadLevel("scn_" + to);
	}

	public static GameObject AddUIElement (string prefabName, Transform parent = null)
	{
		if (!parent) parent = GameObject.Find("_gui").transform;

		var uiElement = Instantiate(Resources.Load(prefabName)) as GameObject;
		uiElement.name = prefabName;
		uiElement.transform.SetParent(parent, false);

		return uiElement;
	}
}