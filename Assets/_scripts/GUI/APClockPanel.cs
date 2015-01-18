using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class APClockPanel : MonoBehaviour
{
	public float APTransitionSpeeed = 2;

	private BridgeController bridgeController;
	private List<Image> apImages = new List<Image>(10);
	private Image arrow;
	private Button endDayButton;
	private Text dateText;
	private int year;

	private void Awake () 
	{
		bridgeController = new BridgeController();

		foreach (Transform ap in transform.Find("ap"))
			apImages.Add(ap.GetComponent<Image>());
		arrow = transform.Find("image_clock-arrow").GetComponent<Image>();

		for (int i = 0; i < apImages.Count; i++)
			apImages[i].color = ServiceLocator.State.CurrentAP > (9 - i) ? Color.green : Color.red;
		arrow.transform.rotation = Quaternion.Euler(0, 0, (ServiceLocator.State.MaxAP - ServiceLocator.State.CurrentAP) * -36);

		endDayButton = GetComponentInChildren<Button>();
		endDayButton.OnClick(() => bridgeController.EndDay());

		dateText = transform.Find("text_date").GetComponent<Text>();

		year = Random.Range(2330, 7777);
	}

	private void Update () 
	{
		if (GlobalConfig.RELEASE_TYPE == ReleaseType.alpha)
		{
			if (Input.GetKeyDown(KeyCode.KeypadPlus)) ServiceLocator.State.CurrentAP++;
			if (Input.GetKeyDown(KeyCode.KeypadMinus)) ServiceLocator.State.CurrentAP--;
		}

		for (int i = 0; i < apImages.Count; i++)
			apImages[i].color = Color.Lerp(apImages[i].color, 
				ServiceLocator.State.CurrentAP > (9 - i) ? Color.green : Color.red, 
				Time.deltaTime * APTransitionSpeeed);

		arrow.transform.rotation = Quaternion.Lerp(arrow.transform.rotation, 
			Quaternion.Euler(0, 0, (ServiceLocator.State.MaxAP - ServiceLocator.State.CurrentAP) * -36), 
			Time.deltaTime * APTransitionSpeeed);

		dateText.text = string.Format("{0} December\nyear: {1}", 12 + ServiceLocator.State.CurrentDay, year);
	}
}