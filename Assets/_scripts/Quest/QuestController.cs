using System.Xml.Linq;

public class QuestController : BaseController
{
	public bool CheckQuest (string questName)
	{
		XDocument conditions = XDocument.Parse(Text.Get(string.Format("Quest{0}#0", questName)));

		if (conditions.Root.Name != "conditions" || State.QuestRecords[questName] != string.Empty) 
			return false;

		if (conditions.Root.Element("ap") != null && State.CurrentAP < int.Parse(conditions.Root.Element("ap").Value))
			return false;

		if (conditions.Root.Element("day") != null && State.CurrentDay < int.Parse(conditions.Root.Element("day").Value))
			return false;

		if (conditions.Root.Element("quest") != null &&
			State.QuestRecords[conditions.Element("quest").Attribute("name").Value] != conditions.Root.Element("quest").Value)
			return false;

		return true;
	}

	public bool StartQuest (string questName)
	{
		if (!CheckQuest(questName)) return false;

		State.CurrentQuest = questName;
		State.QuestRecords[questName] = string.Format("Quest{0}#1", questName);
		return true;
	}

	public bool MakeChoise (string choiseText)
	{
		XDocument block = XDocument.Parse(Text.Get(State.QuestRecords[State.CurrentQuest]));

		XElement choise = new XElement("");
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
			Artifact artifact = State.Artifacts.Find(a => a.Name == choise.Attribute("artifact").Value);
			if (artifact.Status == ArtifactStatus.NotFound) artifact.Status = ArtifactStatus.Found;
		}

		if (choise.Attribute("to").Value == "END") EndQuest();
		else State.QuestRecords[State.CurrentQuest] = choise.Attribute("to").Value;

		return true;
	}

	public void EndQuest ()
	{
		State.CurrentQuest = string.Empty;
	}
}