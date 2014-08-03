using System.Collections.Generic;

public class TextBlock
{
	public string Text;
	public Dictionary<string, TextBlock> Choises;

	public TextBlock (string text, Dictionary<string, TextBlock> choises = null)
	{
		this.Text = text;
		this.Choises = choises;
	}
}