
public class NullText : IText
{
	public TextLanguage Language { get; set; }

	public string GetText (string term)
	{
		if (term == "QuestNullStart")
			return @"
Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc ut tellus ut lacus adipiscing commodo ut eget tellus. Fusce at rutrum augue. Sed venenatis magna bibendum ornare scelerisque. In sagittis dolor et turpis sagittis, non pretium ante placerat. Aenean sit amet vulputate quam. Suspendisse in sagittis libero. 
";

		if (term == "QuestNullGoodDecision")
			return @"[action][ap]-1[/ap][mineralA]+25[/mineralA][/action]
Ut non erat vitae velit suscipit vulputate vitae sit amet sem. Aenean nulla nisl, volutpat non tortor non, interdum sollicitudin nibh. Vestibulum fermentum nulla mauris, a condimentum orci tempor in. Nulla dictum diam sapien, rutrum dignissim nulla feugiat non. Vestibulum aliquam cursus risus a vehicula.

Vestibulum varius volutpat ante, molestie ornare mi feugiat at. Pellentesque ac elit neque. Nullam augue ante, sagittis ut porta eget, interdum sit amet magna. Mauris sit amet vulputate nunc, et rhoncus dui. Praesent in metus non risus bibendum elementum. Aenean pharetra faucibus feugiat. Curabitur diam enim, consectetur ac bibendum convallis, sollicitudin congue leo. 
";

		if (term == "QuestNullBadDecision")
			return @"[action][ap]-10[/ap][/action]
In in blandit risus. Nam tempus justo ac est scelerisque, id venenatis ligula scelerisque. Fusce dui metus, lobortis vitae vestibulum at, malesuada vel nisi. Aenean aliquam elit ac justo eleifend adipiscing. Nullam ut erat erat. In sed commodo massa. Sed blandit sollicitudin lorem non accumsan. Etiam sodales vestibulum quam, eu fringilla purus eleifend et.
";

		if (term == "QuestNullGoodDecisionEnd")
			return @"
Vestibulum varius volutpat ante, molestie ornare mi feugiat at. Pellentesque ac elit neque. Nullam augue ante, sagittis ut porta eget, interdum sit amet magna. Mauris sit amet vulputate nunc, et rhoncus dui. Praesent in metus non risus bibendum elementum. Aenean pharetra faucibus feugiat. Curabitur diam enim, consectetur ac bibendum convallis, sollicitudin congue leo. 

In in blandit risus. Nam tempus justo ac est scelerisque, id venenatis ligula scelerisque. Fusce dui metus, lobortis vitae vestibulum at, malesuada vel nisi. accumsan. 
";

		return string.Format("Null text for the {0} term.", term);
	}
}