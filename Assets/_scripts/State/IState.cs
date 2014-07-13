
public interface IState
{
	void Reset (bool resetRules = false);

	#region CONFIG
	int VersionMajor { get; set; }
	int VersionMiddle { get; set; }
	int VersionMinor { get; set; }
	#endregion

	#region RULES
	int TotalDays { get; set; }
	int MaxAP { get; set; }
	#endregion

	#region STATE
	GameProgressType GameProgress { get; set; }

	int CurrentDay { get; set; }
	int CurrentAP { get; set; }
	#endregion
}