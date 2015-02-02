using UnityEngine;
using UnityEngine.UI;

public class SynthProbe : MonoBehaviour
{
	public Sprite TrueProbeSprite;
	public Sprite CloseProbeSprite;
	public Sprite WrongProbeSprite;

	private Image image;
	private RectTransform trs;
	private RectTransform parentTRS;

	public void Initialize (int[] probe, bool spot = false)
	{
		image = GetComponent<Image>();
		trs = GetComponent<RectTransform>();
		parentTRS = trs.parent as RectTransform;

		trs.anchoredPosition = CalculateProbePos(probe[0], probe[2]);

		if (spot) return;

		var probeType = WorkshopController.MeasureProbe(probe);
		image.sprite = probeType == ProbeType.True ? TrueProbeSprite
			: probeType == ProbeType.Close ? CloseProbeSprite
			: WrongProbeSprite;
	}

	private Vector2 CalculateProbePos (float cA, float cC)
	{
		float a = parentTRS.sizeDelta.x / 9;
		return new Vector2((2 * cA + cC) / 2 * a + 3, Mathf.Sqrt(3) / 2 * cC * a) - trs.sizeDelta / 2;
	}
}