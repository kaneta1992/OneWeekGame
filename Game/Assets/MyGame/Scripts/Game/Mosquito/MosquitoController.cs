using UnityEngine;
using System.Collections;

public class MosquitoController : MonoBehaviour {
	public 	GameObject 	player{ private set; get;}
	private IEnemyState State_=null;

	// Use this for initialization
	void Start () {
		this.player = GameObject.Find ("Player");
		this.State_ = new MosquitoWait (this);
	}

	void Update() {
		IEnemyState retState=this.State_.Update ();
		if (retState != null)
		{
			this.State_=retState;
		}
	}
	
	void OnTriggerEnter(Collider collision)
	{
		this.State_.OnTriggerEnter (collision);
	}
}
