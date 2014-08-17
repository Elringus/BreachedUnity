using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;

public class QuestController : BaseController
{
	public bool CheckQuest (Quest quest)
	{
		if (quest.Status != QuestStatus.NotStarted) return false;
		if (quest.RequireAP > State.CurrentAP) return false;
		if (quest.RequireDay > State.CurrentDay) return false;
		if (quest.RequireQuest != string.Empty &&
			State.QuestRecords.Find(q => q.CurrentBlock == quest.RequireQuest) == null) return false;

		return true;
	}

	public bool StartQuest (Quest quest)
	{
		if (GetCurrentQuest() != null || !CheckQuest(quest)) return false;

		quest.Status = QuestStatus.Started;
		return true;
	}

	public bool MakeChoise (string choiseText)
	{
		XDocument block = XDocument.Parse(Text.Get(GetCurrentQuest().CurrentBlock));

		XElement choise = new XElement("temp");
		foreach (var c in block.Root.Elements("choise"))
			if (c.Value == choiseText)
			{
				choise = c;
				break;
			}

		if (choise.Attribute("ap") != null)
			State.CurrentAP += int.Parse(choise.Attribute("ap").Value);

		if (choise.Attribute("mineralA") != null)
			State.MineralA += int.Parse(choise.Attribute("mineralA").Value);
		if (choise.Attribute("mineralB") != null)
			State.MineralB += int.Parse(choise.Attribute("mineralB").Value);
		if (choise.Attribute("mineralC") != null)
			State.MineralC += int.Parse(choise.Attribute("mineralC").Value);

		if (choise.Attribute("wiring") != null)
			State.Wiring += int.Parse(choise.Attribute("wiring").Value);
		if (choise.Attribute("alloy") != null)
			State.Alloy += int.Parse(choise.Attribute("alloy").Value);
		if (choise.Attribute("chips") != null)
			State.Chips += int.Parse(choise.Attribute("chips").Value);

		if (choise.Attribute("artifact") != null)
		{
			var artifact = State.Artifacts.Find(a => a.Name == choise.Attribute("artifact").Value);
			if (artifact.Status == ArtifactStatus.NotFound) artifact.Status = ArtifactStatus.Found;
		}

		if (choise.Attribute("to").Value == "END") EndQuest();
		else
		{
			GetCurrentQuest().CurrentBlock = choise.Attribute("to").Value;
			State.Save(); // very sad :(
		}

		return true;
	}

	public void EndQuest ()
	{
		GetCurrentQuest().Status = QuestStatus.Completed;
	}

	public Quest GetCurrentQuest ()
	{
		return State.QuestRecords.Find(q => q.Status == QuestStatus.Started);
	}
}