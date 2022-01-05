Shader "Unlit/AlertRange_shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color",Color) = (0,0,0,0)
        _Angel("Angel",Range(-3.14,3.14)) = 0.0
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
    float4 _Color;
    float _Angel;

    v2f vert1 (appdata v)
    {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv = v.uv;

        return o;
    }

    fixed4 frag (v2f i) : SV_Target
    {
        fixed4 col = tex2D(_MainTex, i.uv)*_Color;


        float Pi = 3.1415926;
        float2 uv1 = i.uv - float2(0.5,0.5);
        float2 angel_uv = float2(0.5*cos(_Angel),0.5*sin(_Angel));
        col.a *= dot(normalize(angel_uv),normalize(uv1))>0.525322?1:0;
        //col *= angel;
        col.rgb *= col.a;


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