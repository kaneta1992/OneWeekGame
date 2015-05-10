using UnityEngine;
using System.Collections;

public class EnemyFire : MonoBehaviour {
	private Object 		MazzleObject_	= null;
	private Object 		BulletObject_	= null;

	private GameObject	FirePosition_	= null;
	[SerializeField]
	private float 		GunCoolTime_	= 0.1f;
	private float 		NowCoolTime_	= 0.0f;
	// Use this for initialization
	void Start () {
		this.MazzleObject_ 		= Resources.Load ("Prefabs/Effects/MuzzleFlash");
		this.BulletObject_ 		= Resources.Load ("Prefabs/Bullet");

		this.NowCoolTime_ 	= Random.Range (0.1f, 1.0f);
		this.FirePosition_ 	= this.transform.FindChild ("FirePosition").gameObject;
	}
	//発射
	public void Fire()
	{
		//マスク（Enemy EnemyBullet）
		int layerMask = ~(1 << LayerMask.NameToLayer ("Enemy")) & ~(1 << LayerMask.NameToLayer ("EnemyBullet"));

		this.NowCoolTime_ = this.GunCoolTime_;

		//弾生成処理
		Utility.CreateBulletAndEffect (this.MazzleObject_,
		                               this.BulletObject_,
		                               this.FirePosition_.transform,
		                               0.75f,
		                               layerMask,
		                               LayerMask.NameToLayer ("EnemyBullet"));
	}
	public bool IsFireOK()
	{
		return this.NowCoolTime_ == 0.0f;
	}
	// Update is called once per frame
	void Update () {
		this.NowCoolTime_ -= Time.deltaTime;

		if (this.NowCoolTime_ <= 0.0f) 
		{
			this.NowCoolTime_=0.0f;
		}
		if (this.IsFireOK ())
		{
			Fire ();
		}
	}
}
