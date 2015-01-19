
public class MainMenuController : BaseController
{
	public static void StartNewGame ()
	{
		State.Reset();
		State.GameStatus = GameStatus.InProgress;
	}
}