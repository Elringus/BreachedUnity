using System;

public class SectorParameters
{
	private int _sectorID;
	public int SectorID
	{
		get { return _sectorID; }
		set { _sectorID = value; ServiceLocator.State.Save(); }
	}

	private int _lootSpotCount;
	public int LootSpotCount
	{
		get { return _lootSpotCount; }
		set { _lootSpotCount = value; ServiceLocator.State.Save(); }
	}

	private int _mineralA;
	public int MineralA
	{
		get { return _mineralA; }
		set { _mineralA = value; ServiceLocator.State.Save(); }
	}

	private int _mineralB;
	public int MineralB
	{
		get { return _mineralB; }
		set { _mineralB = value; ServiceLocator.State.Save(); }
	}

	private int _mineralC;
	public int MineralC
	{
		get { return _mineralC; }
		set { _mineralC = value; ServiceLocator.State.Save(); }
	}

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
