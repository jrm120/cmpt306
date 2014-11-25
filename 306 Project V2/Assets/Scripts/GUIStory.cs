using UnityEngine;
using System.Collections;

/// <summary>
/// modification of a Credits script made by "robin.theilade" on the Unity Answers thread found here:
///     http://answers.unity3d.com/questions/315124/in-game-credits.html
///     Please give this person support; with out this person, this modification would not exist
/// Modifications:
///     placement of original code
///     making viewArea into a private Rect object to make use of Screen.width and Screen.height variables
///     adding public TextAsset variable to use for placing custom text documents within credits
///     comments explaining what each function performs 
/// </summary>
public class GUIStory : MonoBehaviour
{
	
	float offset;                           //y-axis offset used to scroll text upward
	Rect viewArea;                          //area in which credits will appear
	
	public float speed;              //speed at which credits will scroll
	public GUIStyle creditsStyle;           //style in which credits will appear in-game
	public TextAsset creditsText;           //text document used for credits
	
	void Start()
	{
		InitializeValuesForScript();
	}
	
	void Update()
	{
		//keeps view area as large as the screen in all aspect ratios
		viewArea = new Rect(0, 0, Screen.width, Screen.height);
		
		//scrolls text upward based time step
		offset -= Time.deltaTime * this.speed;
		StartCoroutine (WaitAndLoad(25.0f));
	}

	IEnumerator WaitAndLoad(float waitTime) {
		yield return new WaitForSeconds(waitTime);
			Application.LoadLevel (2);
	}
	
	void OnGUI()
	{
		DisplayCredits();

	}
	
	//initialize all global private variables used in this script
	void InitializeValuesForScript() {
		viewArea = new Rect(0, 0, Screen.width, Screen.height);
		offset = this.viewArea.height;
	}
	
	//creates view area for placing credits inside
	void DisplayCredits()
	{
		GUI.BeginGroup(this.viewArea);
		
		Rect position = new Rect(0, offset, this.viewArea.width, this.viewArea.height);
		
		string text = SetExampleText();
		
			GUI.Label(position, text, this.creditsStyle);
		
		GUI.EndGroup();

	}
	
	//sets up example text to test script with if no 
	//  credit text documents are available within the project
	string SetExampleText()
	{
		string text;
		
		text = 
@"
There once was a man, he lived in a shoe... wait wrong game... ehem.



There once was a man named Inigo, he was very angry and quite mean.
He found his equal in a delightfully horrible girl.
Together they happily terrorized the world.




Then one day his princess was stolen from him by a self righteous knight Luca. 
His goodness and rightness must be stopped.




It is up to you, Inigo, to wrong all the rights.
It is up to you to run into the light and save a beautiful woman.
A woman who hates you just as much as you hate her. 

";
		
		return text;
	}
}
