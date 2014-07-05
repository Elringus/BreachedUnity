
public interface IState
{
	void Reset ();

	#region CONFIG
	int VersionMajor { get; set; }
	int VersionMiddle { get; set; }
	int VersionMinor { get; set; }

	bool StartedGame { get; set; }
	#endregion

	#region MAIN_PROPERTIES
	int TotalDays { get; set; }
	int CurrentDay { get; set; }
	#endregion
}