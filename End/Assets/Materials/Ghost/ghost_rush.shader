Shader "Hidden/ghost_rush"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Shine("Shine",Float) = 0.0
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        

    }

    CGINCLUDE

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

    v2f vert (appdata v)
    {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv = v.uv;

        return o;
    }

    sampler2D _MainTex;
    sampler2D _AlphaTex;
    float _Shine;
    float4 _motion;

    fixed4 frag (v2f i) : SV_Target
    {
        fixed4 col = tex2D(_MainTex, i.uv);

        col.rgb = col.rgb + _Shine;
        col.rgb *= col.a;
        return col;
    }

    fixed4 frag1 (v2f i) : SV_Target
    {
        fixed4 col = tex2D(_MainTex, i.uv);

        col.rgb = col.rgb + _Shine;
        col.rgb *= col.a;
        return col;
    }
    ENDCG
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Cull Off //ZWrite Off ZTest Always
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag        
            ENDCG
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag1        
            ENDCG
        }
    }
}
