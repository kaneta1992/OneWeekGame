using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	//実情報、仮想情報、速度
	private Vector3 Pos_,VirtualPos_,PosVelocity_;
	private Vector3 Euler_,VirtualEuler_,EulerVelocity_;
	private float 	Fov_,VirtualFov_,FovVelocity_;

	// Use this for initialization
	void Start () {
		this.Pos_		 	= this.VirtualPos_ 		= this.transform.localPosition;
		this.PosVelocity_	= Vector3.zero;
		this.Euler_		 	= this.VirtualEuler_ 	= this.EulerVelocity_	= Vector3.zero;
		this.Fov_			= this.VirtualFov_ 		= this.gameObject.GetComponent<Camera> ().fieldOfView;
		this.FovVelocity_	= 0.0f;
	}

	public void ForceSetPos(Vector3 pos)
	{
		this.Pos_ = this.VirtualPos_ = pos;
	}
	public void SetPos(Vector3 pos)
	{
		this.Pos_ = pos;
	}
	public void SetVirtualPos(Vector3 pos)
	{
		this.VirtualPos_ = pos;
	}

	public void ForceSetFov(float fov)
	{
		this.Fov_ = this.VirtualFov_ = fov;
	}

	public void SetFov(float fov)
	{
		this.Fov_ = fov;
	}

	public void SetVirtualFov(float fov)
	{
		this.VirtualFov_ = fov;
	}

	public void ForceSetEuler(Vector3 euler)
	{
		this.Euler_ = this.VirtualEuler_ = euler;
	}
	public void SetEuler(Vector3 euler)
	{
		this.Euler_ = euler;
	}
	public void SetVirtualEuler(Vector3 euler)
	{
		this.VirtualEuler_ = euler;
	}
	public void SetEulerZ(float z)
	{
		this.Euler_.z = z;
	}
	
	// Update is called once per frame
	void Update () {
		this.Pos_ 	= Vector3.SmoothDamp(this.Pos_, this.VirtualPos_, ref this.PosVelocity_,0.3f,9999.9f,1.0f/60.0f);
		this.Euler_ = Vector3.SmoothDamp(this.Euler_, this.VirtualEuler_, ref this.EulerVelocity_,0.3f,9999.9f,1.0f/60.0f);
		this.Fov_ 	= Mathf.SmoothDamp(this.Fov_, this.VirtualFov_, ref this.FovVelocity_,0.1f,9999.9f,1.0f/60.0f);
		this.transform.localPosition = this.Pos_;
		this.transform.localRotation = Quaternion.Euler (this.Euler_);
		this.gameObject.GetComponent<Camera> ().fieldOfView = this.Fov_;
	}
}
