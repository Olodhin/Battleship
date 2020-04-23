using SwinGameSDK;

public class GameLogic
{
	public static void Main()
	{
		//Opens a new Graphics Window
		SwinGame.OpenGraphicsWindow("Battle Ships", 800, 600);

		//Loads resources
		GameResources.LoadResources();
		
		SwinGame.PlayMusic(GameResources.GameMusic("Background"));
		
		do
		{
			GameController.HandleUserInput();
			GameController.DrawScreen();
		}
		while(!(SwinGame.WindowCloseRequested() && GameController.CurrentState == GameState.Quitting));
		
		SwinGame.StopMusic();

		//Free Resources and Close Audio, to end the program.
		GameResources.FreeResources();
	}
}