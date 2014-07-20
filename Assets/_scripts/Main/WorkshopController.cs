
public class WorkshopController : BaseController
{
	public bool AnalyzeArtifact (Artifact artifact)
	{
		if (State.CurrentAP < State.AnalyzeArtifactAPCost) return false;

		State.CurrentAP -= State.AnalyzeArtifactAPCost;
		artifact.ArtifactStatus = ArtifactStatus.Analyzing;
		return true;
	}
}