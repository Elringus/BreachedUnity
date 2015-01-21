using UnityEngine;
using UnityEngine.UI;

public class StatusPanel : MonoBehaviour
{
	public Sprite OKImage;
	public Sprite WarningImage;

	private Image engineStatusImage;
	private Text engineStatusText;
	private Image fuelStatusImage;
	private Text fuelStatusText;
	private Text daysRemainingText;

	private void Awake () 
	{
		engineStatusImage = transform.Find("image_engine-status").GetComponent<Image>();
		engineStatusText = transform.Find("text_engine-status").GetComponent<Text>();
		fuelStatusImage = transform.Find("image_engine-status").GetComponent<Image>();
		fuelStatusText = transform.Find("text_fuel-status").GetComponent<Text>();
		daysRemainingText = transform.Find("text_days-remaining").GetComponent<Text>();
	}

	private void Update () 
	{
		engineStatusImage.sprite = ServiceLocator.State.EngineFixed ? OKImage : WarningImage;
		engineStatusText.text = ServiceLocator.State.EngineFixed ? "The core is fixed" : "The core is damaged";
		fuelStatusImage.sprite = ServiceLocator.State.FuelSynthed ? OKImage : WarningImage;
		fuelStatusText.text = ServiceLocator.State.FuelSynthed ? "The fuel is synthed" : "Fuel tank is empty";
		daysRemainingText.text = (ServiceLocator.State.TotalDays - ServiceLocator.State.CurrentDay + 1) + " days reamining";
	}
}