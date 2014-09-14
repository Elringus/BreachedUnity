using System.Collections.Generic;
using System.Threading;
using System.Linq;
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

	private static float downloadProgress;

	private Dictionary<string, string> cachedText = new Dictionary<string, string>();

	public GoogleText ()
	{
		Thread thread = new Thread(RetrieveData);
		thread.Start();
	}

	private void RetrieveData ()
	{
		downloadProgress = 0;

		try
		{
			SSLValidator.OverrideValidation();

			var request = (HttpWebRequest)WebRequest.Create(QUESTS_TABLE_URL);
			request.Method = WebRequestMethods.Http.Get;
			request.Timeout = REQUEST_TIMEOUT;
			var response = (HttpWebResponse)request.GetResponse();
			var reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
			List<string> rows = new List<string>(reader.ReadToEnd().Split((char)10));

			downloadProgress = .2f;

			request = (HttpWebRequest)WebRequest.Create(JOURNAL_TABLE_URL);
			response = (HttpWebResponse)request.GetResponse();
			reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
			rows.AddRange(reader.ReadToEnd().Split((char)10));

			downloadProgress = .4f;

			request = (HttpWebRequest)WebRequest.Create(PHRASES_TABLE_URL);
			response = (HttpWebResponse)request.GetResponse();
			reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
			rows.AddRange(reader.ReadToEnd().Split((char)10));

			downloadProgress = .6f;

			request = (HttpWebRequest)WebRequest.Create(ARTIFACTS_TABLE_URL);
			response = (HttpWebResponse)request.GetResponse();
			reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
			rows.AddRange(reader.ReadToEnd().Split((char)10));

			downloadProgress = .8f;

			request = (HttpWebRequest)WebRequest.Create(SCANINFO_TABLE_URL);
			response = (HttpWebResponse)request.GetResponse();
			reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
			rows.AddRange(reader.ReadToEnd().Split((char)10));

			downloadProgress = 1;

			var result = new Dictionary<string, string>();
			foreach (var row in rows)
			{
				string key = row.Split(',')[0];
				if (result.ContainsKey(key)) continue;
				string value = row
					.Replace(key + ',', string.Empty)
					.Replace("\"", string.Empty)
					.Replace("<br>", System.Environment.NewLine);
				result.Add(key, value);
			}

			cachedText = result;
		}
		catch (Exception e)
		{
			ServiceLocator.Logger.Log(e.Message);
			Thread.Sleep(1000);
			RetrieveData();
			return;
		}

		Events.RaiseTextUpdated();

		Thread.CurrentThread.Abort();
	}

	public string Get (string term)
	{
		if (term == "STATE") return cachedText.Count > 0 ? "OK" : "NONE";
		if (term == "PROGRESS") return downloadProgress.ToString();

		if (cachedText.ContainsKey(term)) return cachedText[term];
		else return string.Format("Null text for the {0} term.", term);
	}
}