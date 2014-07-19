using System;
using System.Collections.Generic;
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
	#endregion

	#region RULES
	[XmlElement("TotalDays")]
	private int _totalDays;
	public int TotalDays
	{
		get { return _totalDays; }
		set { _totalDays = value; Save(); }
	}

	[XmlElement("MaxAP")]
	private int _maxAP;
	public int MaxAP
	{
		get { return _maxAP; }
		set { _maxAP = value; Save(); }
	}

	[XmlElement("EnterSectorAPCost")]
	private int _enterSectorAPCost;
	public int EnterSectorAPCost
	{
		get { return _enterSectorAPCost; }
		set { _enterSectorAPCost = value; Save(); }
	}

	[XmlElement("LootCharges")]
	private int _lootCharges;
	public int LootCharges
	{
		get { return _lootCharges; }
		set { _lootCharges = value; Save(); }
	}

	[XmlElement("SectorsParameters")]
	private List<SectorParameters> _sectorsParameters;
	public List<SectorParameters> SectorsParameters
	{
		get { return _sectorsParameters; }
		set { _sectorsParameters = value; Save(); }
	}
	#endregion

	#region STATE
	[XmlElement("GameProgress")]
	private int _gameProgress;
	public GameProgressType GameProgress
	{
		get { return (GameProgressType)_gameProgress; }
		set { _gameProgress = (int)value; Save(); }
	}

	[XmlElement("CurrentDay")]
	private int _currentDay;
	public int CurrentDay
	{
		get { return _currentDay; }
		set { _currentDay = value; Save(); }
	}

	[XmlElement("CurrentAP")]
	private int _currentAP;
	public int CurrentAP
	{
		get { return _currentAP; }
		set { _currentAP = value; Save(); }
	}

	[XmlElement("MineralA")]
	private int _mineralA;
	public int MineralA
	{
		get { return _mineralA; }
		set { _mineralA = value; Save(); }
	}
	[XmlElement("MineralB")]
	private int _mineralB;
	public int MineralB
	{
		get { return _mineralB; }
		set { _mineralB = value; Save(); }
	}
	[XmlElement("MineralC")]
	private int _mineralC;
	public int MineralC
	{
		get { return _mineralC; }
		set { _mineralC = value; Save(); }
	}
	#endregion

	public static XMLState Load ()
	{
		//if (PlayerPrefs.HasKey("XMLState"))
		//{
		//	preventSave = true;
		//	XmlSerializer serializer = new XmlSerializer(typeof(XMLState));
		//	StringReader stringReader = new StringReader(PlayerPrefs.GetString("XMLState"));
		//	XmlTextReader xmlReader = new XmlTextReader(stringReader);
		//	XMLState state = (XMLState)serializer.Deserialize(xmlReader);
		//	xmlReader.Close();
		//	stringReader.Close();
		//	preventSave = false;
		//	return state;
		//}
		//else return new XMLState();

		if (File.Exists(Path.Combine(Environment.CurrentDirectory, "state.xml")))
		{
			preventSave = true;
			var serializer = new XmlSerializer(typeof(XMLState));
			using (var stream = new FileStream(Path.Combine(Environment.CurrentDirectory, "state.xml"), FileMode.Open))
			{
				var state = serializer.Deserialize(stream) as XMLState;
				preventSave = false;
				return state;
			}
		}
		else
		{
			var state = new XMLState();
			state.Reset(true);
			ServiceLocator.Logger.Log("The save file cannot be found and was created.");
			return state;
		}
	}

	private void Save ()
	{
		if (preventSave) return;

		VersionMajor = GlobalConfig.VERSION_MAJOR;
		VersionMiddle = GlobalConfig.VERSION_MIDDLE;
		VersionMinor = GlobalConfig.VERSION_MINOR;

		//XmlSerializer serializer = new XmlSerializer(typeof(XMLState));
		//StringWriter stringWriter = new StringWriter();
		//XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter);
		//serializer.Serialize(xmlWriter, this);
		//string xml = stringWriter.ToString();
		//xmlWriter.Close();
		//stringWriter.Close();
		//PlayerPrefs.SetString("XMLState", xml);
		//PlayerPrefs.Save();

		var serializer = new XmlSerializer(typeof(XMLState));
		using (var stream = new FileStream(Path.Combine(Environment.CurrentDirectory, "state.xml"), FileMode.Create)) 
			serializer.Serialize(stream, this);
	}

	public void Reset (bool resetRules = false)
	{
		preventSave = true;

		#region RULES
		if (resetRules)
		{
			GameProgress = GameProgressType.FirstLaunch;

			TotalDays = 8;
			MaxAP = 10;

			EnterSectorAPCost = 3;
			LootCharges = 3;
			SectorsParameters = new List<SectorParameters> { 
				new SectorParameters(1, 4, 0, 3, 3), 
				new SectorParameters(2, 5, 3, 0, 3), 
				new SectorParameters(3, 3, 3, 3, 0), 
				new SectorParameters(4, 6, 2, 2, 2) 
			};
		}
		#endregion

		#region STATE
		CurrentDay = 1;
		CurrentAP = MaxAP;

		MineralA = 0;
		MineralB = 0;
		MineralC = 0;
		#endregion

		preventSave = false;
		this.Save();
	}
}