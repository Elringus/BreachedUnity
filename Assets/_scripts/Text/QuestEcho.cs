using System.Collections.Generic;

public class QuestEcho : BaseQuest
{
	// no way...
	public QuestEcho ()
		: base(new List<TextBlock>
		{
			new TextBlock(Text.QuestDalia[0][0], new Dictionary<string,TextBlock> { 
			{ Text.QuestDalia[0][1], new TextBlock(Text.QuestDalia[3][0]) }, 
			{ Text.QuestDalia[0][2], new TextBlock(Text.QuestDalia[6][0]) } }),
			new TextBlock(Text.QuestDalia[1][0], new Dictionary<string,TextBlock> { { Text.QuestDalia[1][1], new TextBlock(Text.QuestDalia[4][0]) } }),
			new TextBlock(Text.QuestDalia[2][0], new Dictionary<string,TextBlock> { { Text.QuestDalia[2][1], new TextBlock(Text.QuestDalia[5][0]) } }),
		})
	{

	}
}