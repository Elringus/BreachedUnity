using System;

public class Phrase
{
	private string _id;
	public string ID
	{
		get { return _id; }
		set { _id = value; ServiceLocator.State.Save(); }
	}

	private Requirements _requirements;
	public Requirements Requirements
	{
		get { return _requirements; }
		set { _requirements = value; ServiceLocator.State.Save(); }
	}

	private string _associatedQuest;
	public string AssociatedQuest
	{
		get { return _associatedQuest; }
		set { _associatedQuest = value; ServiceLocator.State.Save(); }
	}

	[Obsolete("For XML serialization only.", true)]
	public Phrase ()
	{
		
	}

	public Phrase (string id, Requirements requirements = null, string associatedQuest = "")
	{
		this.ID = id;
		this.Requirements = requirements ?? new Requirements(-1);
		this.AssociatedQuest = associatedQuest;
	}

	public bool Check ()
	{
		return !(AssociatedQuest != string.Empty && ServiceLocator.State.QuestRecords.Find(q => q.ID == AssociatedQuest).Status != QuestStatus.NotStarted) && Requirements.Check();
	}

	public void StartAssociatedQuest ()
	{
		QuestController.StartQuest(ServiceLocator.State.QuestRecords.Find(q => q.ID == AssociatedQuest), true);
	}
}