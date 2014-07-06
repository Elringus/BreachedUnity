using UnityEngine;
using System.Collections;

public abstract class BaseController : MonoBehaviour
{
	public static IState State;

	public static bool Initialized;

	public virtual void Awake () 
	{
		if (!Initialized) Initialize();
	}

	public virtual void Start ()
	{

	}

	public virtual void Update () 
	{
    	
	}

	private void Initialize ()
	{
		ServiceLocator.State = XMLState.Load();
		State = ServiceLocator.State;
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