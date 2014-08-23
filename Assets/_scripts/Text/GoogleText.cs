using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Net;
using System.IO;
using System;

public class GoogleText : IText
{
	public TextLanguage Language { get; set; }

	private const string TABLE_URL = @"https://docs.google.com/spreadsheets/d/1Lgw033KBgGhTew2hDKrcZR4VXxhqMtCh8f8QCOFbLJ8/export?format=csv&id=1Lgw033KBgGhTew2hDKrcZR4VXxhqMtCh8f8QCOFbLJ8&gid=0";
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
			var request = (HttpWebRequest)WebRequest.Create(TABLE_URL);
			request.Method = WebRequestMethods.Http.Get;
			request.Timeout = 5000;
			var response = (HttpWebResponse)request.GetResponse();
			var reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);

			var result = new Dictionary<string, string>();
			string[] rows = reader.ReadToEnd().Split((char)10);
			foreach (var row in rows)
			{
				string key = row.Split(',')[0];
				if (result.ContainsKey(key)) continue;
				string value = row.Replace(key + ',', string.Empty).Replace("\"", string.Empty);
				result.Add(key, value);
			}

			cachedText = result;
		}
		catch (Exception e)
		{
			updateFailed = true;
			ServiceLocator.Logger.Log(e.Message);
		}

		Events.RaiseTextUpdated();
	}

	public string Get (string term)
	{
		if (term == "Google") return updateFailed ? "FAIL" : "OK";

		if (cachedText.ContainsKey(term)) return cachedText[term];
		else return string.Format("Null text for the {0} term.", term);
	}
}