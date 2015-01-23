using UnityEngine;
using UnityEngine.UI;
using GUIBlendModes;

public class ArtifactButton : MonoBehaviour
{
	public Artifact Artifact;

	private Button artifactButton;
	private Image artifactImage;
	private UIBlendMode blendMode;

	private void Awake () 
	{
		artifactButton = GetComponent<Button>();
		artifactImage = transform.Find("image_artifact").GetComponent<Image>();
		blendMode = artifactImage.GetComponent<UIBlendMode>();
	}

	private void Update () 
	{
    	
	}

	public void Initialize (Artifact artifact)
	{
		Artifact = artifact;

		artifactImage.sprite = Resources.Load("tex_" + Artifact.ID) as Sprite;
	}
}