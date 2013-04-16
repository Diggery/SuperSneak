Shader "Rim/RimSolid" {
	Properties {
		_MainTex ("Base (RGB) TransGloss (A)", 2D) = "white" {}
		_Color ("Main Color", Color) = (1,1,1,1)
		_RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
		_RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
		_RimGlow ("Rim Glow", Range(0.0,1.0)) = 0.5
	}
	SubShader {
	
		Tags { "RenderType" = "Opaque" }
		
		CGPROGRAM
		#pragma surface surf Lambert
		
		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 viewDir;
		};
		
		sampler2D _MainTex;
		float4 _Color;
		float4 _RimColor;
		float _RimPower;
		float _RimGlow;
		
		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = tex.rgb;
			half4 rim = pow( 1.0 - dot (normalize(IN.viewDir), o.Normal), _RimPower ) * _RimColor * _RimPower;
			rim.a = 0;
			o.Albedo += rim;
			o.Albedo *= _Color;
			o.Emission += rim * _RimGlow;
			o.Gloss = tex.a;
		}
		
		ENDCG
	} 
	
	Fallback "Diffuse"
}

//half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
//vec4 rim = pow( 1.0 - dot( normal, vViewDir.xyz ), 1.5 ) * m_RimLighting * m_RimLighting.w;

