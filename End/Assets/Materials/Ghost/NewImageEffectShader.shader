// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/NewImageEffectShader"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		_len ("length", float) = 1.0
	}
 
	CGINCLUDE
	#include "UnityCG.cginc"

	sampler2D _MainTex;
	sampler2D _AlphaTex;

	float _bool;
	float _len;
	float _L;
	float _Hiding;


	float4 _Offset0;
	float4 _Offset1;
	float4 _Offset2;
	float4 _Offset3;

			struct appdata_t                           //vert输入
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};
 
			struct v2f                                 //vert输出数据结构
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _Color;
 
			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;

				return OUT;
			}

			v2f vert1(appdata_t IN)
			{
				v2f OUT;
				float4 pos = mul(unity_ObjectToWorld,IN.vertex);

				OUT.vertex = mul(UNITY_MATRIX_VP,pos+_len* _Offset0 );
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;

				return OUT;
			}

			v2f vert2(appdata_t IN)
			{
				v2f OUT;
				float4 pos = mul(unity_ObjectToWorld,IN.vertex);

				OUT.vertex = mul(UNITY_MATRIX_VP,pos+_len*_Offset1);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;

				return OUT;
			}

			v2f vert3(appdata_t IN)
			{
				v2f OUT;
				float4 pos = mul(unity_ObjectToWorld,IN.vertex);

				OUT.vertex = mul(UNITY_MATRIX_VP,pos+_len*_Offset2);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;

				return OUT;
			}

			v2f vert4(appdata_t IN)
			{
				v2f OUT;
				float4 pos = mul(unity_ObjectToWorld,IN.vertex);

				OUT.vertex = mul(UNITY_MATRIX_VP,pos+_len*_Offset3);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;

				return OUT;
			}
 
			
 
			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);
 
				return color ;
			}
 
			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
				c.rgb *= c.a;
				return c* _Hiding;
			}

			fixed4 frag1(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
				c.a *= 0.8;
				c.rgb *= c.a;
				
				return c* _Hiding;
			}

			fixed4 frag2(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
				c.a *= 0.6;
				c.rgb *= c.a;
				
				return c* _Hiding;
			}

			fixed4 frag3(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
				c.a *= 0.4;
				c.rgb *= c.a;
				
				return c* _Hiding;
			}

			fixed4 frag4(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
				c.a *= 0.2;
				c.rgb *= c.a;
				
				return c* _Hiding;
			}

	ENDCG

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}
 
		Cull Off         //关闭背面剔除
		Lighting Off     //关闭灯光
		ZWrite Off       //关闭Z缓冲
		Blend One OneMinusSrcAlpha     //混合源系数one(1)  目标系数OneMinusSrcAlpha(1-one=0)
 
		Pass
		{
		CGPROGRAM
			#pragma vertex vert4
			#pragma fragment frag1
		ENDCG
		}

		Pass
		{
		CGPROGRAM
			#pragma vertex vert3
			#pragma fragment frag2
		ENDCG
		}

		Pass
		{
		CGPROGRAM
			#pragma vertex vert2
			#pragma fragment frag3
		ENDCG
		}

		Pass
		{
		CGPROGRAM
			#pragma vertex vert1
			#pragma fragment frag4
		ENDCG
		}

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
		ENDCG
		}
	}
}
