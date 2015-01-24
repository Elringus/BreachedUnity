using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using GUIBlendModes;

public class ArtifactsPanel : MonoBehaviour
{
	private Artifact _selectedArtifact;
	public Artifact SelectedArtifact
	{
		get { return _selectedArtifact; }
		set 
		{
			if (value == _selectedArtifact) return;
			_selectedArtifact = value;
			UpdateArtifactView();
		}
	}

	private Image selectedArtifactImage;
	private UIBlendMode selectedArtifactBlendMode;
	private Text selectedArtifactNameText;
	private Text selectedArtifactScanInfoText;
	private Button analyzeSelectedArtifactButton;
	private Transform contentParent;
	private ScrollRect artifactsScroll;

	private void Awake () 
	{
		selectedArtifactImage = transform.Find("image_selected-artifact").GetComponent<Image>();
		selectedArtifactImage.color = Color.clear;
		selectedArtifactBlendMode = selectedArtifactImage.GetComponent<UIBlendMode>();
		selectedArtifactNameText = transform.Find("text_selected-artifact-name").GetComponent<Text>();
		selectedArtifactNameText.text = string.Empty;
		selectedArtifactScanInfoText = transform.Find("text_selected-artifact-scan-info").GetComponent<Text>();
		selectedArtifactScanInfoText.text = string.Empty;
		analyzeSelectedArtifactButton = transform.Find("button_analyze-artifact").GetComponent<Button>();
		analyzeSelectedArtifactButton.OnClick(() => {
			if (SelectedArtifact == null || SelectedArtifact.Status != ArtifactStatus.Found) return;

			WorkshopController.AnalyzeArtifact(SelectedArtifact);
			UpdateArtifactView();
		});
		contentParent = transform.Find("panel_artifacts/panel_scroll/content");

		foreach (var artifact in ServiceLocator.State.Artifacts)
		{
			if (artifact.Status == ArtifactStatus.NotFound) continue;

			var artifactButton = BaseView.AddUIElement("button_artifact", contentParent).GetComponent<ArtifactButton>();
			artifactButton.Initialize(artifact);
		}
		artifactsScroll = transform.Find("panel_artifacts/panel_scroll").GetComponent<ScrollRect>();
		artifactsScroll.verticalNormalizedPosition = 1;
	}

	private void Update () 
	{
		analyzeSelectedArtifactButton.interactable = 
			ServiceLocator.State.CurrentAP >= ServiceLocator.State.AnalyzeArtifactAPCost 
			&& SelectedArtifact != null 
			&& SelectedArtifact.Status == ArtifactStatus.Found;
	}

	private void UpdateArtifactView ()
	{
		selectedArtifactImage.color = Color.white;
		selectedArtifactImage.sprite = FindObjectsOfType<ArtifactButton>()
			.First(a => a.Artifact == SelectedArtifact).transform.Find("image_artifact").GetComponent<Image>().sprite;
		selectedArtifactBlendMode.BlendMode = SelectedArtifact.Status == ArtifactStatus.Analyzed ? BlendMode.Normal : BlendMode.Overlay;
		selectedArtifactNameText.text = SelectedArtifact.Status == ArtifactStatus.Analyzed ? SelectedArtifact.Name : "Unknown artifact";
		selectedArtifactScanInfoText.alignment = SelectedArtifact.Status == ArtifactStatus.Analyzed ? TextAnchor.UpperLeft : TextAnchor.UpperCenter;
		selectedArtifactScanInfoText.text = SelectedArtifact.Status == ArtifactStatus.Analyzed ? SelectedArtifact.ScanInfo
			: string.Format("\nWiring: {0}\n\nAlloy: {1}\n\nChips: {2}", SelectedArtifact.Wiring, SelectedArtifact.Alloy, SelectedArtifact.Chips);
	}
}