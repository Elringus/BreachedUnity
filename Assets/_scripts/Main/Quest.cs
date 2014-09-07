using System;

public class Quest
{
	private string _id;
	public string ID
	{
		get { return _id; }
		set { _id = value; ServiceLocator.State.Save(); }
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

	private Requirements _requirements;
	public Requirements Requirements
	{
		get { return _requirements; }
		set { _requirements = value; ServiceLocator.State.Save(); }
	}

	[Obsolete("For XML serialization only.", true)]
	public Quest ()
	{
		
	}

	public Quest (string id, Requirements requirements = null)
	{
		this.ID = id;
		this.Status = QuestStatus.NotStarted;
		this.CurrentBlock = string.Format("Quest{0}#1", id);
		this.Requirements = requirements ?? new Requirements(-1);
	}

	public bool Check ()
	{
		return Requirements.Check() && Status == QuestStatus.NotStarted;
	}

	public void ResetProgress ()
	{
		Status = QuestStatus.NotStarted;
		CurrentBlock = string.Format("Quest{0}#1", ID);
	}
}