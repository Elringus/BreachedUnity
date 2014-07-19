using System;

public class SectorParameters
{
	public int SectorID { get; set; }

	public int LootSpotCount { get; set; }

	public int MineralA { get; set; }
	public int MineralB { get; set; }
	public int MineralC { get; set; }

	[Obsolete("For XML serialization only.", true)]
	public SectorParameters () 
	{

	}

	public SectorParameters (int sectorID, int lootSpotCount, int mineralA, int mineralB, int mineralC)
	{
		this.SectorID = sectorID;

		this.LootSpotCount = lootSpotCount;

		this.MineralA = mineralA;
		this.MineralB = mineralB;
		this.MineralC = mineralC;
	}

	public int GetMineralByType (LootType lootType)
	{
		switch (lootType)
		{
			case LootType.MineralA: return MineralA;
			case LootType.MineralB: return MineralB;
			case LootType.MineralC: return MineralC;
			default: return 0;
		}
	}
}
