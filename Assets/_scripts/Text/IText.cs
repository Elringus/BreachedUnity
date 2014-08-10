
public interface IText
{
	TextLanguage Language { get; set; }
	string GetText (string term);
}