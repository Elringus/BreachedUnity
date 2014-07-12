
public abstract class BaseController
{
	protected IState State;

	public BaseController ()
	{
		ServiceLocator.Initialize();

		State = ServiceLocator.State;
	}
}