using UnityEngine;
using System.Collections;

public class MosquitoWait : IEnemyState
{
	public MosquitoWait(MonoBehaviour controller)
	{
		this.Controller_ = controller;
		this.Controller_.GetComponent<EnemyFire> ().enabled = false;
	}
	public override IEnemyState Update()
	{
		if (GameManager.Instance.StateName () == "GamePlay") {
			return new MosquitoAttack(this.Controller_);
		}
		return null;
	}
}

public class MosquitoAttack : IEnemyState
{
	public 	float 	SmoothTime_		= 0.3f;
	private Vector3 Velocity_		= Vector3.zero;

	//次の移動までの時間
	private float 	NextMoveTime_	=0.0f;
	//目指す位置
	private Vector3 TargetPos_;


	public MosquitoAttack(MonoBehaviour controller)
	{
		this.Controller_ = controller;
		this.Controller_.GetComponent<EnemyFire> ().enabled = true;
	}
	public override IEnemyState Update()
	{
		this.NextMoveTime_ -= Time.deltaTime;
		//移動位置設定処理
		if (this.NextMoveTime_<0.0f) {
			this.NextMoveTime_ = Random.Range (1.0f,2.0f);
			this.TargetPos_=new Vector3(Random.Range(-20.0f,20.0f),0.0f,Random.Range(0.0f,20.0f));
		}

		//マイフレームy座標を揺らす
		this.TargetPos_.y 	= Random.Range (-5.0f, 15.0f);
		Vector3 this2Player = this.Controller_.GetComponent<MosquitoController> ().player.transform.position-this.Controller_.transform.position;

		//状態更新
		this.Controller_.transform.position = Vector3.SmoothDamp(this.Controller_.transform.position, this.TargetPos_, ref Velocity_, SmoothTime_);
		this.Controller_.GetComponent<MosquitoController> ().transform.rotation = Quaternion.LookRotation (this2Player.normalized);
		
		return null;
	}
	public override void OnTriggerEnter(Collider collision)
	{
		//プレイヤーの弾以外無視
		if(collision.gameObject.layer!=LayerMask.NameToLayer("Bullet"))return;
		//爆発エフェクトを設置
		GameObject.Instantiate (Resources.Load ("Prefabs/Effects/Detonator-Wide"),
		                        this.Controller_.transform.position,
		                        Quaternion.identity);

		//スクリーン座標を計算
		Vector3 spos	= Camera.main.WorldToScreenPoint (this.Controller_.transform.position);
		spos.x 			= spos.x / (float)Screen.width;
		spos.y 			= 1.0f-(spos.y / (float)Screen.height);
		//エフェクトに送信
		PostEffectScript.set (new Vector2 (spos.x, spos.y));
		
		GameObject.Destroy (this.Controller_.gameObject,0.1f);
	}
}