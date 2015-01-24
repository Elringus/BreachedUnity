using UnityEngine;
using UnityEngine.UI;
using GUIBlendModes;

public class ArtifactButton : MonoBehaviour
{
	public Artifact Artifact;

	private ArtifactsPanel artifactsPanel;
	private Button artifactButton;
	private Image artifactImage;
	private UIBlendMode blendMode;

	private void Awake () 
	{
		artifactsPanel = FindObjectOfType<ArtifactsPanel>();
		artifactButton = GetComponent<Button>();
		artifactButton.OnClick(() => artifactsPanel.SelectedArtifact = Artifact);
		artifactImage = transform.Find("image_artifact").GetComponent<Image>();
		blendMode = artifactImage.GetComponent<UIBlendMode>();
	}

	private void Update () 
	{
		if (Artifact != null)
			blendMode.SetBlendMode(Artifact.Status == ArtifactStatus.Analyzed ? BlendMode.Normal : BlendMode.Overlay, true);
		if (artifactsPanel.SelectedArtifact == Artifact) artifactButton.Select();
	}

	public void Initialize (Artifact artifact)
	{
		Artifact = artifact;
		print("tex_" + Artifact.ID);
		artifactImage.sprite = Resources.Load<Sprite>("ArtifactIcons/tex_" + Artifact.ID);
	}
}