using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public string TooltipID;

	private Text consoleText;

	private void Awake () 
	{
		consoleText = GameObject.Find("text_console").GetComponent<Text>();
		consoleText.text = string.Empty;
	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		consoleText.text = ServiceLocator.Text.Get(TooltipID);
	}

	public void OnPointerExit (PointerEventData eventData)
	{
		consoleText.text = string.Empty;
	}
}