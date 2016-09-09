using UnityEngine;
using System.Collections;

public class HashIDs : MonoBehaviour
{
	// Here we store the hash tags for various strings used in our animators.
    public int climbState;
    public int runClimbState;
    public int speed;
    public int angularSpeed;
    public int direction;

    public int running;

    public int axisX;
    public int axisY;
    public int mouseX;

    public int crouch;

    public int dyingState;
	public int locomotionState;
	public int shoutState;
	public int deadBool;
	public int speedFloat;
	public int sneakingBool;
	public int shoutingBool;
	public int playerInSightBool;
	public int shotFloat;
	public int aimWeightFloat;
    public int angularSpeedFloat;
	public int openBool;

	void Awake ()
	{
        climbState = Animator.StringToHash("Climbing.Cliff Climb");
        runClimbState = Animator.StringToHash("Climbing.JumpUp");

        speed = Animator.StringToHash("Speed");
        angularSpeed = Animator.StringToHash("AngularSpeed");
        direction = Animator.StringToHash("Direction");
        deadBool = Animator.StringToHash("Dead");
        dyingState = Animator.StringToHash("Base Layer.Dying");

        running = Animator.StringToHash("Locomotion.Run");

        axisX = Animator.StringToHash("AxisX");
        axisY = Animator.StringToHash("AxisY");
        mouseX = Animator.StringToHash("MouseX");

        crouch = Animator.StringToHash("Crouch");

        playerInSightBool = Animator.StringToHash("PlayerInSight");

		locomotionState = Animator.StringToHash("Base Layer.Locomotion");
		shoutState = Animator.StringToHash("Shouting.Shout");

		sneakingBool = Animator.StringToHash("Sneaking");
		shoutingBool = Animator.StringToHash("Shouting");

		shotFloat = Animator.StringToHash("Shot");
		aimWeightFloat = Animator.StringToHash("AimWeight");
		openBool = Animator.StringToHash("Open");
	}
}
