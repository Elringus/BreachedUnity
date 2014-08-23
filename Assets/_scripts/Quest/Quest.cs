using System;

public class Quest
{
	private string _name;
	public string Name
	{
		get { return _name; }
		set { _name = value; ServiceLocator.State.Save(); }
	}

	private QuestStatus _status;
	public QuestStatus Status
	{
		get { return _status; }
		set { _status = value; ServiceLocator.State.Save(); }
	}

	private string _currentBlock;
	public string CurrentBlock
	{
		get { return _currentBlock; }
		set { _currentBlock = value; ServiceLocator.State.Save(); }
	}

	private int _requireAP;
	public int RequireAP
	{
		get { return _requireAP; }
		set { _requireAP = value; ServiceLocator.State.Save(); }
	}

	private int _requireDay;
	public int RequireDay
	{
		get { return _requireDay; }
		set { _requireDay = value; ServiceLocator.State.Save(); }
	}

	private string _requireQuest;
	public string RequireQuest
	{
		get { return _requireQuest; }
		set { _requireQuest = value; ServiceLocator.State.Save(); }
	}

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