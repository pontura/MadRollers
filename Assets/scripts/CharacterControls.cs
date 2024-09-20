using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterControls : MonoBehaviour {

	//public bool isAutomata;
    CharacterBehavior characterBehavior;
    Player player;
    private float rotationY;
    private float turnSpeed = 2.8f;
    private float speedX = 9f;
    private bool mobileController;
    public bool ControlsEnabled = true;
    private CharactersManager charactersManager;

	float lastKeyPressedTime;
	int lastKeyPressed;

	float jumpingPressedSince;
	float jumpingPressedTime = 0.28f;


	void Start () {
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name != "Game")
			return;
        characterBehavior = GetComponent<CharacterBehavior>();
        player = GetComponent<Player>();
        charactersManager = Game.Instance.GetComponent<CharactersManager>();
	}
	public void EnabledMovements(bool enabledControls)
    {
		ControlsEnabled = enabledControls;
    }

	void LateUpdate () {
		if (characterBehavior == null || characterBehavior.player == null)
			return;
        if (Game.Instance == null || Game.Instance.state != Game.states.PLAYING)
            return;
        if (characterBehavior.state == CharacterBehavior.states.CRASH || characterBehavior.state == CharacterBehavior.states.DEAD) 
			return;
		if (Time.deltaTime == 0) return;

        if (characterBehavior.player.charactersManager == null || Game.Instance.state == Game.states.GAME_OVER)
            return;

#if UNITY_EDITOR
        //  UpdateStandalone();
        UpdateByVirtualJoystick();
#elif UNITY_ANDROID || UNITY_IOS
        //if (Data.Instance.controlsType == Data.ControlsType.GYROSCOPE)
        //    UpdateAccelerometer();
        //else
            UpdateByVirtualJoystick();
#else
            UpdateStandalone();
#endif

        characterBehavior.UpdateByController(rotationY); 
	}
    
	void Jump()
	{
		jumpingPressedSince = 0;
		characterBehavior.Jump ();
	}  
	float lastHorizontalKeyPressed;
	float last_x;
	float last_x_timer;
    private void moveByKeyboard()
    {
        float _speed = 0;// Data.Instance.inputManager.GetAxis(player.id, InputAction.horizontal);

		if ( _speed>0 && lastHorizontalKeyPressed <=0
			|| _speed<0 && lastHorizontalKeyPressed >=0
			|| _speed==0 && (lastHorizontalKeyPressed <0  ||  lastHorizontalKeyPressed >0)
		) {			
			lastHorizontalKeyPressed = _speed;
		//	if(!isAutomata)
			//	Data.Instance.inputSaver.MoveInX (transform.position.x);
		}

		MoveInX (_speed);
    }
	bool playerPlayed;
	public void MoveInX(float _speed)
	{
        if (Time.deltaTime == 0) return;

       // if (Data.Instance.isAndroid)
            RotateAccelerometer(_speed*15);
        //else
        //    RotateStandalone(_speed);

        characterBehavior.SetRotation (rotationY);
		//if (childs.Count > 0)
		//	UpdateChilds ();
	}
    void RotateAccelerometer(float _speed)
    {
        _speed *= 6;

        if (_speed > -2 && _speed < 2)
            _speed = 0;
        else if (_speed > 35)
            _speed = 35;
        else if (_speed < -35)
            _speed = -35;

        rotationY = _speed;
    }
    void RotateStandalone(float _speed)
    {
        if (_speed < -0.2f || _speed > 0.2f)
        {
            if (!playerPlayed)
            {
                playerPlayed = true;
                Data.Instance.multiplayerData.PlayerPlayed(characterBehavior.player.id);
            }
            float newPosX = _speed * speedX;
            float newRot = turnSpeed * (Time.deltaTime * 35);
            if (newPosX > 0)
                rotationY += newRot;
            else if (newPosX < 0)
                rotationY -= newRot;
            else if (rotationY > 0)
                rotationY -= newRot;
            else if (rotationY < 0)
                rotationY += newRot;
        }
        else
        {
            rotationY = 0;
        }

        if (rotationY > 30) rotationY = 30;
        else if (rotationY < -30) rotationY = -30;
    }
    //	void UpdateChilds()
    //	{
    //		foreach (CharacterBehavior cb in childs) {
    //			cb.controls.rotationY = rotationY / 1.5f;
    //			cb.transform.localRotation = transform.localRotation;
    //		}
    //	}
    //

    //void UpdateStandalone()
    //{
    //    if (Input.GetKeyDown(KeyCode.Alpha7))
    //        characterBehavior.characterMovement.DH(1);
    //    else if (Input.GetKeyDown(KeyCode.Alpha8))
    //        characterBehavior.characterMovement.DH(-1);

    //    if (Data.Instance.inputManager.GetButtonDown(player.id, InputAction.action3))
    //        characterBehavior.shooter.ChangeNextWeapon();

    //    if (Data.Instance.inputManager.GetButtonDown(player.id, InputAction.action2))
    //        characterBehavior.shooter.CheckFire();

    //    if (Data.Instance.inputManager.GetAxis(player.id, InputAction.vertical) <-0.1f && Data.Instance.inputManager.GetAxis(player.id, InputAction.horizontal) ==0)
    //    {
    //        characterBehavior.characterMovement.DashForward();
    //    }

    //    if (characterBehavior.state == CharacterBehavior.states.RUN)
    //    {
    //        if (Data.Instance.inputManager.GetButtonDown(player.id, InputAction.action1))
    //        {
    //            jumpingPressedSince = 0;
    //        }
    //        if (Data.Instance.inputManager.GetButton(player.id, InputAction.action1))
    //        {
    //            jumpingPressedSince += Time.deltaTime;
    //            if (jumpingPressedSince > jumpingPressedTime)
    //                Jump();
    //            else
    //                characterBehavior.JumpingPressed();
    //        }
    //        else if  (Data.Instance.inputManager.GetButtonUp(player.id, InputAction.action1))
    //            {
    //            Jump();
    //        }
    //    }
    //    else if (Data.Instance.inputManager.GetButtonDown(player.id, InputAction.action1))
    //    {
    //        Jump();
    //    }
    //    if (characterBehavior.player.charactersManager == null)
    //        return;

    //    if (characterBehavior.player.charactersManager.distance < 12)
    //        return;

    //    if(!isAutomata)
    //         moveByKeyboard();
    //}
    private void UpdateAccelerometer()
    {

        if (characterBehavior.player.charactersManager == null)
            return;
        if (characterBehavior.player.charactersManager.distance < 12)
            return;
    }


    JumpligStates jumpligState;
    enum JumpligStates
    {
        IDLE,
        JUMPING,
        JUMP_DONE
    }
    void UpdateByVirtualJoystick()
    {
        if (characterBehavior.player.charactersManager == null)
            return;
        if (characterBehavior.player.charactersManager.distance < 6)
            return;

        //if (Input.GetAxis("Vertical") > 0.6f)
        //{
        //    if (jumpligState == JumpligStates.IDLE)
        //        JumpInit();
        //} else
        if (Input.GetAxis("Vertical") < -0.8f)
        {
            characterBehavior.characterMovement.DashForward();
        }
        //else
        //{
        //    if (jumpligState == JumpligStates.JUMPING)
        //        DOJump();
        //    else
        //        jumpligState = JumpligStates.IDLE;
        //}

        //if (jumpligState == JumpligStates.JUMPING)
        //   {
        //     jumpingPressedSince += Time.deltaTime;
        //     if (jumpingPressedSince > jumpingPressedTime)
        //         DOJump();
        //     else
        //         characterBehavior.JumpingPressed();
        // }

        float v = Input.GetAxis("Horizontal");
        if (v != 0)
            v /= 1.25f;
        MoveInX(v);    

       

        
    }
    Vector2 pos;
    private void OnDown()
    {
        pos = Input.mousePosition;
    }
    private void OnUp()
    {
        if (Input.mousePosition.x < Screen.width / 2) return;
        if (pos == Input.mousePosition) characterBehavior.shooter.CheckFire();
    }
    void DOJump()
    {
        jumpligState = JumpligStates.JUMP_DONE;
        jumpingPressedSince = 0;
        characterBehavior.Jump();
    }
    public void JumpInit()
    {
        if (characterBehavior.state != CharacterBehavior.states.RUN)
            DOJump();
        else
        {
            jumpingPressedSince = 0;
            jumpligState = JumpligStates.JUMPING;
        }
    }
    //public void JumpRelease()
    //{
    //    jumping = false;
    //    if (characterBehavior.state == CharacterBehavior.states.RUN)
    //        DOJump();
    //}
}
