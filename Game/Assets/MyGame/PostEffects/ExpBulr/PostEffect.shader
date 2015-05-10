Shader "Custom/ExpBlur" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Info ("Info", Vector) = (0.5,0.5,0,0)
    }

    SubShader {
        Pass {
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
            CGPROGRAM
				//ZTest Always;
                #include "UnityCG.cginc"

                #pragma vertex vert_img
                #pragma fragment frag
                #pragma fragmentoption ARB_precision_hint_fastest 

                #define PI 3.14159
                
                uniform sampler2D _MainTex;
				uniform float4 _Info;
				
                float4 frag( v2f_img i ) : COLOR {
                	//ブラー強度0で処理無視
                	if(_Info.z<=0.0f)
                	{
                		return tex2D( _MainTex, i.uv);
                	}
					float4 ret=float4(0,0,0,0);
					//ブラー比率
					float ratio[6]={1.0,0.8,0.6,0.4,0.3,0.2};
					//比率合計の逆数
					float invTotal=ratio[0]+ratio[1]+ratio[2]+ratio[3]+ratio[4]+ratio[5];
					invTotal=1.0/invTotal;
					
					float2 dir = _Info.xy - i.uv;

					//距離を計算する
					float len = length( dir );

					//tu,tvはテクセルサイズ
					float tu=1.0/_ScreenParams.x;
					float tv=1.0/_ScreenParams.y;
					//1テクセルベクトル
					dir = normalize( dir ) * float2( tu, tv );

					float power=50.0f*_Info.z;
					//dir *= ((sin(_Time.y*3.0)+1.0)*power) * len;
					dir *= power * len;
					//合成する
					for(int j=0;j<6;j++)
					{
						float3 col=float3(0,0,0);
						//色収差
						col.r=tex2D( _MainTex, i.uv + (dir) * (float)j).r;
						col.g=tex2D( _MainTex, i.uv + (dir*1.1) * (float)j).g;
						col.b=tex2D( _MainTex, i.uv + (dir*1.2) * (float)j).b;
						ret.rgb+=col*(ratio[j]*invTotal);
					}
					ret.a=1.0;
					return ret;
                }

            ENDCG
        }
    }
}