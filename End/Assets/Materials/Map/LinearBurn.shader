Shader "MyShader/LinearBurn"
{
    Properties
    {
        _Color("Color",Color) = (1,1,1,1)
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

    uniform sampler2D _MainTex;
    uniform float4 _MainTex_ST;
    fixed4 _Color;

    v2f vert (appdata v)
    {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv = TRANSFORM_TEX(v.uv,_MainTex);
        return o;
    }   

    fixed4 frag (v2f i) : SV_Target
    {
        fixed4 col = tex2D(_MainTex, i.uv)*_Color;
        //col=col.a*(col-1.0)+colDst;
        //col.rgb = col.rgb + colDst.rgb -1;
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
        }
        Blend Zero OneMinusSrcAlpha

        Pass
        {
            //ZWrite Off
            //Blend DstColor One
            //BlendOp RevSub
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag       
            ENDCG
        }
    }
}
