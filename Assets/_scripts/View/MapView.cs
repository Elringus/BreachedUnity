using UnityEngine;
using UnityEngine.UI;

public class MapView : BolideView
{
	private int selectedSector;
	private Text selectedSectorText;
	private Text sectorDescriptionText;
	private Button[] sectorButtons = new Button[5];
	private Button sendDroneButton;

	protected override void Awake ()
	{
		base.Awake();

		selectedSectorText = GameObject.Find("text_sector-name").GetComponent<Text>();
		sectorDescriptionText = GameObject.Find("text_sector-description").GetComponent<Text>();

		for (int i = 1; i < 5; i++)
		{
			int ic = i;
			sectorButtons[ic] = GameObject.Find("button_sector" + ic).GetComponent<Button>();
			sectorButtons[ic].OnClick(() => { 
				selectedSector = ic;
				selectedSectorText.text = "Sector " + ic;
				sectorDescriptionText.text = Text.Get(string.Format("Sector{0}Description", ic));
			});
		}

		sendDroneButton = GameObject.Find("button_send-drone").GetComponent<Button>();
		sendDroneButton.OnClick(() => {
			if (selectedSector != 0 && MapController.EnterSector())
			{
				SwitchView(selectedSector == 1 ? ViewType.Sector1 
					: selectedSector == 2 ? ViewType.Sector2 
					: selectedSector == 3 ? ViewType.Sector3 
					: ViewType.Sector4);
			}
		});

		selectedSector = 1;
		selectedSectorText.text = "Sector 1";
		sectorDescriptionText.text = Text.Get("Sector1Description");
	}

	protected override void Update ()
	{
		base.Update();

		sendDroneButton.interactable = selectedSector != 0 && State.CurrentAP >= State.EnterSectorAPCost;

		for (int i = 1; i < 5; i++)
		{
			if (selectedSector == i)
				sectorButtons[i].Select();
		}
	}
}