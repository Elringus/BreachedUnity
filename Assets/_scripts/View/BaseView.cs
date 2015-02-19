using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseView : MonoBehaviour
{
	public static bool Initialized;
	public static readonly Color32 DEFAULT_TEXT_COLOR = new Color32(186, 255, 129, 255);
	public ViewType ActiveView
	{
		get { return (ViewType)Enum.Parse(typeof(ViewType), Application.loadedLevelName.Replace("scn_", string.Empty), true); }
	}

	protected static IState State { get { return ServiceLocator.State; } }
	protected static ILogger Logger { get { return ServiceLocator.Logger; } }
	protected static IText Text { get { return ServiceLocator.Text; } }

	private static void Initialize ()
	{
		ServiceLocator.Logger = GlobalConfig.RELEASE_TYPE != ReleaseType.RTM ? new UnityLogger() as ILogger : new NullLogger() as ILogger;
		ServiceLocator.State = FileState.Load();
		//ServiceLocator.Text = GlobalConfig.RELEASE_TYPE == ReleaseType.alpha ? new GoogleText() as IText : new LocalText() as IText;

		//if (ServiceLocator.State.VersionMiddle < GlobalConfig.VERSION_MIDDLE || 
		//	ServiceLocator.State.VersionMajor < GlobalConfig.VERSION_MAJOR)
		//{
		//	ServiceLocator.State.Reset(true);
		//	Initialize();
		//	ServiceLocator.Logger.LogWarning("The saved state is outdated and will be reseted!");
		//	return;
		//}

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

		Initialized = true;
	}

	protected virtual void Awake ()
	{
		if (!Initialized) Initialize();

		Events.LogHandlersCount();
	}

	protected virtual void Start ()
	{

	}

	protected virtual void Update ()
	{
		if (GlobalConfig.RELEASE_TYPE != ReleaseType.RTM)
		{
			if (Input.GetKeyDown(KeyCode.F1)) SwitchView(ViewType.Sector1);
			if (Input.GetKeyDown(KeyCode.F2)) SwitchView(ViewType.Sector2);
			if (Input.GetKeyDown(KeyCode.F3)) SwitchView(ViewType.Sector3);
			if (Input.GetKeyDown(KeyCode.F4)) SwitchView(ViewType.Sector4);

			if (Input.GetKeyDown(KeyCode.F5)) SwitchView(ViewType.MainMenu);
		}

		if (State.GameStatus == GameStatus.GameWin && ActiveView != ViewType.GameWin)
			SwitchView(ViewType.GameWin);
		if (State.GameStatus == GameStatus.GameOver && ActiveView != ViewType.GameOver)
			SwitchView(ViewType.GameOver);
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