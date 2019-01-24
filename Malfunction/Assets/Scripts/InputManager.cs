using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager  {

    private InputManager instance;
    public InputManager Instance => instance ?? (instance = new InputManager());

    private InputManager() { }

    public void Initialize() { }

	public void UpdateInput(float dt)
    {
        bool inputProcessed = false;

        //Keyboard input
        Vector2 keysPressed = new Vector2();
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            keysPressed.x -= 1;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            keysPressed.x += 1;
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space))
            keysPressed.y = -1;
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            keysPressed.y = 1;
        if (keysPressed.x != 0 || keysPressed.y != 0)
        {
            inputProcessed = true;
        }
        //if (Input.GetKeyDown(KeyCode.KeypadEnter))
        //    GV.ws.popupManager.Next();
    }

	public void OnMouseOver()
	{
		MouseOver (Camera.main.ScreenToWorldPoint(Input.mousePosition));
	}

	private void _MouseClicked(Vector2 clickPos,float _dt, bool firstClick)
	{
		Vector2 clickedPos = Camera.main.ScreenToWorldPoint (clickPos);
		MouseDown (clickedPos,_dt, firstClick);
	}

	protected virtual void MouseOver(Vector2 mouseWorldPos)
	{

	}

	protected virtual void MouseDown(Vector2 mouseWorldPos, float _dt, bool firstClick)
	{
		//pc.MouseDown(mouseWorldPos,_dt, firstClick);
	}
    
        
}
