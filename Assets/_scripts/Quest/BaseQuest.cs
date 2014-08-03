using System.Collections.Generic;

public abstract class BaseQuest
{
	public TextBlock CurrentBlock;

	protected static IText Text;
	protected List<TextBlock> TextBlocks;

	static BaseQuest ()
	{
		Text = ServiceLocator.Text;
	}

	public BaseQuest (List<TextBlock> textBlocks)
	{
		this.TextBlocks = textBlocks;
	}
}