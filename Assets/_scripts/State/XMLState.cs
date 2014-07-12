using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("XMLState")]
public class XMLState : IState
{
	// prevent calling Save() from all the property setters when loading state
	private static bool preventSave;

	#region CONFIG
	[XmlElement("VersionMajor")]
	private int _versionMajor;
	public int VersionMajor
	{
		get { return _versionMajor; }
		set { _versionMajor = value; }
	}
	[XmlElement("VersionMiddle")]
	private int _versionMiddle;
	public int VersionMiddle
	{
		get { return _versionMiddle; }
		set { _versionMiddle = value; }
	}
	[XmlElement("VersionMinor")]
	private int _versionMinor;
	public int VersionMinor
	{
		get { return _versionMinor; }
		set { _versionMinor = value; }
	}

	[XmlElement("StartedGame")]
	private bool _startedGame;
	public bool StartedGame
	{
		get { return _startedGame; }
		set { _startedGame = value; Save(); }
	}
	#endregion

	#region MAIN_PROPERTIES
	[XmlElement("TotalDays")]
	private int _totalDays = 8;
	public int TotalDays
	{
		get { return _totalDays; }
		set { _totalDays = value; Save(); }
	}

	[XmlElement("CurrentDay")]
	private int _currentDay = 1;
	public int CurrentDay
	{
		get { return _currentDay; }
		set { _currentDay = value; Save(); }
	}

	[XmlElement("MaxAP")]
	private int _maxAP = 10;
	public int MaxAP
	{
		get { return _maxAP; }
		set { _maxAP = value; Save(); }
	}

	[XmlElement("CurrentAP")]
	private int _currentAP = 10;
	public int CurrentAP
	{
		get { return _currentAP; }
		set { _currentAP = value; Save(); }
	}
	#endregion

	public static XMLState Load ()
	{
		if (PlayerPrefs.HasKey("XMLState"))
		{
			preventSave = true;
			XmlSerializer serializer = new XmlSerializer(typeof(XMLState));
			StringReader stringReader = new StringReader(PlayerPrefs.GetString("XMLState"));
			XmlTextReader xmlReader = new XmlTextReader(stringReader);
			XMLState state = (XMLState)serializer.Deserialize(xmlReader);
			xmlReader.Close();
			stringReader.Close();
			preventSave = false;
			return state;
		}
		else return new XMLState();
	}

	private void Save ()
	{
		if (preventSave) return;

		VersionMajor = GlobalConfig.VERSION_MAJOR;
		VersionMiddle = GlobalConfig.VERSION_MIDDLE;
		VersionMinor = GlobalConfig.VERSION_MINOR;

		XmlSerializer serializer = new XmlSerializer(typeof(XMLState));
		StringWriter stringWriter = new StringWriter();
		XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);
		serializer.Serialize(xmlWriter, this);
		string xml = stringWriter.ToString();
		xmlWriter.Close();
		stringWriter.Close();
		PlayerPrefs.SetString("XMLState", xml);
		PlayerPrefs.Save();
	}

	public IState Reset ()
	{
		XMLState state = new XMLState();

		// creat a seperate xml for the non-state data (TotalDays, APCost, etc.)
		// add button to edit non-state data
		// add checkbox for using the simple view
		// show simple view, edit state and edit non-state data when release type is alpha

		state.Save();
		return state;
	}
}