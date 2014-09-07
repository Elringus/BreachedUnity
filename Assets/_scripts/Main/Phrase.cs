using System;

public class Phrase
{
	private string _id;
	public string ID
	{
		get { return _id; }
		set { _id = value; ServiceLocator.State.Save(); }
	}

	private Requirements _requirements;
	public Requirements Requirements
	{
		get { return _requirements; }
		set { _requirements = value; ServiceLocator.State.Save(); }
	}

	[Obsolete("For XML serialization only.", true)]
	public Phrase ()
	{
		
	}

	public Phrase (string id, Requirements requirements = null)
	{
		this.ID = id;
		this.Requirements = requirements ?? new Requirements(-1);
	}

	public bool Check ()
	{
		return Requirements.Check();
	}
}