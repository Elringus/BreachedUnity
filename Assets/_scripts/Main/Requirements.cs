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

	[Obsolete("For XML serialization only.", true)]
	public Requirements ()
	{

	}

	public Requirements (int minAP = 0, int minDay = 0, int day = 0, int maxDay = 0, List<string> completedQuests = null)
	{
		this.MinAP = minAP;
		this.MinDay = minDay;
		this.Day = day;
		this.MaxDay = maxDay;
		this.CompletedQuests = completedQuests ?? new List<string>();
	}

	public bool Check ()
	{
		IState state = ServiceLocator.State;

		if (MinAP < 0 || MinDay != 0 && MinAP > state.CurrentAP) return false;
		if (MinDay != 0 && MinDay > state.CurrentDay) return false;
		if (Day != 0 && Day != state.CurrentDay) return false;
		if (MaxDay != 0 && MaxDay < state.CurrentDay) return false;
		if (CompletedQuests != null)
			foreach (var qBlock in CompletedQuests)
				if (qBlock != string.Empty && state.QuestRecords.Find(q => q.CurrentBlock == qBlock) == null) return false;

		return true;
	}
}