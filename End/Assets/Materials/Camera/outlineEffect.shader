Shader "Hidden/outlineEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        Blend One OneMinusSrcAlpha
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float4 _MainTex_PixelSize;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float cola = col.a;
                //
                float2 offset = _MainTex_PixelSize.xy * 2;
                fixed col1 = tex2D(_MainTex, i.uv + float2(-1,1)*offset)*fixed4(1,0,0,1);
                fixed col2 = tex2D(_MainTex, i.uv + float2(0,1)*offset)*fixed4(1,0,0,1);
                fixed col3 = tex2D(_MainTex, i.uv + float2(-1,1)*offset)*fixed4(1,0,0,1);
                fixed col4 = tex2D(_MainTex, i.uv + float2(-1,0)*offset)*fixed4(1,0,0,1);
                fixed col5 = tex2D(_MainTex, i.uv + float2(1,0)*offset)*fixed4(1,0,0,1);
                fixed col6 = tex2D(_MainTex, i.uv + float2(-1,-1)*offset)*fixed4(1,0,0,1);
                fixed col7 = tex2D(_MainTex, i.uv + float2(0,-1)*offset)*fixed4(1,0,0,1);
                fixed col8 = tex2D(_MainTex, i.uv + float2(1,-1)*offset)*fixed4(1,0,0,1);
                
                //cola = 1 - cola;
                cola *= (col1+col2+col3+col4+col5+col6+col7+col8)>=0.1?1:0;

                //return fixed4(1,0,0,1)*col;
                return fixed4(1,0,0,1)*cola;
            }
            ENDCG
        }
    }
}
