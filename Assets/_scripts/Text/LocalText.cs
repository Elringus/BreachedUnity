using System.Collections.Generic;
using UnityEngine;

public class LocalText : IText
{
	public TextLanguage Language { get; set; }

	private const string QUESTS_TABLE_NAME = @"Breached ingame text - Quests";
	private const string JOURNAL_TABLE_NAME = @"Breached ingame text - Journal";
	private const string PHRASES_TABLE_NAME = @"Breached ingame text - Phrases";
	private const string ARTIFACTS_TABLE_NAME = @"Breached ingame text - Artifacts";
	private const string SCANINFO_TABLE_NAME = @"Breached ingame text - ScanInfo";
	private const string TOOLTIPS_TABLE_NAME = @"Breached ingame text - Tooltips";
	private const string SECTORS_TABLE_NAME = @"Breached ingame text - Sectors";

	private Dictionary<string, string> cachedText;

	public LocalText ()
	{
		cachedText = new Dictionary<string, string>();

		RetrieveTable(QUESTS_TABLE_NAME);
		RetrieveTable(JOURNAL_TABLE_NAME);
		RetrieveTable(PHRASES_TABLE_NAME);
		RetrieveTable(ARTIFACTS_TABLE_NAME);
		RetrieveTable(SCANINFO_TABLE_NAME);
		RetrieveTable(TOOLTIPS_TABLE_NAME);
		RetrieveTable(SECTORS_TABLE_NAME);
	}

	private void RetrieveTable (string tablePath)
	{
		var text = Resources.Load<TextAsset>("LocalText/" + tablePath).text;
		var rows = new List<string>(text.Split((char)10));

		foreach (var row in rows)
		{
			string key = row.Split(',')[0];
			if (cachedText.ContainsKey(key)) continue;
			string value = row
				.Replace(key + ',', string.Empty)
				.Replace("\"", string.Empty)
				.Replace("<br>", System.Environment.NewLine);
			cachedText.Add(key, value);
		}
	}

	public string Get (string term)
	{
		if (cachedText.ContainsKey(term)) return cachedText[term];
		else return string.Format("Missing {0} term in LocalText.", term);
	}
}