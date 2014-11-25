using UnityEngine;
using System.Collections;

public class GUIDisplay : MonoBehaviour {

	//Level
	public int Level;
	public bool LevelComplete;

	//The empty health and energy bar
	public Texture HealthBar;
	public Texture EnergyBar;
	public Texture ShieldBar;

	//The colour fill for the health and energy bars
	public Texture Health;
	public Texture Energy;
	public Texture Shield;
	
	public Texture CoinDisplay;
	public Texture SelectedCoinDisplay;
	public Texture GemDisplay;
	public Texture Pause;
	public Texture MenuTab;
	public Texture Menu;
	public Texture ItemWindow;
	public Texture Banner;

	//Various icons used in the GUI
	public Texture HealthIcon;
	public Texture EnergyIcon;
	public Texture ShieldIcon;
	public Texture PlusIcon;
	public Texture MeleeIcon;
	public Texture RangedIcon;
	public Texture CoinIcon;
	public Texture GemIcon;
	public Texture UpgradeAmt;
	public Texture IncreaseButton;
	public Texture SelectedIncreaseButton;

	//The skill hud and upgrade textures
	public Texture SkillToUpgrade;
	public Texture SkillUpgradeMenu;
	public Texture SkillHud;
	public Texture SkillTab;
	
	GameObject player;
	PlayerControls playerScript;
	
	float healthAmount;
	float energyAmount;
	float shieldAmount;
	int coins;
	int gems;
	
	public Camera mainCam;
	public Font font;

	// The font used in the skill and shop menus
	GUIStyle style;

	// The font for the health and energy bars
	GUIStyle resourceStyle;

	// The font used in the skill hud
	GUIStyle skillText;

	// booleans for opening different menus
	bool paused;
	bool shopMenu;
	bool skillMenu;

	// Used to display skill-up bars in skill menu
	int skill1Bars, skill2Bars, skill3Bars, skill4Bars, skill5Bars, skill6Bars, skill7Bars, skill8Bars;

	JoystickMenuSelect jselector = new JoystickMenuSelect();

	// Use this for initialization
	void Start () {

		//Set label style
		style = new GUIStyle();
		style.font = font;
		style.alignment = TextAnchor.MiddleCenter;
		style.normal.textColor = Color.yellow;

		//Set health/energy label style
		resourceStyle = new GUIStyle ();
		resourceStyle.font = font;
		resourceStyle.alignment = TextAnchor.MiddleCenter;
		resourceStyle.normal.textColor = Color.white;
		resourceStyle.fontSize = 15;

		//Set skill hud label style
		skillText = new GUIStyle ();
		skillText.font = font;
		skillText.alignment = TextAnchor.LowerLeft;
		skillText.normal.textColor = Color.white;
		skillText.fontSize = 10;
	}
	
	// Update is called once per frame
	void Update () {
		jselector.Update ();
		if ( GameObject.FindGameObjectsWithTag("Player").Length != 0){
			if(player == null){
				player = GameObject.FindGameObjectWithTag("Player");
				playerScript = player.GetComponent<PlayerControls>();
			}
			healthAmount = playerScript.getHealth();
			energyAmount = playerScript.getEnergy();
			shieldAmount = playerScript.getShield();
			coins = playerScript.getCoins();
			gems = playerScript.getGems();
		}
		
		//Pause the game with P
		if ((Input.GetKeyDown (KeyCode.P) || Input.GetButtonDown ("Pause")) && (!shopMenu) && (!skillMenu)){
			if (paused){
				paused = false;
				Time.timeScale = 1;
			}else{
				paused = true;
				Time.timeScale = 0;
			}
		}

		//Open shop menu with tab (temporary)
		if ((Input.GetKeyDown (KeyCode.Tab) || Input.GetButtonDown ("Shop")) && (!paused) && (!skillMenu)){
			if (shopMenu){
				shopMenu = false;
				Time.timeScale = 1;
			}else{
				shopMenu = true;
				Time.timeScale = 0;
			}
			jselector.reset(3,1);
		}

		//Open skill menu with K
		if ((Input.GetKeyDown (KeyCode.K)||Input.GetButtonDown ("SkillMenu")) && (!paused) && (!shopMenu)){
			if (skillMenu){
				skillMenu = false;
				Time.timeScale = 1;
			}else{
				skillMenu = true;
				Time.timeScale = 0;
			}
			jselector.reset(2,4);
		}
	}

