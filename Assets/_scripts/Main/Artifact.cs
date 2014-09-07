using System;
using System.Xml.Serialization;

public class Artifact
{
	private string _id;
	public string ID
	{
		get { return _id; }
		set { _id = value; ServiceLocator.State.Save(); }
	}

	[XmlIgnore]
	public string Name
	{
		get { return ServiceLocator.Text.Get(ID); }
	}

	[XmlIgnore]
	public string Infotrace
	{
		get { return ServiceLocator.Text.Get(ID + "Infotrace"); }
	}

	private int _sector;
	public int Sector
	{
		get { return _sector; }
		set { _sector = value; ServiceLocator.State.Save(); }
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

	public Artifact (string id, int sector, BreakageType? identity, int wiring, int alloy, int chips)
	{
		this.ID = id;
		this.Sector = sector;
		this.Identity = identity;
		this.Wiring = wiring;
		this.Alloy = alloy;
		this.Chips = chips;
	}
}