using UnityEngine;

public abstract class BaseView : MonoBehaviour
{
	protected IState State;

	protected virtual void Awake ()
	{
		ServiceLocator.Initialize();

		State = ServiceLocator.State;
	}

	protected virtual void Start ()
	{

	}

	protected virtual void Update ()
	{

	}

	protected virtual void OnGUI ()
	{
		GUI.Label(new Rect(10, Screen.height - 40, 150, 80), string.Format("Breached {0}\nver. {1}.{2}.{3}",
			GlobalConfig.RELEASE_TYPE, GlobalConfig.VERSION_MAJOR, GlobalConfig.VERSION_MIDDLE, GlobalConfig.VERSION_MINOR));
	}
}