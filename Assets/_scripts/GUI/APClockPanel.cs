using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class APClockPanel : MonoBehaviour
{
	public float APTransitionSpeeed = 2;

	private List<Image> apImages = new List<Image>(10);
	private Image arrow;

	private void Awake () 
	{
		foreach (Transform ap in transform.Find("ap"))
			apImages.Add(ap.GetComponent<Image>());
		arrow = transform.Find("image_clock-arrow").GetComponent<Image>();

		for (int i = 0; i < apImages.Count; i++)
			apImages[i].color = ServiceLocator.State.CurrentAP > (9 - i) ? Color.green : Color.red;
		arrow.transform.rotation = Quaternion.Euler(0, 0, (ServiceLocator.State.MaxAP - ServiceLocator.State.CurrentAP) * -36);
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

	}
}