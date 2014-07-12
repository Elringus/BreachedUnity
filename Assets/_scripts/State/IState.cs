
public interface IState
{
	void Reset (bool resetRules = false);

	#region CONFIG
	int VersionMajor { get; set; }
	int VersionMiddle { get; set; }
	int VersionMinor { get; set; }

	bool StartedGame { get; set; }
	#endregion

	#region RULES
	int TotalDays { get; set; }
	int MaxAP { get; set; }
	#endregion

	#region STATE
	int CurrentDay { get; set; }
	int CurrentAP { get; set; }
	#endregion
}