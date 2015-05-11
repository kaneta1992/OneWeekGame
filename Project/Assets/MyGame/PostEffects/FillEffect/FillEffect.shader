Shader "Custom/Fill" {
        Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Color ("Color", Color) = (1.0,1.0,1.0,1.0)
    }
    SubShader {
        Pass {
			ZTest Always Cull Off ZWrite Off
			Fog { Mode off }
			Blend One Zero
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

			uniform sampler2D _MainTex;
            uniform float4 _Color;

            struct v2f {
                float4 position : SV_POSITION;
                fixed4 color    : COLOR;
                float2 uv       : TEXCOORD0;
            };

            v2f vert(appdata_full v) {
                v2f o;
                o.position = mul(UNITY_MATRIX_MVP, v.vertex);
                o.uv       = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
                o.color    = v.color;
                return o;
            }

            float4 frag(v2f i) : COLOR {
                float4 ret = _Color;
                float4 tex = tex2D( _MainTex, i.uv);
                ret=lerp(ret,tex,1.0f-ret.a);
                return ret;
            }
            ENDCG
        }
    }
}

