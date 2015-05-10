using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {
	private Object 	BulletHit_ 			= null;
	private const 	float DestroyTime 	= 2.0f;
	private float 	BulletSpeed			= 5.0f;

	private int 	LayerMask_			= 0;

	// Use this for initialization
	void Start () {
		this.BulletHit_ = Resources.Load ("Prefabs/Effects/BulletHit");
		Destroy (this.gameObject,DestroyTime);
	}

	public void Init(float speed,int layerMask,int objectLayer)
	{
		this.LayerMask_ 		= layerMask;
		this.gameObject.layer	= objectLayer;
		this.BulletSpeed 		= speed;
		//コライダを設定
		BoxCollider boxCollider	= this.gameObject.GetComponent<BoxCollider> ();
		boxCollider.center 		= new Vector3(0.0f, 0.0f, speed * 0.5f);
		boxCollider.size 		= new Vector3(0.1f,0.1f,speed);
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		//弾ヒット判定
		if (Physics.Raycast (this.transform.position, this.transform.forward, out hit, this.BulletSpeed * 1.0f,this.LayerMask_))
		{
			//ヒットエフェクト表示
			Vector3 point			= hit.point + hit.normal.normalized*0.01f;
			GameObject hitEffect	= Instantiate(
													this.BulletHit_,
													point,
													Quaternion.LookRotation(hit.normal)) as GameObject;
			hitEffect.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
		}
		//位置更新
		this.transform.localPosition += this.transform.forward * this.BulletSpeed * Utility.normalDeltaTime;

	}
	void OnTriggerEnter(Collider collision)
	{
		Destroy (this.gameObject);
	}
}
