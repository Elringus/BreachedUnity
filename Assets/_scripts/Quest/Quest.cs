using System;

public class Quest
{
	public string Name { get; set; }
	public QuestStatus Status;
	public string CurrentBlock { get; set; }

	public int RequireAP { get; set; }
	public int RequireDay { get; set; }
	public string RequireQuest { get; set; }

	[Obsolete("For XML serialization only.", true)]
	public Quest ()
	{

	}

	public Quest (string name, int requireAP = 0, int requireDay = 0, string requireQuest = "")
	{
		this.Name = name;
		this.Status = QuestStatus.NotStarted;
		this.CurrentBlock = string.Format("Quest{0}#1", name);

		this.RequireAP = requireAP;
		this.RequireDay = requireDay;
		this.RequireQuest = requireQuest;
	}

	public void ResetProgress ()
	{
		Status = QuestStatus.NotStarted;
		CurrentBlock = string.Format("Quest{0}#1", Name);
	}
}