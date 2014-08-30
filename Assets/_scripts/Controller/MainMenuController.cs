
public class MainMenuController : BaseController
{
	public void StartNewGame ()
	{
		State.Reset();
		State.GameStatus = GameStatus.InProgress;
	}
}