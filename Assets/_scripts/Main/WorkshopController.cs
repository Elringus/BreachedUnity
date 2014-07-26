
public class WorkshopController : BaseController
{
	public bool AnalyzeArtifact (Artifact artifact)
	{
		if (State.CurrentAP < State.AnalyzeArtifactAPCost) return false;

		State.CurrentAP -= State.AnalyzeArtifactAPCost;
		artifact.Status = ArtifactStatus.Analyzing;
		return true;
	}

	public bool FixEngine ()
	{
		if (State.CurrentAP < State.FixEngineAPCost || !CanFixEngine()) 
			return false;

		State.EngineFixed = true;

		State.CurrentAP -= State.FixEngineAPCost;
		return true;
	}

	public bool CanFixEngine ()
	{
		var requirements = State.FixEngineRequirements[State.BreakageType];
		return !State.EngineFixed && 
			State.Wiring >= requirements[0] && State.Alloy >= requirements[1] && State.Chips >= requirements[2] && 
			State.Artifacts.Find(x => x.Identity.GetValueOrDefault() == State.BreakageType).Status == ArtifactStatus.Analyzed;
	}
}