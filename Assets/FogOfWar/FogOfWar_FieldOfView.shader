Shader "Custom/Fog of War/Field of View"
{
	Properties
	{
		[FloatRange] _InnerRadius("Inner Radius %", Range(0, 1)) = 0.5

		[HideInInspector] _Center("Center", Vector) = (0.0, 0.0, 0.0, 0.0)
		[HideInInspector] _Radius("Radius", float) = 0.0

		[IntRange] _StencilRef("Stencil Reference Value", Range(0,255)) = 0
	}

	SubShader
	{
		Tags
		{
			"RenderType" = "Transparent"
			"Queue" = "Transparent-1"
		}

		Stencil
		{
			Ref[_StencilRef]
			Comp Always
			Pass Replace
		}

		ZWrite Off

		Pass
		{
			Blend One OneMinusSrcAlpha


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
				float3 worldPos : TEXCOORD1;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.uv = v.uv;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				return o;
			}

			float _FalloffStrength;
			float _InnerRadius;
			float _Radius;
			float4 _Center;

			static const float PI = 3.14159265;

			static const float INNER_RADIUS = _InnerRadius * _Radius;
			static const float OUTER_RADIUS = _Radius - INNER_RADIUS;

			float4 frag(v2f i) : SV_Target
			{
				float dist = distance(i.worldPos, _Center.xyz);
				float t = clamp((_Radius - dist) / OUTER_RADIUS, 0.0, 1.0);
				t = 1.0 - cos((t * PI) / 2.0);
				return clamp(t, 0.0, 1.0);
			}
			ENDCG
		}
	}
}