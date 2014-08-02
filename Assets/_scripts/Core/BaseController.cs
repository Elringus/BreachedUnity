
public abstract class BaseController
{
	protected static IState State;
	protected static ILogger Logger;

	static BaseController ()
	{
		State = ServiceLocator.State;
		Logger = ServiceLocator.Logger;

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
