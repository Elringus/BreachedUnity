using System.Collections.Generic;

public interface IText
{
	#region QUEST
	// List[x], x = quest block number
	// string[y], y(0) = text block, y(1..n) = choises text 
	List<string[]> QuestOutside { get; }
	List<string[]> QuestDalia { get; }
	List<string[]> QuestEcho { get; }
	#endregion
}