using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : BaseView
{
	private static bool connecting;
	private GameObject googlePanel;

	static MainMenuView ()
	{
		if (Text.GetType() == typeof(GoogleText) && Text.Get("STATE") == "NONE")
		{
			connecting = true;
			Events.TextUpdated += (c, e) =>
			{
				connecting = false;
			};
		}
	}

	protected override void Start ()
	{
		base.Start();

		if (GlobalConfig.RELEASE_TYPE != ReleaseType.RTM)
		{
			var menu = AddUIElement("panel_main-menu-dev").transform;
			menu.FindChild("button_state-editor").GetComponent<Button>()
				.onClick.AddListener(() => SwitchView(ViewType.StateEditor));
			menu.FindChild("button_simple-view").GetComponent<Button>()
				.onClick.AddListener(() => SwitchView(ViewType.SimpleView));
			menu.FindChild("button_update-text").GetComponent<Button>()
				.onClick.AddListener(() => {
					connecting = true;
					ServiceLocator.Text = new GoogleText(); 
					googlePanel = AddUIElement("panel_google-text"); 
				});
			menu.FindChild("button_new-game").GetComponent<Button>()
				.onClick.AddListener(() => { });
			menu.FindChild("button_continue").GetComponent<Button>()
				.onClick.AddListener(() => { });
			menu.FindChild("button_settings").GetComponent<Button>()
				.onClick.AddListener(() => { });
			menu.FindChild("button_exit").GetComponent<Button>()
				.onClick.AddListener(() => Application.Quit());

			if (Text.GetType() == typeof(GoogleText)) 
				googlePanel = AddUIElement("panel_google-text");
		}
		else
		{
			var menu = AddUIElement("panel_main-menu").transform;
			menu.FindChild("button_new-game").GetComponent<Button>()
				.onClick.AddListener(() => { });
			menu.FindChild("button_continue").GetComponent<Button>()
				.onClick.AddListener(() => { });
			menu.FindChild("button_settings").GetComponent<Button>()
				.onClick.AddListener(() => { });
			menu.FindChild("button_exit").GetComponent<Button>()
				.onClick.AddListener(() => Application.Quit());
		}
	}

	protected override void Update ()
	{
		base.Update();

		if (!connecting && googlePanel) Destroy(googlePanel);
	}
}