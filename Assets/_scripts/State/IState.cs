
public interface IState
{
	IState Reset ();

	#region CONFIG
	int VersionMajor { get; set; }
	int VersionMiddle { get; set; }
	int VersionMinor { get; set; }

	bool StartedGame { get; set; }
	#endregion

	#region MAIN_PROPERTIES
	int TotalDays { get; set; }
	int CurrentDay { get; set; }
	int MaxAP { get; set; }
	int CurrentAP { get; set; }
	#endregion
}