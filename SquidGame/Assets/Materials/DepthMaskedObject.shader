Shader "Depth Masked Object" 
{
	Properties 
	{
		_Color ("Main Color", Color) = (1, 1, 1, 1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader 
	{
		Tags {"Queue"="Geometry+20" "RenderType"="Opaque" }
		LOD 200
		
//		Pass
//		{
//			Lighting On
//			SetTexture[_MainTex]
//			{
				//constantColor[_Color]
//				combine texture * constant
//			}

//		}


CGPROGRAM
#pragma surface surf Lambert

sampler2D _MainTex;
fixed4 _Color;

struct Input {
	float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	o.Albedo = c.rgb;
	o.Alpha = c.a;
}
ENDCG

	}
}
