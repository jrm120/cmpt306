using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour {

	public Font font;
	
	public Texture MenuTab;
	public Texture SelectedMenuTab;

	public Camera mainCam;
	
	// The font used in the skill and shop menus
	GUIStyle style;

	JoystickMenuSelect jselector = new JoystickMenuSelect();

	// Use this for initialization
	void Start () {
		//Set label style
		style = new GUIStyle();
		style.font = font;
		style.alignment = TextAnchor.MiddleCenter;
		style.normal.textColor = Color.yellow;
	}
	
	// Update is called once per frame
	void Update () {
		jselector.Update ();
	}

	void OnGUI(){
		jselector.reset (2, 1);
		style.fontSize = 64;
		GUI.Label(new Rect (0, 0, mainCam.pixelWidth, mainCam.pixelHeight - 100), "LUIGO", style);
		style.fontSize = 24;


		Rect tutRect = new Rect (mainCam.pixelWidth /3-175/2, 2 * mainCam.pixelHeight / 3, 175, 100);

		if(jselector.selectedX == 0)
		{
			if(GUI.Button (tutRect, SelectedMenuTab)|| jselector.select)
			{
				Application.LoadLevel(1);
			}
		}
		else
		{
			if(GUI.Button (tutRect,MenuTab))
			{
			}
		}

		GUI.Label(tutRect, "Tutorial", style);
		
		Rect gameRect = new Rect (2*mainCam.pixelWidth /3-175/2, 2 * mainCam.pixelHeight / 3, 175, 100);
		if(jselector.selectedX == 1)
		{
			if(GUI.Button (gameRect, SelectedMenuTab)|| jselector.select)
			{
				Application.LoadLevel(3);
			}
		}
		else
		{
			if(GUI.Button (gameRect,MenuTab))
			{
				Application.LoadLevel(3);
			}
		}
		GUI.Label(new Rect (2*mainCam.pixelWidth /3-175/2, 2 * mainCam.pixelHeight / 3, 175, 100), "Play Game", style);

	}
}

