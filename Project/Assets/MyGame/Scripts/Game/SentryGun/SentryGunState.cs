using UnityEngine;
using System.Collections;

public class SentryGunWait : IEnemyState
{
	public SentryGunWait(MonoBehaviour controller)
	{
		this.Controller_ = controller;
		this.Controller_.GetComponent<EnemyFire> ().enabled = false;
	}
	public override IEnemyState Update()
	{
		if (GameManager.Instance.StateName () == "GamePlay") {
			return new SentryGunAttack(this.Controller_);
		}
		return null;
	}
}

public class SentryGunAttack : IEnemyState
{
	public SentryGunAttack(MonoBehaviour controller)
	{
		this.Controller_ = controller;
		this.Controller_.GetComponent<EnemyFire> ().enabled = true;
	}
	public override IEnemyState Update()
	{
		Vector3 thisToPlayer 		= Utility.Vector3AtoBXZ (this.Controller_.transform.position,this.Controller_.GetComponent<SentryGunController>().player.transform.position);
		Vector3 thisToPlayerCross 	= Utility.Vector3NormalizedCrossXZ (this.Controller_.transform.forward, thisToPlayer);
		float 	thisToPlayerDot 	= Utility.Vector3NormalizedDotXZ (this.Controller_.transform.forward,thisToPlayer);
		float 	degree 				= Mathf.Acos (thisToPlayerDot)*Mathf.Rad2Deg;
		float 	deltaDegree 		= 1.0f*Utility.normalDeltaTime;
		float 	rotateAngle;
		
		//変位する角度を微小角度におさめる
		rotateAngle = Mathf.Min (degree,deltaDegree);
		//左右判定
		if (thisToPlayerCross.y < 0.0f)
			rotateAngle = -rotateAngle;
		
		this.Controller_.transform.Rotate (new Vector3(0,1,0),rotateAngle);
		return null;
	}
	public override void OnTriggerEnter(Collider collision)
	{
		//プレイヤーが発射した弾以外を無視
		if(collision.gameObject.layer!=LayerMask.NameToLayer("Bullet"))return;
		//爆発エフェクトを配置
		GameObject.Instantiate (Resources.Load ("Prefabs/Effects/Detonator-Wide"),
		                        this.Controller_.transform.position,
		                        Quaternion.identity);
		//オブジェクトのスクリーン座標を計算
		Vector3 spos 	= Camera.main.WorldToScreenPoint (this.Controller_.transform.position);
		spos.x 			= spos.x / (float)Screen.width;
		spos.y 			= 1.0f-(spos.y / (float)Screen.height);
		//エフェクトに送信
		PostEffectScript.set (new Vector2 (spos.x, spos.y));
		
		GameObject.Destroy (this.Controller_.gameObject,0.1f);
	}
}