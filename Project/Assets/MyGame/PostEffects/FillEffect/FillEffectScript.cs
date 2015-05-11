using UnityEngine;
using System.Collections;

public class FillEffectScript : MonoBehaviour {
	[SerializeField]
	private Shader Shader_=null;
	private Material Material_=null;

	private static Color	SrcColor_=new Color(0.0f,0.0f,0.0f,1.0f);
	private static Color	DestColor_=new Color(0.0f,0.0f,0.0f,1.0f);
	private static float	AnimationTime_ 		= 0.0f;
	private static float	MaxAnimationTime_ 	= 1.0f;

	public static void SetColorAnimation(Color srcColor,Color destColor,float animationTime)
	{
		FillEffectScript.SrcColor_ = srcColor;
		FillEffectScript.DestColor_ = destColor;
		FillEffectScript.AnimationTime_ =
			FillEffectScript.MaxAnimationTime_ = animationTime;
	}

	void OnRenderImage(RenderTexture src,RenderTexture desc)
	{
		Color color = Utility.LerpColor (FillEffectScript.SrcColor_,
		                                 FillEffectScript.DestColor_,
		                                 FillEffectScript.AnimationTime_/FillEffectScript.MaxAnimationTime_
		                                 );
		this.Material_.SetColor("_Color", color);
		Graphics.Blit (src, desc, this.Material_);
	}
	
	// Use this for initialization
	void Start () {
		this.Material_ = new Material(this.Shader_);
		this.Material_.hideFlags = HideFlags.HideAndDontSave;
	}
	
	// Update is called once per frame
	void Update () {
		FillEffectScript.AnimationTime_ -= 0.0166666f;
		FillEffectScript.AnimationTime_ = Mathf.Max (FillEffectScript.AnimationTime_,0.0f);
	}
}


