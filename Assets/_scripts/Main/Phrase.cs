using System;

public class Phrase
{
	private string _name;
	public string Name
	{
		get { return _name; }
		set { _name = value; ServiceLocator.State.Save(); }
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

	public Phrase (string name, Requirements requirements = null)
	{
		this.Name = name;
		this.Requirements = requirements ?? new Requirements(-1);
	}

	public bool Check ()
	{
		return Requirements.Check();
	}
}