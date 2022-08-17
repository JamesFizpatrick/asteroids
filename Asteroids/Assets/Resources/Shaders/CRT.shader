Shader "Custom/CRT"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

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

			uniform float Bend;
			uniform float ScanlineSize1;
			uniform float ScanlineSpeed1;
			uniform float ScanlineSize2;
			uniform float ScanlineSpeed2;
			uniform float ScanlineAmount;
			uniform float VignetteSize;
			uniform float VignetteSmoothness;
			uniform float VignetteEdgeRound;
			uniform float NoiseSize;
			uniform float NoiseAmount;
			uniform half2 RedOffset;
			uniform half2 GreenOffset;
			uniform half2 BlueOffset;

			half2 crt_coords(half2 uv, float bend)
			{
				//Switch to -1;1 space from 0;1 space
				uv -= 0.5;
				uv *= 2.0;

				// Add curvature
				uv.x *= 1.0 + pow(abs(uv.y) / bend, 2.0);
				uv.y *= 1.0 + pow(abs(uv.x) / bend, 2.0);

				//Return to 0;1 space
				uv /= 2.0;
				return uv + 0.5;
			}


			float vignette(half2 uv, float size, float smoothness, float edgeRounding)
			{
				// switch to 0,5; 0,5 coords form 0;1 coords
				uv -= 0.5;

				// appy vignette 
				uv *= size;
				float amount = sqrt(pow(abs(uv.x), edgeRounding) + pow(abs(uv.y), edgeRounding));
				amount = 1.0 - amount;
				return smoothstep(0.0, smoothness, amount);
			}


			float scanline(half2 uv, float lines, float speed)
			{
				return sin(uv.y * lines + _Time * 10.0 * speed);
			}


			float random(half2 uv)
			{
				// Pseudo random
				return frac(sin(dot(uv, half2(32.3331, 23.5523))) * 45675.345736 * sin(_Time * 10.0 * 0.03));
			}


			float noise(half2 uv)
			{
				half2 i = floor(uv);
				half2 f = frac(uv);

				float a = random(i);
				float b = random(i + half2(1.0, 0.0));
				float c = random(i + half2(0.0, 1.0));
				float d = random(i + half2(1.0, 1.0));

				half2 u = smoothstep(0.0, 1.0, f);

				return lerp(a, b, u.x) + (c - a) * u.y * (1. - u.x) + (d - b) * u.x * u.y;
			}


			fixed4 frag (v2f i) : SV_Target
			{
				// Retrieve UV with CRT-like bend
				half2 uv = crt_coords(i.uv, Bend);
				fixed4 col;

				// Apply color offset
				col.r = tex2D(_MainTex, uv + RedOffset).r;
				col.g = tex2D(_MainTex, uv + GreenOffset).g;
				col.b = tex2D(_MainTex, uv + BlueOffset).b;
				col.a = tex2D(_MainTex, uv).a;

				// Create scanlines
				float s1 = scanline(i.uv, ScanlineSize1, ScanlineSpeed1);
				float s2 = scanline(i.uv, ScanlineSize2, ScanlineSpeed2);

				// Add scanlines
				col = lerp(col, fixed(s1 + s2), ScanlineAmount);

				// Add noise
				col = lerp(col, fixed(noise(i.uv * NoiseSize)), NoiseAmount);

				// Add vignette
				col *= vignette(i.uv, VignetteSize, VignetteSmoothness, VignetteEdgeRound);

				return col;
			}

			ENDCG
		}
	}
}