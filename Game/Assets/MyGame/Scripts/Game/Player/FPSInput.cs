using UnityEngine;
using System.Collections;

public class FPSInput : MonoBehaviour {
	private CameraController 	CameraController_ 	= null;
	private FPSFire				Fire_				= null;
	private FPSController 		Controller_			= null;
	private Animator 			GunAnimator_		= null;
	private float				KeyDispBias_		= 0.0f;
	// Use this for initialization
	void Start () {
		this.CameraController_ 	= this.transform.FindChild ("Main Camera").gameObject.GetComponent<CameraController>();
		this.Fire_ 				= this.GetComponent<FPSFire> ();
		this.Controller_		= this.GetComponent<FPSController>();
		this.GunAnimator_ 		= GameObject.Find ("M4A1 Sopmod").GetComponent<Animator>();
	}
	//キー入力
	Vector3 GetKeyDisp()
	{
		Vector3 retDir;
		Vector3 obj_right_2D	= this.transform.right;
		Vector3 obj_forword_2D	= this.transform.forward;
		int 	key_W, key_A, key_S, key_D;
		Vector2 key_Disp;
		float 	keyBiasVal = 0.0f;

		obj_right_2D.y		= 0.0f;
		obj_forword_2D.y	= 0.0f;

		//キーバイアス計算
		if (Input.GetKey (KeyCode.W) ||
		    Input.GetKey (KeyCode.A) ||
		    Input.GetKey (KeyCode.S) ||
		    Input.GetKey (KeyCode.D))
		{
			keyBiasVal = 0.05f;
		} else {
			keyBiasVal = -0.05f;
		}
		this.KeyDispBias_ = Mathf.Clamp (this.KeyDispBias_+keyBiasVal,0.0f,1.0f);

		//キー入力取得
		key_W 		= Utility.Bool2Int(Input.GetKey (KeyCode.W));
		key_A 		= -(Utility.Bool2Int(Input.GetKey (KeyCode.A)));
		key_S 		= -(Utility.Bool2Int(Input.GetKey (KeyCode.S)));
		key_D 		= Utility.Bool2Int(Input.GetKey (KeyCode.D));
		key_Disp	= new Vector2 (key_D+key_A,key_W+key_S);

		//入力値調節
		retDir	=  obj_right_2D.normalized*key_Disp.normalized.x;
		retDir	+= obj_forword_2D.normalized*key_Disp.normalized.y;
		retDir 	=  retDir * this.KeyDispBias_;

		return retDir;
	}
	//マウス座標変位
	Vector2 GetMouseDisp()
	{
		Vector2 retDisp;
		retDisp.x 	= -Input.GetAxis ("Mouse Y");//*(1.0f/Time.timeScale);
		retDisp.y	= Input.GetAxis ("Mouse X");//*(1.0f/Time.timeScale);
		return retDisp;
	}

	//ゲーム進行時間計算
	float CalcGameSpeed(Vector3 stickDisp,int mouseClick)
	{
		float stick_Length = stickDisp.magnitude;
		return Mathf.Clamp (stick_Length+mouseClick,0.0001f,1.0f);
	}

	
	// Update is called once per frame
	void Update () {
		Vector3 stickDisp = GetKeyDisp ();
		Vector2 mouseDisp = GetMouseDisp ();
		//移動値送信
		this.Controller_.KeyDispUpdate(stickDisp);
		//カメラ回転値送信
		this.Controller_.MouseDispUpdate(mouseDisp);
		//発射判定
		if(Input.GetMouseButton(0))
		{
			if(this.Fire_.IsFireOK() && this.Fire_.IsBulletOK())
			{
				this.GunAnimator_.SetBool("Fire",true);
				this.Fire_.Fire();
			}
		}
		//ズーム判定
		if (Input.GetMouseButton (1)) {
			this.CameraController_.SetVirtualFov (30.0f);
		} else {
			this.CameraController_.SetVirtualFov (65.0f);
		}

		Time.timeScale = CalcGameSpeed (stickDisp,Utility.Bool2Int(Input.GetMouseButton(0)));
	}
}
