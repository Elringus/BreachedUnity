using UnityEngine;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class UnityState : BaseState
{
	private const string KEY_NAME = "GameState";

	public static IState Load ()
	{
		if (!PlayerPrefs.HasKey(KEY_NAME)) return new UnityState();

		preventSave = true;
		var serializer = new XmlSerializer(typeof(BaseState), new Type[] { typeof(UnityState) });
		var stringReader = new StringReader(PlayerPrefs.GetString(KEY_NAME));
		var xmlReader = new XmlTextReader(stringReader);
		var state = serializer.Deserialize(xmlReader) as UnityState;
		xmlReader.Close();
		stringReader.Close();
		preventSave = false;
		return state;
	}

	public override bool Save ()
	{
		if (!base.Save()) return false;

		var serializer = new XmlSerializer(typeof(BaseState), new Type[] { typeof(UnityState) });
		var stringWriter = new StringWriter();
		var xmlWriter = new XmlTextWriter(stringWriter);
		serializer.Serialize(xmlWriter, this);
		string xmlString = stringWriter.ToString();
		xmlWriter.Close();
		stringWriter.Close();
		PlayerPrefs.SetString(KEY_NAME, xmlString);
		PlayerPrefs.Save();

		return true;
	}
}