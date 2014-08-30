
public class NullText : IText
{
	public TextLanguage Language { get; set; }

	public string Get (string term)
	{
		return string.Format("Null text for the {0} term.", term);
	}
}