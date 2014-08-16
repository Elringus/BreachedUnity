
public interface IText
{
	TextLanguage Language { get; set; }
	string Get (string term);
}