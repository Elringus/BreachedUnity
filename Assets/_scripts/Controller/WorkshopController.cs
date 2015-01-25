using System;
using System.Linq;

public class WorkshopController : BaseController
{
	public static bool AnalyzeArtifact (Artifact artifact)
	{
		if (State.CurrentAP < State.AnalyzeArtifactAPCost) return false;

		State.CurrentAP -= State.AnalyzeArtifactAPCost;
		//artifact.Status = ArtifactStatus.Analyzing;
		artifact.Status = ArtifactStatus.Analyzed;
		State.Wiring += artifact.Wiring;
		State.Alloy += artifact.Alloy;
		State.Chips += artifact.Chips;
		return true;
	}

	public static bool FixEngine ()
	{
		if (!CanFixEngine()) return false;

		State.EngineFixed = true;

		State.CurrentAP -= State.FixEngineAPCost;
		return true;
	}

	public static bool CanFixEngine ()
	{
		if (State.CurrentAP < State.FixEngineAPCost) return false;

		var requirements = State.FixEngineRequirements[State.BreakageType];
		return !State.EngineFixed && 
			State.Wiring >= requirements[0] && State.Alloy >= requirements[1] && State.Chips >= requirements[2] && 
			IdenticalArtifactAnalyzed();
	}

	public static bool IdenticalArtifactAnalyzed ()
	{
		return State.Artifacts.Find(x => x.Identity.GetValueOrDefault() == State.BreakageType).Status == ArtifactStatus.Analyzed;
	}

	public static bool SynthFuel (int[] probe)
	{
		if (!CanSynthFuel(probe)) return false;

		State.MineralA -= probe[0];
		State.MineralB -= probe[1];
		State.MineralC -= probe[2];

		State.FuelSynthProbes.Add(probe);

		if (MeasureProbe(probe) == ProbeType.True)
			State.FuelSynthed = true;

		State.CurrentAP -= State.FuelSynthAPCost;
		return true;
	}

	public static bool CanSynthFuel (int[] probe) 
	{
		return !State.FuelSynthed && State.CurrentAP >= State.FuelSynthAPCost 
			&& !State.FuelSynthProbes.Exists(p => p[0] == probe[0] && p[1] == probe[1] && p[2] == probe[2])
			&& State.MineralA >= probe[0] && State.MineralB >= probe[1] && State.MineralC >= probe[2] 
			&& probe[0] > 0 && probe[1] > 0 && probe[2] > 0 
			&& probe[0] + probe[1] + probe[2] == State.FuelSynthSumm;
	}

	public static ProbeType MeasureProbe (int[] probe)
	{
		int distance = Math.Abs(probe[0] - State.FuelSynthFormula[0]) +
			Math.Abs(probe[1] - State.FuelSynthFormula[1]) +
			Math.Abs(probe[2] - State.FuelSynthFormula[2]);

		return distance <= State.FuelSynthGrace * State.FuelSynthSumm / 3 ? ProbeType.True :
			distance <= State.FuelSynthSumm / 1.5f ? ProbeType.Close : ProbeType.Wrong;
	}
}