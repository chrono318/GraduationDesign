Shader "MyShader/LinearBurn"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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


    v2f vert (appdata v)
    {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv = v.uv;
        return o;
    }

    sampler2D _MainTex;
    fixed4 _Color;

    fixed4 frag (v2f i) : SV_Target
    {
        fixed4 col = tex2D(_MainTex, i.uv)*_Color;
        col.rgb*=col.a;
        //col.rgb = lerp(fixed3(1,1,1),col.rgb,p)*col.a;
        return col;
    }
    fixed4 frag1 (v2f i) : SV_Target
    {
        fixed4 col = tex2D(_MainTex, i.uv)*_Color;
        col.rgb = fixed3(1,1,1);
        col.rgb*=col.a;
        //col.rgb = lerp(fixed3(1,1,1),col.rgb,p)*col.a;
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
        //Blend One OneMinusSrcAlpha
        //BlendOp Add

        Pass
        {
            ZWrite Off
            Blend DstColor One
            BlendOp RevSub
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag       
            ENDCG
        }
        //Pass
        //{
        //    Blend One One
        //    BlendOp RevSub
        //    CGPROGRAM
        //    #pragma vertex vert
        //    #pragma fragment frag1       
        //    ENDCG
        //}
    }
}
