using System;
using System.Collections.Generic;

public class Requirements
{
	private int _minAP;
	public int MinAP
	{
		get { return _minAP; }
		set { _minAP = value; ServiceLocator.State.Save(); }
	}

	private int _minDay;
	public int MinDay
	{
		get { return _minDay; }
		set { _minDay = value; ServiceLocator.State.Save(); }
	}

	private int _day;
	public int Day
	{
		get { return _day; }
		set { _day = value; ServiceLocator.State.Save(); }
	}

	private int _maxDay;
	public int MaxDay
	{
		get { return _maxDay; }
		set { _maxDay = value; ServiceLocator.State.Save(); }
	}

	private List<string> _completedQuests;
	public List<string> CompletedQuests
	{
		get { return _completedQuests; }
		set { _completedQuests = value; ServiceLocator.State.Save(); }
	}

	private List<string> _analyzedArtifacts;
	public List<string> AnalyzedArtifacts
	{
		get { return _analyzedArtifacts; }
		set { _analyzedArtifacts = value; ServiceLocator.State.Save(); }
	}

	[Obsolete("For XML serialization only.", true)]
	public Requirements ()
	{

	}

	public Requirements (int minAP = 0, int minDay = 0, int day = 0, int maxDay = 0, List<string> completedQuests = null, List<string> analyzedArtifacts = null)
	{
		this.MinAP = minAP;
		this.MinDay = minDay;
		this.Day = day;
		this.MaxDay = maxDay;
		this.CompletedQuests = completedQuests ?? new List<string>();
		this.AnalyzedArtifacts = analyzedArtifacts ?? new List<string>();
	}

	public bool Check ()
	{
		IState state = ServiceLocator.State;

		if (MinAP < 0 || MinAP != 0 && MinAP > state.CurrentAP) return false;
		if (MinDay != 0 && MinDay > state.CurrentDay) return false;
		if (Day != 0 && Day != state.CurrentDay) return false;
		if (MaxDay != 0 && MaxDay < state.CurrentDay) return false;
		if (CompletedQuests != null)
			foreach (var qBlock in CompletedQuests)
				if (qBlock != string.Empty && state.QuestRecords.Find(q => q.CurrentBlock == qBlock) == null) return false;
		if (AnalyzedArtifacts != null)
			foreach (string artifactID in AnalyzedArtifacts)
				if (artifactID != string.Empty && state.Artifacts.Find(artifact => artifact.ID == artifactID).Status != ArtifactStatus.Analyzed) return false;

		return true;
	}
}