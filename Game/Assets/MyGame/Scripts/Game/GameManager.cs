using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class IGameState
{
	public abstract IGameState Update ();
	public virtual string ClassName() 
	{
		return "IGameState";
	}
}

public class GameStart : IGameState
{
	public override IGameState Update()
	{
		if (GameManager.Instance.reciveMessage == "ToGamePlay")
		{
			return new GamePlay();
		}
		return null;
	}
	public override string ClassName()
	{
		return "GameStart";
	}
}

public class GamePlay : IGameState
{
	public override IGameState Update()
	{
		if (GameManager.Instance.reciveMessage == "ToGameClear")
		{
			return new GameClear();
		}
		if (GameManager.Instance.reciveMessage == "ToGameOver")
		{
			return new GameOver();
		}
		return null;
	}
	public override string ClassName()
	{
		return "GamePlay";
	}
}

public class GameClear : IGameState
{
	public override IGameState Update()
	{

		Application.LoadLevel ("Title");
		return new GameStart();
	}
	public override string ClassName()
	{
		return "GameClear";
	}
}

public class GameOver : IGameState
{
	public override IGameState Update()
	{
		Application.LoadLevel ("Title");
		return new GameStart();
	}
	public override string ClassName()
	{
		return "GameOver";
	}
}

public class GameManager : Singleton<GameManager> {
	private IGameState 	State_;
	public string 		reciveMessage{ private set; get;}
	public GameObject 	Player_;

	//public List
	// Use this for initialization
	void Start()
	{
		DontDestroyOnLoad (this.gameObject);
		this.State_ = new GameStart ();
		this.reciveMessage = "";
	}
	public void GameStart () {
		Start ();
	}
	
	// Update is called once per frame
	void Update () {
		Cursor.visible = false;
		if (Input.GetKey (KeyCode.Escape)) {
			Application.Quit ();
		}
		IGameState retState=this.State_.Update ();
		if (retState != null)
		{
			this.State_=retState;
		}
	}

	public void Send(string mes)
	{
		reciveMessage = mes;
	}

	public string StateName()
	{
		return State_.ClassName ();
	}
}
