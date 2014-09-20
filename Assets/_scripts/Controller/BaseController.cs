
public abstract class BaseController
{
	protected static IState State { get { return ServiceLocator.State; } }
	protected static ILogger Logger { get { return ServiceLocator.Logger; } }
	protected static IText Text { get { return ServiceLocator.Text; } }

	static BaseController ()
	{
		Events.EngineFixed += (c, e) =>
		{
			if (State.FuelSynthed)
				State.GameStatus = GameStatus.GameWin;
		};

		Events.FuelSynthed += (c, e) =>
		{
			if (State.EngineFixed)
				State.GameStatus = GameStatus.GameWin;
		};
	}
}
