using UnityEngine;

public static class ServiceLocator
{
	public static bool Initialized;

	public static IState State { get; set; }

	public static void Initialize ()
	{
		if (Initialized) return;

		State = XMLState.Load();
		if (State.VersionMiddle < GlobalConfig.VERSION_MIDDLE || State.VersionMajor < GlobalConfig.VERSION_MAJOR)
		{
			Debug.LogWarning("The saved state is outdated and will be reseted!");
			State.Reset();
			Initialize();
			return;
		}

		Initialized = true;
	}
}
