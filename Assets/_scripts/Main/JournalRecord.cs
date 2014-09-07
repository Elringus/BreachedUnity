using System;

public class JournalRecord
{
	private string _id;
	public string ID
	{
		get { return _id; }
		set { _id = value; ServiceLocator.State.Save(); }
	}

	private int _assignedDay;
	public int AssignedDay
	{
		get { return _assignedDay; }
		set { _assignedDay = value; ServiceLocator.State.Save(); }
	}

	private Requirements _requirements;
	public Requirements Requirements
	{
		get { return _requirements; }
		set { _requirements = value; ServiceLocator.State.Save(); }
	}

	[Obsolete("For XML serialization only.", true)]
	public JournalRecord ()
	{

	}

	public JournalRecord (string id, Requirements requirements = null)
	{
		this.ID = id;
		this.Requirements = requirements ?? new Requirements(-1);
		this.AssignedDay = 0;
	}

	public bool Check ()
	{
		return AssignedDay == 0 && Requirements.Check();
	}
}