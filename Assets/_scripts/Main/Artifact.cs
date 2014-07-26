using System;

public class Artifact
{
	public string Name { get; set; }
	public string Infotrace { get; set; }
	public BreakageType? Identity { get; set; }
	public int Wiring { get; set; }
	public int Alloy { get; set; }
	public int Chips { get; set; }
	public ArtifactStatus Status { get; set; }

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