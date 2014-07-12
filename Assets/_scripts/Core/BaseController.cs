
public abstract class BaseController
{
	protected IState State;
	protected ILogger Logger;

	public BaseController ()
	{
		ServiceLocator.Initialize();

		State = ServiceLocator.State;
		Logger = ServiceLocator.Logger;
	}
}