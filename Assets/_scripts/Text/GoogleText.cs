using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Net;
using System.IO;
using System;

public class GoogleText : IText
{
	public TextLanguage Language { get; set; }

	private const string QUESTS_TABLE_URL = @"https://docs.google.com/spreadsheets/d/1Lgw033KBgGhTew2hDKrcZR4VXxhqMtCh8f8QCOFbLJ8/export?format=csv&id=1Lgw033KBgGhTew2hDKrcZR4VXxhqMtCh8f8QCOFbLJ8&gid=0";
	private const string PHRASES_TABLE_URL = @"https://docs.google.com/spreadsheets/d/1Lgw033KBgGhTew2hDKrcZR4VXxhqMtCh8f8QCOFbLJ8/export?format=csv&id=1Lgw033KBgGhTew2hDKrcZR4VXxhqMtCh8f8QCOFbLJ8&gid=1387093746";

	private Dictionary<string, string> cachedText = new Dictionary<string, string>();
	private bool updateFailed;

	public GoogleText ()
	{
		Thread thread = new Thread(RetrieveData);
		thread.Start();
	}

	private void RetrieveData ()
	{
		try
		{
			SSLValidator.OverrideValidation();

			var request = (HttpWebRequest)WebRequest.Create(QUESTS_TABLE_URL);
			request.Method = WebRequestMethods.Http.Get;
			request.Timeout = 5000;
			var response = (HttpWebResponse)request.GetResponse();
			var reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
			List<string> rows = new List<string>(reader.ReadToEnd().Split((char)10));

			request = (HttpWebRequest)WebRequest.Create(PHRASES_TABLE_URL);
			request.Method = WebRequestMethods.Http.Get;
			request.Timeout = 5000;
			response = (HttpWebResponse)request.GetResponse();
			reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
			rows.AddRange(reader.ReadToEnd().Split((char)10));

			var result = new Dictionary<string, string>();
			foreach (var row in rows)
			{
				string key = row.Split(',')[0];
				if (result.ContainsKey(key)) continue;
				string value = row.Replace(key + ',', string.Empty).Replace("\"", string.Empty);
				result.Add(key, value);
			}

			cachedText = result;
			updateFailed = false;
		}
		catch (Exception e)
		{
			updateFailed = true;
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

		if (cachedText.ContainsKey(term)) return cachedText[term];
		else return string.Format("Null text for the {0} term.", term);
	}
}