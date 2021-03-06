Shader "Unlit/Enemy"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HideInInspector] _Color1 ("RendererColor", Color) = (1,1,1,1)
        _Color("Tint",Color) = (1,1,1,1)
        _Shine("Shine",Range(0,0.3)) = 0.0
        _OutlineWidth("Outline Width",Range(0,100)) = 1
    }

    CGINCLUDE

    #include "UnityCG.cginc"


    struct appdata
    {
        float4 vertex : POSITION;
        float2 uv : TEXCOORD0;
        fixed4 color : COLOR;
    };
    struct v2f
    {
        float2 uv : TEXCOORD0;
        float4 vertex : SV_POSITION;
        fixed4 color : COLOR;
    };


    
    fixed4 _Color1;

    float _OffsetUV = 0;

    v2f vert (appdata v)
    {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv = v.uv;
        o.color = v.color;
        return o;
    }

    sampler2D _MainTex;
    sampler2D _AlphaTex;
    float _Shine;
    fixed4 _Color;
    float _OutlineWidth;
    float4 _MainTex_TexelSize;

    fixed4 frag (v2f i) : SV_Target
    {
        fixed4 col = tex2D(_MainTex, i.uv)*_Color*_Color1*i.color;


        col.rgb = col.rgb + _Shine;
        col.rgb*=col.a;
        col = clamp(col,float4(0,0,0,0),float4(1,1,1,1));
        return col;
    }
    ENDCG
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }
        Cull Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag       
            ENDCG
        }
        
    }
}