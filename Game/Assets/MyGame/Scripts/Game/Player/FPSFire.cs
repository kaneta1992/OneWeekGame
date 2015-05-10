using UnityEngine;
using System.Collections;

public class FPSFire : MonoBehaviour {
	private Object 				MazzleObject_		= null;
	private Object 				BulletObject_		= null;

	[SerializeField]
	private GameObject 			BulletsNum_ 		= null;
	//発射位置オブジェクト
	private GameObject 			FirePosition_		= null;
	private CameraController 	CameraController_ 	= null;
	private FPSController 		FPSController_ 		= null;
	private float 				NowCoolTime_		= 0.0f;
	private int 				NowBulletNum_		= 0;

	[SerializeField]
	private float 				GunCoolTime_		= 0.5f;
	[SerializeField]
	private int 				GunBulletNum_		= 30;
	// Use this for initialization
	void Start () 
	{
		this.MazzleObject_ 		= Resources.Load ("Prefabs/Effects/MuzzleFlash");
		this.BulletObject_ 		= Resources.Load ("Prefabs/Bullet");
		this.FPSController_ 	= this.gameObject.GetComponent<FPSController> ();
		this.CameraController_ 	= this.transform.FindChild ("Main Camera").gameObject.GetComponent<CameraController>();
		this.FirePosition_ 		= this.transform.FindChild ("FirePosition").gameObject;
		this.NowBulletNum_ 		= this.GunBulletNum_;
	}
	public bool IsFireOK()
	{
		return this.NowCoolTime_ == 0.0f;
	}
	public bool IsBulletOK()
	{
		return this.NowBulletNum_ > 0;
	}
	//発射
	public void Fire()
	{
		//マスク（Player Bullet無視）
		int layerMask = ~(1 << LayerMask.NameToLayer ("Player")) & ~(1 << LayerMask.NameToLayer ("Bullet"));

		//残弾処理
		this.NowCoolTime_ = this.GunCoolTime_;
		this.NowBulletNum_--;

		//弾生成処理
		Utility.CreateBulletAndEffect (this.MazzleObject_,
		                               this.BulletObject_,
		                               this.FirePosition_.transform,
		                               2.0f,
		                               layerMask,
		                               LayerMask.NameToLayer ("Bullet"));

		//キャラクタ、カメラ状態変更
		this.CameraController_.SetEulerZ (Random.Range(-1,1));
		this.FPSController_.AddEuler (new Vector3(Random.Range(-0.5f,-1.0f),0.0f,0.0f));
	}
	// Update is called once per frame
	void Update () 
	{
		this.NowCoolTime_ -= Time.deltaTime;
		if (this.NowCoolTime_ <= 0.0f) 
		{
			this.NowCoolTime_ = 0.0f;
		}
		//残弾表示
		TextMesh text	 = BulletsNum_.GetComponent<TextMesh> ();
		text.text		 = this.NowBulletNum_.ToString ();
	}
}