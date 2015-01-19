using System.Linq;
using System.Collections.Generic;

public class HorizonController : BaseController
{
	public static List<Phrase> GetPhrases ()
	{
		return State.Phrases.FindAll(p => p.Check());
	}
}