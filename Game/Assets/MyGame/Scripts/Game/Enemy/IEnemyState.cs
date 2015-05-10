using UnityEngine;
using System.Collections;

public abstract class IEnemyState
{
	protected MonoBehaviour Controller_;
	
	public abstract IEnemyState Update();
	public virtual void OnTriggerEnter(Collider collision){}
}
