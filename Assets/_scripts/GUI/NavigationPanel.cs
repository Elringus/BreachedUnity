using UnityEngine;
using UnityEngine.UI;

public class NavigationPanel : MonoBehaviour
{
	private BaseView view;

	private Button toBridgeButton;
	private Button toWorkshopButton;
	private Button toMapButton;
	private Button toHorizonButton;
	private Button toMenuButton;

	private void Awake () 
	{
		view = FindObjectOfType<BaseView>();

		toBridgeButton = transform.FindChild("button_to-bridge").GetComponent<Button>();
		toBridgeButton.OnClick(() => view.SwitchView(ViewType.Bridge));

		toWorkshopButton = transform.FindChild("button_to-workshop").GetComponent<Button>();
		toWorkshopButton.OnClick(() => view.SwitchView(ViewType.Workshop));

		toMapButton = transform.FindChild("button_to-map").GetComponent<Button>();
		toMapButton.OnClick(() => view.SwitchView(ViewType.Map));

		toHorizonButton = transform.FindChild("button_to-horizon").GetComponent<Button>();
		toHorizonButton.OnClick(() => view.SwitchView(ViewType.Horizon));

		switch (view.ActiveView)
		{
			case ViewType.Bridge:
				SetSelectedState(toBridgeButton);
				break;
			case ViewType.Workshop:
				SetSelectedState(toWorkshopButton);
				break;
			case ViewType.Map:
				SetSelectedState(toMapButton);
				break;
			case ViewType.Horizon:
				SetSelectedState(toHorizonButton);
				break;
		}

		toMenuButton = transform.FindChild("button_to-menu").GetComponent<Button>();
		toMenuButton.OnClick(() => view.SwitchView(ViewType.MainMenu));
	}

	private void SetSelectedState (Button button)
	{
		button.interactable = false;
		button.GetComponent<Image>().color = Color.white;
		button.transform.Find("Image").GetComponent<Image>().color = Color.white;
		button.GetComponentInChildren<Text>().color = BaseView.DEFAULT_TEXT_COLOR;
	}

}