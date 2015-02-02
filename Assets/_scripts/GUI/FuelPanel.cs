using UnityEngine;
using UnityEngine.UI;

public class FuelPanel : MonoBehaviour
{
	public float FillSpeed = 10;

	private Slider mineralASlider;
	private Slider mineralBSlider;
	private Slider mineralCSlider;
	private Button synthFuelButton;

	private Image mineralAFill;
	private Image mineralBFill;
	private Image mineralCFill;

	private Transform probesParent;

	private void Awake () 
	{
		mineralASlider = transform.Find("panel_sliders/slider_mineral-a").GetComponent<Slider>();
		mineralASlider.value = 1;
		mineralBSlider = transform.Find("panel_sliders/slider_mineral-b").GetComponent<Slider>();
		mineralBSlider.value = 1;
		mineralCSlider = transform.Find("panel_sliders/slider_mineral-c").GetComponent<Slider>();
		mineralCSlider.value = 1;

		synthFuelButton = transform.Find("button_synth-fuel").GetComponent<Button>();
		synthFuelButton.OnClick(() => { WorkshopController.SynthFuel(GetCurrentProbe()); AddProbe(GetCurrentProbe()); });

		mineralAFill = transform.Find("panel_triangle/image_triangle-fill-a").GetComponent<Image>();
		mineralAFill.fillAmount = 0;
		mineralBFill = transform.Find("panel_triangle/image_triangle-fill-b").GetComponent<Image>();
		mineralBFill.fillAmount = 0;
		mineralCFill = transform.Find("panel_triangle/image_triangle-fill-c").GetComponent<Image>();
		mineralCFill.fillAmount = 0;

		probesParent = transform.Find("panel_triangle/image_triangle-segments");
	}

	private void Start ()
	{
		foreach (var probe in ServiceLocator.State.FuelSynthProbes) AddProbe(probe);
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

		mineralAFill.fillAmount = Mathf.Lerp(mineralAFill.fillAmount, ServiceLocator.State.MineralA < 1 ? 0 : mineralASlider.normalizedValue / 1.2857f, Time.deltaTime * FillSpeed);
		mineralBFill.fillAmount = Mathf.Lerp(mineralBFill.fillAmount, ServiceLocator.State.MineralB < 1 ? 0 : mineralBSlider.normalizedValue / 1.2857f, Time.deltaTime * FillSpeed);
		mineralCFill.fillAmount = Mathf.Lerp(mineralCFill.fillAmount, ServiceLocator.State.MineralC < 1 ? 0 : mineralCSlider.normalizedValue / 1.2857f, Time.deltaTime * FillSpeed);
	}

	private int[] GetCurrentProbe () 
	{
		return new int[3] { (int)mineralASlider.value, (int)mineralBSlider.value, (int)mineralCSlider.value };
	}

	private void AddProbe (int[] probe)
	{
		var synthProbe = BaseView.AddUIElement("synth-probe", probesParent).GetComponent<SynthProbe>();
		synthProbe.Initialize(probe);
	}
}