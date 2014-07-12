
public class MainMenuController : BaseController
{
	public void StartNewGame ()
	{
		State.Reset();
		State.StartedGame = true;
	}
}