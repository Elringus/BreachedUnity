using System;

public class SectorParameters
{
	public int SectorID { get; set; }

	public int MineralA { get; set; }
	public int MineralB { get; set; }
	public int MineralC { get; set; }

	[Obsolete("For XML serialization only.", true)]
	public SectorParameters () 
	{

	}

	public SectorParameters (int sectorID, int mineralA, int mineralB, int mineralC)
	{
		this.SectorID = sectorID;

		this.MineralA = mineralA;
		this.MineralB = mineralB;
		this.MineralC = mineralC;
	}
}
