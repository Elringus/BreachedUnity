using UnityEngine;
using UnityEngine.UI;

public class FuelPanel : MonoBehaviour
{
	private Slider mineralASlider;
	private Slider mineralBSlider;
	private Slider mineralCSlider;
	private Button synthFuelButton;

	private void Awake () 
	{
		mineralASlider = transform.Find("panel_sliders/slider_mineral-a").GetComponent<Slider>();
		mineralASlider.value = 1;
		mineralBSlider = transform.Find("panel_sliders/slider_mineral-b").GetComponent<Slider>();
		mineralBSlider.value = 1;
		mineralCSlider = transform.Find("panel_sliders/slider_mineral-c").GetComponent<Slider>();
		mineralCSlider.value = 1;
		synthFuelButton = transform.Find("button_synth-fuel").GetComponent<Button>();
		synthFuelButton.OnClick(() => WorkshopController.SynthFuel(GetCurrentProbe()));
	}

	private void Update () 
	{
		if (ServiceLocator.State.MineralA < 1) mineralASlider.interactable = false;
		else mineralASlider.value = Mathf.Clamp(mineralASlider.value, 1, ServiceLocator.State.MineralA);
		if (ServiceLocator.State.MineralB < 1) mineralBSlider.interactable = false;
		else mineralBSlider.value = Mathf.Clamp(mineralBSlider.value, 1, ServiceLocator.State.MineralB);
		if (ServiceLocator.State.MineralC < 1) mineralCSlider.interactable = false;
		else mineralCSlider.value = Mathf.Clamp(mineralCSlider.value, 1, ServiceLocator.State.MineralC);

		synthFuelButton.interactable = WorkshopController.CanSynthFuel(GetCurrentProbe());
	}

	private int[] GetCurrentProbe () 
	{
		return new int[3] { (int)mineralASlider.value, (int)mineralBSlider.value, (int)mineralCSlider.value };
	}
}