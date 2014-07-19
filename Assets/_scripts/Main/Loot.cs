
public struct Loot
{
	public LootType LootType { get; set; }

	public int MineralA { get; set; }
	public int MineralB { get; set; }
	public int MineralC { get; set; }

	public void AddMineralsByType (LootType mineralType, int amount)
	{
		switch (mineralType)
		{
			case LootType.MineralA:
				this.MineralA += amount;
				break;
			case LootType.MineralB:
				this.MineralB += amount;
				break;
			case LootType.MineralC:
				this.MineralC += amount;
				break;
		}
	}
}