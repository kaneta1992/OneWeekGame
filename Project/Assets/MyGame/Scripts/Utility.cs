using UnityEngine;
using System.Collections;

public class Utility{
	//deltaTimeをフレーム数で正規化
	public static float normalDeltaTime
	{
		get{
			return Time.deltaTime*60.0f;
		}
	}
	public static int Bool2Int(bool flag)
	{
		return flag ? 1 : 0;
	}
	public static Vector3 Vector3AtoBXZ(Vector3 a,Vector3 b)
	{
		a.y = 0.0f;
		b.y = 0.0f;
		return b - a;
	}
	public static float Vector3NormalizedDotXZ(Vector3 a, Vector3 b)
	{
		a.y = 0.0f;
		b.y = 0.0f;
		return Vector3.Dot (a.normalized, b.normalized);
	}
	public static Vector3 Vector3NormalizedCrossXZ(Vector3 a, Vector3 b)
	{
		a.y = 0.0f;
		b.y = 0.0f;
		return Vector3.Cross (a.normalized, b.normalized);
	}
	public static void CreateBulletAndEffect(Object mazzleObject,Object bulletObject,Transform trans,float speed,int layerMask,int layer)
	{
		//発射エフェクト生成
		GameObject mazzle=GameObject.Instantiate(	mazzleObject,
													trans.position,
													trans.rotation)as GameObject;
		mazzle.transform.localScale = new Vector3 (5.0f, 5.0f, 5.0f);
		//弾生成
		GameObject bullet=GameObject.Instantiate(	bulletObject,
													trans.position,
													trans.rotation)as GameObject;
		bullet.GetComponent<BulletScript> ().Init (speed,layerMask,layer);
	}
	public static Color LerpColor(Color srcColor,Color destColor,float time)
	{
		return srcColor * (time) + destColor * (1.0f - time);
	}
	public static Vector2 LerpVector2(Vector2 srcVector,Vector2 destVector,float time)
	{
		return srcVector * (time) + destVector * (1.0f - time);
	}
	public static float Second2Frame60(float second)
	{
		return second*60.0f;
	}
}