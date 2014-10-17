using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	// Reference to player to access skills
	public Player1Controls player;

	
	// Players life
	public Texture playersHealthTexture;
	
	// Controls screen position of health texture
	public float screenPositionX;
	public float screenPositionY;
	
	// Controls size of icons
	public int iconSizeX = 25;
	public int iconSizeY = 25;
	
	// Players starting health
	public int playersHealth = 3;
	
	// Amount of EXP the player currently has
	public int curEXP = 0;
	
	// Amount of EXP needed to level up
	public int maxEXP = 50;
	
	// Players level
	public int level = 1;
	
	// Players skill points
	public int skillPoints = 0;

	// Toggles pause
	private bool paused = false;
	
	void Update()
	{
		if (curEXP >= maxEXP) 
		{
			Debug.Log ("Level up!");
			LevelUp ();
		}
		// This is just for testing the game
		if (Input.GetKeyDown (KeyCode.C))
		{
			Debug.Log ("Level: " + level + "  EXP: " + curEXP + "/" + maxEXP + "  SP: " + skillPoints);
		}
		if (Input.GetKeyDown (KeyCode.V))
		{
			curEXP = curEXP + maxEXP;
			Debug.Log ("Level: " + level + "  EXP: " + curEXP + "/" + maxEXP + "  SP: " + skillPoints);
		}
		
		// Pause game with either P or Esc button, time freezes when paused
		if (Input.GetKeyDown (KeyCode.P) || (Input.GetKeyDown (KeyCode.Escape)))
		{
			if (paused) 
			{
				paused = false;
				Time.timeScale = 1;
				Debug.Log ("Game Unpaused");
			}
			else 
			{
				paused = true;
				Time.timeScale = 0;
				Debug.Log ("Game Paused");
			}
		}
	}
	
	// On level up, reset curEXP and increase EXP needed for next level
	// Increments Lv, SP, HP
	void LevelUp()
	{
		curEXP = 0;
		maxEXP = maxEXP + 50;
		level++;
		skillPoints++;
		playersHealth++;
	}
	
	void OnGUI()
	{
		// Controls player health texture
		for (int h = 0; h < playersHealth; h++)
		{
			GUI.DrawTexture (new Rect(screenPositionX + (h * iconSizeX), screenPositionY, iconSizeX, iconSizeY), playersHealthTexture, ScaleMode.ScaleToFit, true, 0);
		}
		if (paused)
		{
			GUI.TextField(new Rect (Screen.width * .35f, Screen.height * .2f, Screen.width * .3f, Screen.height * .1f), "Skill Points: " + skillPoints);
			GUI.TextField(new Rect (Screen.width * .35f, Screen.height * .3f, Screen.width * .1f, Screen.height * .1f), "Current health: " + playersHealth);
			GUI.TextField(new Rect (Screen.width * .35f, Screen.height * .4f, Screen.width * .1f, Screen.height * .1f), "Current damage: " + player.attackSkill);
			GUI.TextField(new Rect (Screen.width * .35f, Screen.height * .5f, Screen.width * .1f, Screen.height * .1f), "Current jump: " + player.jumpSkill);
			if (GUI.Button (new Rect (Screen.width * .45f, Screen.height * .3f, Screen.width * .2f, Screen.height * .1f), "Increase Health")) 
			{
				if(skillPoints > 0)
				{
					playersHealth++;
					skillPoints--;
				}
				else
				{
					Debug.Log ("Not enough skill points!");
				}
			}
			if (GUI.Button (new Rect (Screen.width * .45f, Screen.height * .4f, Screen.width * .2f, Screen.height * .1f), "Increase Damage"))
			{
				if(skillPoints > 0)
				{
					player.attackSkill = player.attackSkill + 1;
					skillPoints--;
				}
			}
			if (GUI.Button (new Rect (Screen.width * .45f, Screen.height * .5f, Screen.width * .2f, Screen.height * .1f), "Increase Jump Skill")) 
			{
				if(skillPoints > 0)
				{
					player.jumpSkill = player.jumpSkill + 1;
					skillPoints--;
				}
				else
				{
					Debug.Log ("Not enough skill points!");
				}
			}
		}
	}
}