using System;
using System.IO;
using System.Collections.Generic;
using SwinGameSDK;

/*
 * <summary>
 * Controls displaying and collecting high score data.
 * </summary>
 * <remarks>
 * Data is saved to a file.
 * </remarks>
 */
public static class HighScoreController
{
	private const int NAME_WIDTH = 3;
	private const int SCORES_LEFT = 490;
	
	private static List<Score> _Scores = new List<Score>();
		
	/*
	 * <summary>
	 * The score structure is used to keep the name and
	 * score of the top players together.
	 * </summary>
	 */
	private struct Score : IComparable
	{
		public string Name;
		public int Value;
		
		/*
		 * <summary>
		 * Allows scores to be compared to facilitate sorting
		 * </summary>
		 * <param name="obj">the object to compare to</param>
		 * <returns>a value that indicates the sort order</returns>
		 */
		public int CompareTo(object obj)
		{
			Score other = (Score)obj;
			
			if (obj is Score)
			{
				return other.Value - this.Value;
			}
			else
			{
				return 0;
			}
		}
	}
	
	/*
	 * <summary>
	 * Loads the scores from the highscores text file.
	 * </summary>
	 * <remarks>
	 * The format is
	 * # of scores
	 * NNNSSS
	 *
	 * Where NNN is the name and SSS is the score
	 * </remarks>
	 */
	private static void LoadScores()
	{
		string fileName = SwinGame.PathToResource("high.txt");
		
		StreamReader input = new StreamReader(fileName);
		
		//read in the # of scores
		int numScores = int.Parse(input.ReadLine());
		
		_Scores.Clear();
		
		for(int i = 0; i < numScores; i++)
		{
			Score s;
			
			string line = input.ReadLine();
			
			s.Name = line.Substring(0, NAME_WIDTH);
			s.Value = int.Parse(line.Substring(NAME_WIDTH));
			_Scores.Add(s);
		}
		
		input.Close();
	}
	
	/*
	 * <summary>
	 * Saves the scores back to the highscores text file.
	 * </summary>
	 * <remarks>
	 * The format is
	 * # of scores
	 * NNNSSS
	 *
	 * Where NNN is the name and SSS is the score
	 * </remarks>
	 */
	private static void SaveScores()
	{
		string fileName = SwinGame.PathToResource("highscores.txt");
		
		StreamWriter output = new StreamWriter(fileName);
		
		output.WriteLine(_Scores.Count);
		
		foreach(Score s in _Scores)
		{
			output.WriteLine($"{s.Name}{s.Value}");
		}
		
		output.Close();
	}
	
	/*
	 * <summary>
	 * Draws the high scores to the screen.
	 * </summary>
	 */
	public static void DrawHighScores()
	{
		const int SCORES_HEADING = 40;
		const int SCORES_TOP = 80;
		const int SCORE_GAP = 30;
		
		if (_Scores.Count == 0)
		{
			LoadScores();
		}
		
		SwinGame.DrawText("   High Scores   ", Color.White, GameResources.GameFont("Courier"), SCORES_LEFT, SCORES_HEADING);
		
		//for all scores
		for(int i = 0; i < _Scores.Count; i++)
		{
			Score s = _Scores[i];
			
			//for scores 1 - 9 use 01 - 09
			if (i < 9)
			{
				SwinGame.DrawText(" " + (i + 1) + ":   " + s.Name + "   " + s.Value, Color.White, GameResources.GameFont("Courier"), SCORES_LEFT, SCORES_TOP + i * SCORE_GAP);
			}
			else
			{
				SwinGame.DrawText((i + 1) + ":   " + s.Name + "   " + s.Value, Color.White, GameResources.GameFont("Courier"), SCORES_LEFT, SCORES_TOP + i * SCORE_GAP);
			}
		}
	}
	
	/*
	 * <summary>
	 * Handles the user input during the top score screen.
	 * </summary>
	 */
	public static void HandleHighScoreInput()
	{
		if (SwinGame.MouseClicked(MouseButton.LeftButton) || SwinGame.KeyTyped(KeyCode.EscapeKey) || SwinGame.KeyTyped(KeyCode.ReturnKey))
		{
			GameController.EndCurrentState();
		}
	}
	
	/*
	 * <summary>
	 * Read the user's name for their highsSwinGame.
	 * </summary>
	 * <param name="value">the player's sSwinGame.</param>
	 * <remarks>
	 * This verifies if the score is a highsSwinGame.
	 * </remarks>
	 */
	public static void ReadHighScore(int val)
	{
		const int ENTRY_TOP = 500;
		
		if (_Scores.Count == 0)
		{
			LoadScores();
		}
		
		//if it a high score?
		if (val > _Scores[_Scores.Count - 1].Value)
		{
			Score s = new Score();
			s.Value = val;
			
			GameController.AddNewState(GameState.ViewingHighScores);
			
			int x = SCORES_LEFT + SwinGame.TextWidth(GameResources.GameFont("Courier"), "Name: ");
			
			SwinGame.StartReadingText(Color.White, NAME_WIDTH, GameResources.GameFont("Courier"), x, ENTRY_TOP);
			
			//read text from user
			while(SwinGame.ReadingText())
			{
				SwinGame.ProcessEvents();

				UtilityFunctions.DrawBackground();
				DrawHighScores();
				SwinGame.DrawText("Name: ", Color.White, GameResources.GameFont("Courier"), SCORES_LEFT, ENTRY_TOP);
				SwinGame.RefreshScreen();
			}
			
			s.Name = SwinGame.TextReadAsASCII();
			
			//changes from if (s.Name.Length < 3)
			while (s.Name.Length < 3)
			{
				s.Name = " " + s.Name;
			}
			
			_Scores.RemoveAt(_Scores.Count - 1);
			_Scores.Add(s);
			_Scores.Sort();
			
			GameController.EndCurrentState();
		}
	}
}