using UnityEngine;
using System.Collections;

public abstract class IPlayerState
{
	protected FPSController Controller_;

	public abstract IPlayerState Update(Vector3 keyDisp,Vector2 mouseDisp,float gravity,float speed);
	public virtual void OnTriggerEnter(Collider collision){}
}
public class PlayerFall : IPlayerState
{
	public PlayerFall(FPSController controller)
	{
		Time.timeScale 		= 1.0f;
		this.Controller_ 	= controller;
		this.Controller_.GetComponent<FPSInput> ().enabled = false;
	}
	public bool StateUpdate(float gravity)
	{
		//地面についているかどうか
		if (this.Controller_.IsGrounded()) 
		{
			return true;
		}
		// 重力を計算
		this.Controller_.AddMoveDir (new Vector3(0.0f,-gravity * Utility.normalDeltaTime,0.0f));
		//回転
		float moveDispY 	= this.Controller_.GetMoveDir().y*2.0f;
		float eulerZ 		= Random.Range (moveDispY, -moveDispY);
		eulerZ	= Mathf.Pow (eulerZ, 2.0f);
		eulerZ 	= Mathf.Clamp (eulerZ, -4.0f, 4.0f);
		this.Controller_.cameraController.ForceSetEuler (new Vector3(30.0f,0.0f,eulerZ));
		return false;
	}
	public override IPlayerState Update(Vector3 keyDisp,Vector2 mouseDisp,float gravity,float speed)
	{
		bool retGrounded=StateUpdate (gravity);
		if (retGrounded) 
		{
			return new PlayerStart(this.Controller_);
		}
		return null;
	}
}

public class PlayerStart : IPlayerState
{
	public PlayerStart(FPSController controller)
	{
		GameManager.Instance.Send ("ToGamePlay");
		this.Controller_ = controller;
		//カメラ状態変更
		this.Controller_.cameraController.SetPos (this.Controller_.cameraController.transform.localPosition + new Vector3 (0.0f, -0.4f, 0.0f));
		this.Controller_.cameraController.SetEuler (new Vector3 (50.0f, 0.0f, 0.0f));
		this.Controller_.cameraController.SetVirtualEuler (Vector3.zero);
	}
	public override IPlayerState Update(Vector3 keyDisp,Vector2 mouseDisp,float gravity,float speed)
	{
		return new PlayerInput(this.Controller_);
	}
}

public class PlayerInput : IPlayerState
{
	public PlayerInput(FPSController controller)
	{
		this.Controller_ = controller;
		this.Controller_.GetComponent<FPSInput> ().enabled = true;
	}
	//角度更新
	public void DirectionUpdate(Vector2 disp)
	{
		this.Controller_.AddEuler (new Vector3 (disp.x * 2.0f, disp.y * 2.0f, 0.0f));
	}
	public void PositionUpdate(Vector3 disp,float gravity,float speed)
	{
		if (this.Controller_.IsGrounded()) 
		{
			this.Controller_.SetMoveDir(disp*speed);
		}
		this.Controller_.AddMoveDir (new Vector3(0.0f,-gravity * Utility.normalDeltaTime,0.0f));
	}
	public override IPlayerState Update(Vector3 keyDisp,Vector2 mouseDisp,float gravity,float speed)
	{
		PositionUpdate (keyDisp,gravity,speed);
		DirectionUpdate(mouseDisp);
		return null;
	}
	public override void OnTriggerEnter(Collider collision)
	{
		GameManager.Instance.Send ("ToToGameOver");
		this.Controller_.SetState (new PlayerToGameOver (this.Controller_));
	}
}

public class PlayerToGameOver : IPlayerState
{
	float EulerX_,EulerY;
	float Time_;
	public PlayerToGameOver(FPSController controller)
	{
		this.Controller_ = controller;
		this.Controller_.GetComponent<FPSInput> ().enabled = false;
		this.Controller_.transform.FindChild ("GunModel").gameObject.SetActive (false);
		this.Time_ = 3.0f;
		Time.timeScale = 1.0f;
		this.Controller_.SetMoveDir (Vector3.zero);
	}

	public override IPlayerState Update(Vector3 keyDisp,Vector2 mouseDisp,float gravity,float speed)
	{
		this.Time_ -= Time.deltaTime;
		if (this.Time_ > 2.5f) {
			float secondFrame=Utility.Second2Frame60(0.5f);
			this.Controller_.AddEuler (new Vector3((-120.0f)/secondFrame,90.0f/secondFrame,0.0f));
		}

		if (this.Time_ <= 0.0f) {
			GameManager.Instance.Send("ToGameOver");
		}
		return null;
	}
}