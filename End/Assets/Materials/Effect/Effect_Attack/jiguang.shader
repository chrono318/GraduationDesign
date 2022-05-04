Shader "Hidden/jiguang"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("NoiseTex", 2D) = "white" {}
        _Intensity("Intensity",Float) = 1
        _Speed("Speed",Float) = 1
        [HDR]_Color("Color",Color) = (1,1,1,1)
    }
    SubShader {
		Tags { "RenderType"="Transparent"  "IgnoreProjector"="True"  "Queue"="Transparent"}
		LOD 200
		Blend One OneMinusSrcAlpha
		CGPROGRAM
		#pragma surface surf NoLight vertex:vert alpha noforwardadd

		//光照方程，名字为Lighting接#pragma suface后的光照方程名称 
  		//lightDir :顶点到光源的单位向量
		//viewDir  :顶点到摄像机的单位向量   
		//atten	   :关照的衰减系数 
  		float4 LightingNoLight(SurfaceOutput s, float3 lightDir,half3 viewDir, half atten) 
  		{ 
  		 	float4 c ; 
  		 	c.rgb =  s.Albedo;
  			c.a = s.Alpha; 
			return c; 
  		}

		sampler2D _MainTex;
		fixed4 _SelfCol;
		sampler2D _NoiseTex;
        float _Intensity;
        float _Speed;
        fixed4 _Color;

		struct Input 
		{
			float2 uv_MainTex;
			float4 vertColor;
		};

		void vert(inout appdata_full v, out Input o)
		{
			o.vertColor = v.color;
			o.uv_MainTex = v.texcoord;
		}
		float2 random2(float2 p)
		{
			return frac(sin(float2(dot(p,float2(117.12,341.7)),dot(p,float2(269.5,123.3))))*43458.5453);
		}
		void surf (Input IN, inout SurfaceOutput o) 
		{
			float2 uv1 = IN.uv_MainTex + float2( - _Time.x * _Speed , 0);
			float2 uv = uv1;
			uv *= 6.0; //Scaling amount (larger number more cells can be seen)
			float2 iuv = floor(uv); //gets integer values no floating point
			float2 fuv = frac(uv); // gets only the fractional part
			float minDist = 1.0;  // minimun distance
			for (int y = -1; y <= 1; y++)
			{
				for (int x = -1; x <= 1; x++)
				{
					// Position of neighbour on the grid
					float2 neighbour = float2(float(x), float(y));
					// Random position from current + neighbour place in the grid
					float2 pointv = random2(iuv + neighbour);
					// Move the point with time
					pointv = 0.5 + 0.5*sin(_Time.z + 6.2236*pointv);//each point moves in a certain way
																		// Vector between the pixel and the point
					float2 diff = neighbour + pointv - fuv;
					// Distance to the point
					float dist = length(diff);
					// Keep the closer distance
					minDist = min(minDist, dist);
				}
			}
			float noise =  minDist * minDist;
			noise = noise * _Intensity + 1 - _Intensity;
			half4 c = tex2D (_MainTex, uv1) * noise ;
			o.Alpha = c.a * IN.vertColor.a * _Color;
			o.Albedo = IN.vertColor.rgb;
		}

		
		ENDCG
	} 
}
