
public static class ServiceLocator
{
	public static bool Initialized;

	public static IState State { get; set; }
	public static ILogger Logger { get; set; }

	public static void Initialize ()
	{
		if (Initialized) return;

		Logger = new UnityLogger();

		State = XMLState.Load();
		if (State.VersionMiddle < GlobalConfig.VERSION_MIDDLE || State.VersionMajor < GlobalConfig.VERSION_MAJOR)
		{
			State.Reset(true);
			Initialize();
			Logger.LogWarning("The saved state is outdated and will be reseted!");
			return;
		}

		Initialized = true;
	}
}
