using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.IO;
using System;

public class GoogleText : IText
{
	public TextLanguage Language { get; set; }

	private const int REQUEST_TIMEOUT = 5000;

	private const string QUESTS_TABLE_URL = @"https://docs.google.com/spreadsheets/d/1DztvoxCDKT2abEwqtxuRj2oQZNbtPAnYOwrj7tHvphk/export?format=csv&id=1DztvoxCDKT2abEwqtxuRj2oQZNbtPAnYOwrj7tHvphk&gid=0";
	private const string JOURNAL_TABLE_URL = @"https://docs.google.com/spreadsheets/d/1DztvoxCDKT2abEwqtxuRj2oQZNbtPAnYOwrj7tHvphk/export?format=csv&id=1DztvoxCDKT2abEwqtxuRj2oQZNbtPAnYOwrj7tHvphk&gid=291239901";
	private const string PHRASES_TABLE_URL = @"https://docs.google.com/spreadsheets/d/1DztvoxCDKT2abEwqtxuRj2oQZNbtPAnYOwrj7tHvphk/export?format=csv&id=1DztvoxCDKT2abEwqtxuRj2oQZNbtPAnYOwrj7tHvphk&gid=1387093746";
	private const string ARTIFACTS_TABLE_URL = @"https://docs.google.com/spreadsheets/d/1DztvoxCDKT2abEwqtxuRj2oQZNbtPAnYOwrj7tHvphk/export?format=csv&id=1DztvoxCDKT2abEwqtxuRj2oQZNbtPAnYOwrj7tHvphk&gid=222661492";
	private const string SCANINFO_TABLE_URL = @"https://docs.google.com/spreadsheets/d/1DztvoxCDKT2abEwqtxuRj2oQZNbtPAnYOwrj7tHvphk/export?format=csv&id=1DztvoxCDKT2abEwqtxuRj2oQZNbtPAnYOwrj7tHvphk&gid=1514799521";
	private const string TOOLTIPS_TABLE_URL = @"https://docs.google.com/spreadsheets/d/1DztvoxCDKT2abEwqtxuRj2oQZNbtPAnYOwrj7tHvphk/export?format=csv&id=1DztvoxCDKT2abEwqtxuRj2oQZNbtPAnYOwrj7tHvphk&gid=469296090";
	private const string SECTORS_TABLE_URL = @"https://docs.google.com/spreadsheets/d/1DztvoxCDKT2abEwqtxuRj2oQZNbtPAnYOwrj7tHvphk/export?format=csv&id=1DztvoxCDKT2abEwqtxuRj2oQZNbtPAnYOwrj7tHvphk&gid=2114384986";

	private static float downloadProgress;

	private Dictionary<string, string> cachedText;

	public GoogleText ()
	{
		cachedText = new Dictionary<string, string>();
		downloadProgress = 0;

		new Thread(() => { RetrieveTable(QUESTS_TABLE_URL); }).Start();
		new Thread(() => { RetrieveTable(JOURNAL_TABLE_URL); }).Start();
		new Thread(() => { RetrieveTable(PHRASES_TABLE_URL); }).Start();
		new Thread(() => { RetrieveTable(ARTIFACTS_TABLE_URL); }).Start();
		new Thread(() => { RetrieveTable(SCANINFO_TABLE_URL); }).Start();
		new Thread(() => { RetrieveTable(TOOLTIPS_TABLE_URL); }).Start();
		new Thread(() => { RetrieveTable(SECTORS_TABLE_URL); }).Start();
	}

	private void RetrieveTable (string tableURL)
	{
		try
		{
			SSLValidator.OverrideValidation();

			var request = (HttpWebRequest)WebRequest.Create(tableURL);
			request.Method = WebRequestMethods.Http.Get;
			request.Timeout = REQUEST_TIMEOUT;
			var response = (HttpWebResponse)request.GetResponse();
			var reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
			var rows = new List<string>(reader.ReadToEnd().Split((char)10));

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
		catch (Exception e)
		{
			ServiceLocator.Logger.Log(e.Message);
			Thread.Sleep(500);
			RetrieveTable(tableURL);
			return;
		}

		downloadProgress += .144f;
		if (downloadProgress > 1)
		{
			downloadProgress = 1;
			Events.RaiseTextUpdated();
		}
	}

	public string Get (string term)
	{
		if (term == "STATE") return cachedText.Count > 0 ? "OK" : "NONE";
		if (term == "PROGRESS") return downloadProgress.ToString();

		if (cachedText.ContainsKey(term)) return cachedText[term];
		else return string.Format("Missing {0} term in GoogleText.", term);
	}
}