using UnityEngine;
using UnityEngine.UI;

public class EnginePanel : MonoBehaviour
{
	private Text wiringText;
	private Text alloyText;
	private Text chipsText;
	private Text componentText;
	private Button fixEngineButton;

	private void Awake ()
	{
		wiringText = transform.Find("text_wiring").GetComponent<Text>();
		alloyText = transform.Find("text_alloy").GetComponent<Text>();
		chipsText = transform.Find("text_chips").GetComponent<Text>();
		componentText = transform.Find("text_component").GetComponent<Text>();
		fixEngineButton = transform.Find("button_fix-engine").GetComponent<Button>();
		fixEngineButton.OnClick(() => WorkshopController.FixEngine());
	}

	private void Update ()
	{
		wiringText.text = string.Format("Wiring\n{0}/{1}", ServiceLocator.State.Wiring,
			ServiceLocator.State.FixEngineRequirements[ServiceLocator.State.BreakageType][0]);
		alloyText.text = string.Format("Alloy\n{0}/{1}", ServiceLocator.State.Alloy,
			ServiceLocator.State.FixEngineRequirements[ServiceLocator.State.BreakageType][1]);
		chipsText.text = string.Format("Chips\n{0}/{1}", ServiceLocator.State.Chips,
			ServiceLocator.State.FixEngineRequirements[ServiceLocator.State.BreakageType][2]);
		componentText.text = string.Format("Component\n{0}/1", WorkshopController.IdenticalArtifactAnalyzed() ? 1 : 0);

		fixEngineButton.interactable = WorkshopController.CanFixEngine();
	}
}