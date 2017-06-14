// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Diffuse Lightmap" {

	Properties {
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Base", 2D) = "white" {}
		_LightMapTex("LightMap", 2D) = "white" {}
		_Shininess("Shininess", Float) = 8.0
	}

	SubShader {
		Tags{ "RenderType" = "Opaque" }

		Pass {

			Lighting Off
			
			CGPROGRAM
		
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
			};

			struct appdata_lightmap {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST; 
			sampler2D _LightMapTex;
			float4 _LightMapTex_ST;

			float4 _Color;
			float _Shininess;


			inline fixed3 DecodeLightmapCustom(fixed4 color) {
				return (_Shininess * color.a) * color.rgb;
			}

			v2f vert(appdata_lightmap i) {
				v2f o;
				o.pos = UnityObjectToClipPos(i.vertex);
				o.uv0 = TRANSFORM_TEX(i.texcoord, _MainTex);
				o.uv1 = i.texcoord1.xy * _LightMapTex_ST.xy + _LightMapTex_ST.zw;

				return o;
			}

			half4 frag(v2f i) : COLOR{
				half4 color = tex2D(_MainTex, i.uv0);
				color.rgb *= DecodeLightmapCustom(tex2D(_LightMapTex, i.uv1)) * _Color;
				
				return color;
			}
			ENDCG
		}
	}
}