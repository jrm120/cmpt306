using UnityEngine;
using System.Collections;

public class JoystickMenuSelect {

	public int selectedX, selectedY;
	private int countX = 1, countY = 1;
	private bool _select = false;
	public bool select
	{
		get
		{
			if (_select)
			{
				_select = false;
				return true;
			}
			return false;
		}
		set
		{
			if (value)
				Debug.Log("SELECT IT SUUUUCKER");
			_select = value;
		}
	}

	float prevHor = 0;
	float prevVert = 0;
	public void Update()
	{
		select = Input.GetButtonDown ("Jump");
		float currHor = Input.GetAxis ("Horizontal");
		float currVert = Input.GetAxis ("Vertical");
		if (currHor > 0.2 && prevHor < 0.2) 
		{
			selectedX += 1;
			selectedX %= countX;
		}
		else if (currHor < -0.2 && prevHor > -0.2) 
		{
			selectedX -= 1 - countX;
			selectedX %= countX;
		}
		
		if (currVert > -0.2 && prevVert < -0.2) 
		{
			selectedY += 1;
			selectedY %= countY;
		}
		else if (currVert < 0.2 && prevVert > 0.2) 
		{
			selectedY -= 1 - countY;
			selectedY %= countY;
		}
		prevHor = currHor;
		prevVert = currVert;
	}

	public void reset (int countX, int countY)
	{
		selectedX = selectedY = 0;
		this.countX = countX;
		this.countY = countY;
	}
}
