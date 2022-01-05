Shader "Hidden/Possessing"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Para("Para",Range(1,20))= 1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha
        //Tags { 
        //    "Queue" = "Transparent"
        //    "IgnoreProjector" = "True"
        //    "RenderType" = "Transparent"
        //}
        Pass
        {
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

            float _Para;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                //float2 target = float2(1.0,0.5);
                float2 uv1 = v.uv;
                float dir = v.uv.y - 0.5;
                dir = dir > 0 ? 1 : (dir < 0 ? -1 : 0);
                float A = (v.uv.y - 0.5) / pow((v.uv.x - 1),2);
                float offset = _Para - (1 - v.uv.x) * 1 ;
                offset = saturate(offset);
                uv1.y = 0.5 + dir * offset * 0.5;
                uv1.x = 1 - sqrt((uv1.y - 0.5)/A);
                o.uv = uv1;

                return o;
            }

            sampler2D _MainTex;
            

            fixed4 frag (v2f i) : SV_Target
            {
                

                fixed4 col = tex2D(_MainTex, i.uv);
                

                return col;
            }
            ENDCG
        }
    }
}