	void OnGUI()
	{
		//draw health bar background, get bar length, draw bar, display health and healthCap number text
		GUI.DrawTexture (new Rect(10, 10, 150, 50),HealthBar, ScaleMode.ScaleToFit, true, 0);
		float healthScale = (healthAmount/playerScript.healthCap)*100; // 100) * 110;
		GUI.DrawTexture (new Rect(48, 17, healthScale, 40),Health, ScaleMode.ScaleAndCrop, true, 0);
		GUI.Label(new Rect(110, 15, 10, 10), healthAmount+"/"+playerScript.healthCap, resourceStyle);
		
		//draw energy bar background, get bar length, draw bar, display energy and energyCap number text
		GUI.DrawTexture (new Rect(160, 13, 150, 50),EnergyBar, ScaleMode.ScaleToFit, true, 0);
		float energyScale = (energyAmount/playerScript.energyCap)*100 ;// 100) * 110;
		GUI.DrawTexture (new Rect(195, 20, energyScale, 35),Energy, ScaleMode.ScaleAndCrop, true, 0);
		GUI.Label(new Rect(257, 15, 10, 10), energyAmount+"/"+playerScript.energyCap, resourceStyle);


		//draw shield bar background, get bar length, draw bar, display shield and shieldCap number text
		if (playerScript.shield > 0)
		{
			GUI.DrawTexture (new Rect (310, 10, 140, 50), ShieldBar, ScaleMode.ScaleToFit, true, 0);
			float shieldScale = (shieldAmount/playerScript.shieldCap)*100;// 100) * 110;
			GUI.DrawTexture (new Rect (340, 21, shieldScale, 25), Shield, ScaleMode.ScaleAndCrop, true, 0);
			GUI.Label (new Rect (402, 15, 10, 10), shieldAmount + "/" + playerScript.shieldCap, resourceStyle);
		}

		//draw level
		if(Level < 1){
			GUI.Label(new Rect(450, 10, 150, 50), "Level: Tutorial", resourceStyle);
		}else{
			GUI.Label(new Rect(450, 10, 150, 50), "Level: " + Level, resourceStyle);
		}

		if(LevelComplete){
			GUI.DrawTexture (mainCam.pixelRect,Pause, ScaleMode.ScaleToFit, true, 0);
			style.fontSize = 68;
			GUI.Label (mainCam.pixelRect, "Level Complete", style);
		}
		
		//draw gem count background
		style.fontSize = 14;
		GUI.DrawTexture (new Rect(mainCam.pixelWidth - 100, 10, 70, 50),GemDisplay, ScaleMode.ScaleToFit, true, 0);
		GUI.Label (new Rect (mainCam.pixelWidth - 90, 10, 70, 50), gems.ToString(), style);

		//draw coin count background
		GUI.DrawTexture (new Rect(mainCam.pixelWidth - 175, 10, 70, 50),CoinDisplay, ScaleMode.ScaleToFit, true, 0);
		GUI.Label (new Rect (mainCam.pixelWidth - 165, 10, 70, 50), coins.ToString(), style);

		//draw pause label
		if(paused){
			GUI.DrawTexture (mainCam.pixelRect,Pause, ScaleMode.ScaleToFit, true, 0);
			style.fontSize = 68;
			GUI.Label (mainCam.pixelRect, "Paused", style);
		}

		//Draw the skill hud
		GUI.DrawTexture (new Rect(10,Screen.height-60,350,100),SkillHud);

		//Draw the skill tabs on top of the hud
		GUI.DrawTexture (new Rect(30,Screen.height-90,70,90),SkillTab);
		GUI.DrawTexture (new Rect(110,Screen.height-90,70,90),SkillTab);
		GUI.DrawTexture (new Rect(190,Screen.height-90,70,90),SkillTab);
		GUI.DrawTexture (new Rect(270,Screen.height-90,70,90),SkillTab);

		//Display the skill keybind
		GUI.Label(new Rect(40, Screen.height-80, 10, 10), "Ctrl", skillText);
		GUI.Label(new Rect(120, Screen.height-80, 10, 10), "F", skillText);
		GUI.Label(new Rect(200, Screen.height-80, 10, 10), "/", skillText);
		GUI.Label(new Rect(280, Screen.height-80, 10, 10), "/", skillText);

		//Display the skill energy cost
		GUI.Label(new Rect(65, Screen.height-25, 10, 10), "0", skillText);
		GUI.Label(new Rect(145, Screen.height-25, 10, 10), "15", skillText);
		GUI.Label(new Rect(225, Screen.height-25, 10, 10), "/", skillText);
		GUI.Label(new Rect(305, Screen.height-25, 10, 10), "/", skillText);

		//Display the skill icon
		GUI.DrawTexture (new Rect(60,Screen.height-80,30,47),MeleeIcon);
		GUI.DrawTexture (new Rect(130,Screen.height-80,40,47),RangedIcon);

		// Draw the shop menu (work in progress)
		if(shopMenu)
		{

			GUI.backgroundColor = new Color(0,0,0,0);
			style.fontSize = 28;

			//Draw the shop menu background
			GUI.DrawTexture (mainCam.pixelRect, Menu, ScaleMode.ScaleToFit, true, 0);
			GUI.DrawTexture (new Rect(mainCam.pixelWidth/2-460,-20,920,160),Banner);
			GUI.Label(new Rect(mainCam.pixelWidth/2-25, 30, 50, 50), "Shop", style);

			//Draw the health item
			GUI.DrawTexture (new Rect(350,115,225,325),ItemWindow);
			GUI.DrawTexture (new Rect(388,200,150,150),HealthIcon);
			if(jselector.selectedX == 0)
			{
				//draw selected texture
				if(GUI.Button (new Rect(410, 360, 100, 50),SelectedCoinDisplay)|| jselector.select)
				{
				}
			}
			else
			{
				if(GUI.Button (new Rect(410, 360, 100, 50),CoinDisplay))
				{
				}
			}
			GUI.Label(new Rect(435, 140, 50, 50), "Health", style);
			GUI.Label(new Rect(440, 358, 50, 50), "10", style);

			//Draw the energy item
			GUI.DrawTexture (new Rect(575,115,225,325),ItemWindow);
			GUI.DrawTexture (new Rect(613,200,150,150),EnergyIcon);
				if(jselector.selectedX == 1)
				{
					//draw selected texture
				if((GUI.Button (new Rect(635, 360, 100, 50),SelectedCoinDisplay)|| jselector.select) && healthAmount < playerScript.healthCap)
					{
						playerScript.AddHealth();
					}
				}
			else
				{
			if(GUI.Button (new Rect(635, 360, 100, 50),CoinDisplay))
			{
			}
				}
			GUI.Label(new Rect(660, 140, 50, 50), "Energy", style);
			GUI.Label(new Rect(665, 358, 50, 50), "20", style);

			//Draw the shield item
			GUI.DrawTexture (new Rect(800,115,225,325),ItemWindow);
			GUI.DrawTexture (new Rect(838,200,150,150),ShieldIcon);
					if(jselector.selectedX == 2)
					{
						//draw selected texture
				if((GUI.Button (new Rect(860, 360, 100, 50),SelectedCoinDisplay) || jselector.select) && shieldAmount < playerScript.shieldCap)
						{
							playerScript.AddShield();
						}
						}
				else
					{
			if(GUI.Button (new Rect(860, 360, 100, 50),CoinDisplay) && shieldAmount < playerScript.shieldCap)
			{
				playerScript.AddShield();
			}
					}
			GUI.Label(new Rect(885, 140, 50, 50), "Shield", style);
			GUI.Label(new Rect(890, 358, 50, 50), "10", style);

			//Display coins and gems on hand
			GUI.DrawTexture (new Rect(350,500,200,90), CoinDisplay);
			GUI.Label (new Rect (440, 520, 70, 50), coins.ToString(), style);
			GUI.DrawTexture (new Rect(810,500,200,90), GemDisplay);
			GUI.Label (new Rect (900, 520, 70, 50), gems.ToString(), style);
		}

		//Draw the skill menu
		if(skillMenu)
		{
			GUI.backgroundColor = new Color(0,0,0,0);
			GUI.DrawTexture (mainCam.pixelRect, SkillUpgradeMenu, ScaleMode.ScaleToFit, true, 0);

			// Health upgrade GUI (skill #1)
			GUI.DrawTexture (new Rect(460,130,225,100),SkillToUpgrade);
			GUI.DrawTexture (new Rect(475,150,50,50),HealthIcon);
			GUI.Label(new Rect(540, 134, 50, 50), "HEALTH", style);
			GUI.DrawTexture (new Rect(608,145,22,22),GemIcon);
			GUI.Label(new Rect(617, 134, 50, 50), "2", style);
			if (jselector.selectedX == 0 && jselector.selectedY==0)
			{
				if ((GUI.Button(new Rect(630,165,45,45),SelectedIncreaseButton)||jselector.select) && playerScript.Gems > 0 && skill1Bars < 5)
				{
					playerScript.IncreaseHealth();
					skill1Bars++;
				}
			}
			else
			{
				if (GUI.Button(new Rect(630,165,45,45),IncreaseButton));
			}
			// Energy upgrade GUI (skill #2)
			GUI.DrawTexture (new Rect(675,130,225,100),SkillToUpgrade);
			//GUI.DrawTexture (new Rect(mainCam.pixelWidth/2,130,225,100),SkillToUpgrade);
			GUI.DrawTexture (new Rect(690,150,50,50),EnergyIcon);
			GUI.Label(new Rect(755, 134, 50, 50), "ENERGY", style);
			GUI.DrawTexture (new Rect(823,145,22,22),GemIcon);
			GUI.Label(new Rect(832, 134, 50, 50), "3", style);
			if (jselector.selectedX==1 && jselector.selectedY==0)
			{
				if ((GUI.Button(new Rect(845,165,45,45),SelectedIncreaseButton)||jselector.select) && playerScript.Gems > 0 && skill2Bars < 5)
				{
					playerScript.IncreaseEnergy();
					skill2Bars++;
				}
			}
			else
			{
				if (GUI.Button(new Rect(845,165,45,45),IncreaseButton));
			}

			// Jump upgrade GUI (skill #3)
			GUI.DrawTexture (new Rect(460,230,225,100),SkillToUpgrade);
			GUI.DrawTexture (new Rect(475,250,50,50),PlusIcon);
			GUI.Label(new Rect(540, 234, 50, 50), "JUMP", style);
			GUI.DrawTexture (new Rect(608,245,22,22),GemIcon);
			GUI.Label(new Rect(617, 234, 50, 50), "4", style);
			if (jselector.selectedX==0 && jselector.selectedY==1)
			{
				if ((GUI.Button(new Rect(630,265,45,45),SelectedIncreaseButton)||jselector.select) && playerScript.Gems > 0 && skill3Bars < 5)
				{
					playerScript.IncreaseJump();
					skill3Bars++;
				}
			}
			else
			{
				if (GUI.Button(new Rect(630,265,45,45),IncreaseButton));
			}

			// Shield upgrade GUI (skill #4)
			GUI.DrawTexture (new Rect(675,230,225,100),SkillToUpgrade);
			GUI.DrawTexture (new Rect(690,250,50,50),ShieldIcon);
			GUI.Label(new Rect(755, 234, 50, 50), "SHIELD", style);
			GUI.DrawTexture (new Rect(823,245,22,22),GemIcon);
			GUI.Label(new Rect(832, 234, 50, 50), "2", style);
			if (jselector.selectedX==1 && jselector.selectedY==1)
			{
				if ((GUI.Button(new Rect(845,265,45,45),SelectedIncreaseButton)||jselector.select) && playerScript.Gems > 0 && skill4Bars < 5)
				{
					playerScript.IncreaseShield();
					skill4Bars++;
				}
			}
			else
			{
				if(GUI.Button(new Rect(845,265,45,45),IncreaseButton));
			}

			// Melee damage upgrade GUI (skill #5)
			GUI.DrawTexture (new Rect(460,330,225,100),SkillToUpgrade);
			GUI.DrawTexture (new Rect(482,350,35,50),MeleeIcon);
			GUI.Label(new Rect(540, 334, 50, 50), "MELEE", style);
			GUI.DrawTexture (new Rect(608,345,22,22),GemIcon);
			GUI.Label(new Rect(617, 334, 50, 50), "1", style);
			if (jselector.selectedY==2 && jselector.selectedX==0)
			{
				if ((GUI.Button(new Rect(630,365,45,45),SelectedIncreaseButton)||jselector.select) && playerScript.Gems > 0 && skill5Bars < 5)
				{
					playerScript.IncreaseMeleeDamage();
					skill5Bars++;
				}
			}
			else
			{
				if(GUI.Button(new Rect(630,365,45,45),IncreaseButton));
			}

			// Ranged damage upgrade GUI (skill #6)
			GUI.DrawTexture(new Rect(675,330,225,100),SkillToUpgrade);
			GUI.DrawTexture (new Rect(690,350,50,50),RangedIcon);
			GUI.Label(new Rect(755, 334, 50, 50), "RANGED", style);
			GUI.DrawTexture (new Rect(823,345,22,22),GemIcon);
			GUI.Label(new Rect(832, 334, 50, 50), "5", style);
			if (jselector.selectedY==2 && jselector.selectedX==1)
			{
				if (GUI.Button(new Rect(845,365,45,45),SelectedIncreaseButton) && playerScript.Gems > 0 && skill6Bars < 5)
				{
					playerScript.IncreaseRangedDamage();
					skill6Bars++;
				}
			}
			else
			{
				if (GUI.Button(new Rect(845,365,45,45),IncreaseButton));
			}
				
			// Teleport upgrade GUI (skill #7)
			GUI.DrawTexture (new Rect(460,430,225,100),SkillToUpgrade);
			GUI.DrawTexture (new Rect(482,450,35,50),CoinIcon);
			GUI.Label(new Rect(540, 434, 50, 50), "TELEPORT", style);
			GUI.DrawTexture (new Rect(608,445,22,22),GemIcon);
			GUI.Label(new Rect(617, 434, 50, 50), "4", style);
			if (jselector.selectedY==3 && jselector.selectedX==0)
			{
				if ((GUI.Button(new Rect(630,465,45,45),SelectedIncreaseButton)||jselector.select) && playerScript.Gems > 0 && skill7Bars < 5)
				{
					playerScript.IncreaseTeleport();
					skill7Bars++;
				}
			}
			else
			{
				if (GUI.Button(new Rect(630,465,45,45),IncreaseButton));
			}

			// Gem obtain upgrade GUI (skill #8)
			GUI.DrawTexture (new Rect(675,430,225,100),SkillToUpgrade);
			GUI.DrawTexture (new Rect(690,450,50,50),GemIcon);
			GUI.Label(new Rect(755, 434, 50, 50), "GEMFIND", style);
			GUI.DrawTexture (new Rect(823,445,22,22),GemIcon);
			GUI.Label(new Rect(832, 434, 50, 50), "1", style);
			if (jselector.selectedY==3 && jselector.selectedX==1)
			{
				if ((GUI.Button(new Rect(845,465,45,45),SelectedIncreaseButton)||jselector.select) && playerScript.Gems > 0 && skill8Bars < 5)
				{
					skill8Bars++;
				}
			}
			else
			{
				if (GUI.Button(new Rect(845,465,45,45),IncreaseButton));
			}

			//Handles all skill bar textures. Will not go past 5 bars.
			//Skill #1
			for(int i = 1; i <= skill1Bars; i++)
			{
				int spacing = i * 20;
				GUI.DrawTexture (new Rect(510 + spacing, 172,20,30),UpgradeAmt);
			}
			//Skill #2
			for(int i = 1; i <= skill2Bars; i++)
			{
				int spacing = i * 20;
				GUI.DrawTexture (new Rect(725 + spacing, 172,20,30),UpgradeAmt);
			}
			//Skill #3
			for(int i = 1; i <= skill3Bars; i++)
			{
				int spacing = i * 20;
				GUI.DrawTexture (new Rect(510 + spacing, 272,20,30),UpgradeAmt);
			}
			//Skill #4
			for(int i = 1; i <= skill4Bars; i++)
			{
				int spacing = i * 20;
				GUI.DrawTexture (new Rect(725 + spacing, 272,20,30),UpgradeAmt);
			}
			//Skill #5
			for(int i = 1; i <= skill5Bars; i++)
			{
				int spacing = i * 20;
				GUI.DrawTexture (new Rect(510 + spacing, 372,20,30),UpgradeAmt);
			}
			//Skill #6
			for(int i = 1; i <= skill6Bars; i++)
			{
				int spacing = i * 20;
				GUI.DrawTexture (new Rect(725 + spacing, 372,20,30),UpgradeAmt);
			}
			//Skill #7
			for(int i = 1; i <= skill7Bars; i++)
			{
				int spacing = i * 20;
				GUI.DrawTexture (new Rect(510 + spacing, 472,20,30),UpgradeAmt);
			}
			//Skill #8
			for(int i = 1; i <= skill8Bars; i++)
			{
				int spacing = i * 20;
				GUI.DrawTexture (new Rect(725 + spacing, 472,20,30),UpgradeAmt);
			}
		}
	}

	void CompleteLevel(){
		LevelComplete = true;
		if(Level > 0){
			Invoke("NextLevel", 2);
		}
	}

	void NextLevel(){
		LevelComplete = false;
		Level++;
	}
}