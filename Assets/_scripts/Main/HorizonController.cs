using System.Linq;
using System.Collections.Generic;

public class HorizonController : BaseController
{
	public List<Phrase> GetPhrases ()
	{
		return State.PhraseRecords.FindAll(p => p.Check());
	}
}