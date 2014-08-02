
public abstract class BaseController
{
	protected IState State;
	protected ILogger Logger;

	public BaseController ()
	{
		State = ServiceLocator.State;
		Logger = ServiceLocator.Logger;
	}
}