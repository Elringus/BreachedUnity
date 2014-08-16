
public abstract class BaseController
{
	protected static IState State;
	protected static ILogger Logger;
	protected static IText Text;

	static BaseController ()
	{
		State = ServiceLocator.State;
		Logger = ServiceLocator.Logger;
		Text = ServiceLocator.Text;

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
