using System;
using System.Collections.Generic;

public class FlightController : BaseController
{
	public void GenerateLoot (int sectorID, out List<Loot> lootList)
	{
		var sectorParameters = State.SectorsParameters.Find(x => x.SectorID == sectorID);
		lootList = new List<Loot>();
		List<Artifact> chosenArtifacts = new List<Artifact>();
		int artefactsLeft = State.Artifacts.FindAll(x => x.ArtifactStatus == ArtifactStatus.NotFound).Count;

		for (int i = 0; i < sectorParameters.LootSpotCount; i++)
		{
			var loot = new Loot();

			// static artifact spawn rate for now
			if (Rand.RND.Next(0, 4) == 0 && artefactsLeft > 0)
			{
				loot.LootType = LootType.Artefact;
				var newArtifacts = State.Artifacts.FindAll(x => 
					x.ArtifactStatus == ArtifactStatus.NotFound && !chosenArtifacts.Contains(x));
				loot.Artifact = newArtifacts[Rand.RND.Next(0, newArtifacts.Count - 1)];
				chosenArtifacts.Add(loot.Artifact);
				artefactsLeft--;
			}
			else
			{
				// 33% probability for each not-zero mineral
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
			}

			lootList.Add(loot);
		}
	}

	public void RecieveLoot (Loot loot)
	{
		switch (loot.LootType)
		{
			case LootType.Artefact:
				loot.Artifact.ArtifactStatus = ArtifactStatus.Found;
				break;
			case LootType.MineralA:
				State.MineralA += loot.MineralA;
				break;
			case LootType.MineralB:
				State.MineralB += loot.MineralB;
				break;
			case LootType.MineralC:
				State.MineralC += loot.MineralC;
				break;
		}
	}
}