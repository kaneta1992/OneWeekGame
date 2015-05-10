using UnityEngine;
using System.Collections;

public class PostEffectScript : MonoBehaviour {
	[SerializeField]
	private Material mat;

	private static Vector2 pos=Vector2.zero;
	private static float val = 0.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		val -= Time.deltaTime*5.0f;
		if (val < 0)
			val = 0;
	}
	
	void OnRenderImage(RenderTexture src,RenderTexture dest)
	{
		float sita = (1.0f - val) * Mathf.PI;
		Vector4 vec = new Vector4 (PostEffectScript.pos.x, PostEffectScript.pos.y, Mathf.Sin(sita)*1.1f, 0);
		mat.SetVector ("_Info", vec);
		Graphics.Blit (src, dest,mat);
	}

	public static void set(Vector2 p)
	{
		PostEffectScript.pos = p;
		PostEffectScript.val = 1.0f;
	}
}
