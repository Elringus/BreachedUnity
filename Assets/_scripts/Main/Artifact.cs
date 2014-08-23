using System;

public class Artifact
{
	private string _name;
	public string Name
	{
		get { return _name; }
		set { _name = value; ServiceLocator.State.Save(); }
	}

	private string _infotrace;
	public string Infotrace
	{
		get { return _infotrace; }
		set { _infotrace = value; ServiceLocator.State.Save(); }
	}

	private BreakageType? _identity;
	public BreakageType? Identity
	{
		get { return _identity; }
		set { _identity = value; ServiceLocator.State.Save(); }
	}

	private int _wiring;
	public int Wiring
	{
		get { return _wiring; }
		set { _wiring = value; ServiceLocator.State.Save(); }
	}

	private int _alloy;
	public int Alloy
	{
		get { return _alloy; }
		set { _alloy = value; ServiceLocator.State.Save(); }
	}

	private int _chips;
	public int Chips
	{
		get { return _chips; }
		set { _chips = value; ServiceLocator.State.Save(); }
	}

	private ArtifactStatus _status;
	public ArtifactStatus Status
	{
		get { return _status; }
		set { _status = value; ServiceLocator.State.Save(); }
	}

	[Obsolete("For XML serialization only.", true)]
	public Artifact ()
	{

	}

	public Artifact (string name, string Infotrace, BreakageType? identity, int wiring, int alloy, int chips)
	{
		this.Name = name;
		this.Infotrace = Infotrace;
		this.Identity = identity;
		this.Wiring = wiring;
		this.Alloy = alloy;
		this.Chips = chips;
	}
}