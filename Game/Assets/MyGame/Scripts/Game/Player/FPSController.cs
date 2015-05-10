using UnityEngine;
using System.Collections;

public class FPSController : MonoBehaviour {
	public CameraController cameraController {
		private set;
		get;
	}
	private CharacterController CharaController_	= null;

	private IPlayerState	State_		= null;

	private float			Gravity		= 0.0098f;
	private float			Speed		= 0.3f;

	//移動速度向き
	private Vector3			MoveDir_;
	//回転状態
	private Vector3			RotateEuler_;

	private Vector3			KeyDisp_	= Vector3.zero;
	private Vector2			MouseDisp_	= Vector2.zero;

	// Use this for initialization
	void Start () {
		this.CharaController_ = this.GetComponent<CharacterController> ();
		this.cameraController = this.transform.FindChild ("Main Camera").gameObject.GetComponent<CameraController>();
		MoveDir_ 	= Vector3.zero;
		RotateEuler_	= Vector3.zero;
		State_ 		= new PlayerFall (this);
	}
	public void MouseDispUpdate(Vector2 disp)
	{
		this.MouseDisp_ = disp;
	}
	public void KeyDispUpdate(Vector3 disp)
	{
		this.KeyDisp_ = disp;
	}
	public void AddEuler(Vector3 euler)
	{
		this.RotateEuler_ += euler;
	}
	public void AddMoveDir(Vector3 move)
	{
		this.MoveDir_ += move;
	}
	public void SetMoveDir(Vector3 move)
	{
		this.MoveDir_ = move;
	}
	public Vector3 GetMoveDir()
	{
		return MoveDir_;
	}
	// Update is called once per frame
	void Update () {
		IPlayerState retState=this.State_.Update (this.KeyDisp_,this.MouseDisp_,this.Gravity,this.Speed);
		this.CharaController_.Move(this.MoveDir_);
		this.transform.localRotation = Quaternion.Euler (this.RotateEuler_);
		if (retState != null)
		{
			this.State_=retState;
		}
	}
	public bool IsGrounded()
	{
		return this.CharaController_.isGrounded;
	}
	void OnTriggerEnter(Collider collision)
	{
		GameManager.Instance.Send ("ToGameOver");
	}
}
