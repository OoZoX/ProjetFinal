Shader "Custom/Fog of War/Fog Canvas"
{
	Properties
	{
		[FloatRange] _Darkness("Darkness", Range(0, 1)) = 0.8
		[NoScaleOffset] _MainTex("Main Texture", 2D) = "white" {}
	}
		SubShader
	{
		Tags
		{
			"Queue" = "Transparent+1"
		}

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.uv = v.uv;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}

			sampler2D _MainTex;
			float _Darkness;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				col.a = _Darkness - col.a;
				return fixed4(0, 0, 0, col.a);
			}
			ENDCG
		}
	}
}
