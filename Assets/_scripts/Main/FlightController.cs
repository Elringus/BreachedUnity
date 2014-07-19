using System;

public class FlightController : BaseController
{
	public Loot GenerateLoot (SectorParameters sectorParameters)
	{
		var loot = new Loot();

		while (true)
		{
			LootType[] values = (LootType[])Enum.GetValues(typeof(LootType));
			loot.LootType = values[Rand.RND.Next(0, 3)];

			int mineralCount = sectorParameters.GetMineralByType(loot.LootType);
			if (mineralCount != 0)
			{
				loot.AddMineralsByType(loot.LootType, mineralCount);
				break;
			}
		}

		return loot;
	}

	public void RecieveLoot (Loot loot)
	{
		State.MineralA += loot.MineralA;
		State.MineralB += loot.MineralB;
		State.MineralC += loot.MineralC;
	}
}