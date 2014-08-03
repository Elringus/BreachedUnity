using System.Collections.Generic;

public class NullText : IText
{
	public List<string[]> QuestOutside { get { return new List<string[]> { }; } }
	public List<string[]> QuestDalia { get { return new List<string[]> { }; } }
	public List<string[]> QuestEcho { get { return new List<string[]> { }; } }
}