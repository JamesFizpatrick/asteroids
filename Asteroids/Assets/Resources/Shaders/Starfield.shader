Shader "Unlit/Starfield"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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

            sampler2D _MainTex;
            float4 _MainTex_ST;

            #define NUM_LAYERS 4.0
            #define PI 3.1415

            float4 Rotate(float a)
            {
                float sinA = sin(a);
                float cosA = cos(a);    
                return float4(cosA, -sinA, sinA, cosA);
            }


            float Star(float2 uv, float flare)
            {
                float d = length(uv);
                float m = .05/d;
    
                float rays = max (0.0, 1.0 - abs(uv.x * uv.y * 1000.0));
                m += rays * flare;
    
                uv *= Rotate(PI/4.0);
    
                rays = max (0.0, 1.0 - abs(uv.x * uv.y * 1000.0));
                m += rays * 0.3 * flare;
       
                m *= smoothstep(0.5, 0.2, d);
                return m;
            }


            float Hash21(float2 p)
            {
                // Pseudo random
                p = frac(p * float2(145.234, 357.65));
                p += dot(p, p + 567.7);
                return frac(p.x * p.y);
            }


            float3 StarLayer(float2 uv)
            {
                float3 col = float3(0, 0, 0);
    
                float2 gv = frac(uv) - 0.5;
                float2 id = floor(uv);
           
                for (int y = -1; y <= 1; y++)
                {
                   for (int x = -1; x <= 1; x++)
                   {
                       float2 offset = float2(x, y);
           
                       float n = Hash21(id + offset);
                       float size = frac(n * 585.5);

                       float star = Star(gv - offset - float2(n, frac(n * 34.0)) + 0.5, smoothstep(0.9, 1.0, size) * 0.6);
           
                       float3 color = sin((float3(0.2, 0.3, 0.9)) * frac(n * 2356.8) * 134.5) * 0.5 + 0.5;
                       color = color * float3(1.0, 0.1, 1.0 + size);
           
                       star *= sin(_Time + 3.0 + n * 2 * PI) * 0.5 + 1.;
           
                       col += star * size * color;
                   }
                }
    
                return col;
            }


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f input) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, input.uv);               
                float t = _Time * 0.05;

                float3 color = float3(0, 0, 0);
        
                for (float i = 0.0; i < 1.0; i += 1.0/NUM_LAYERS)
                {
                    float depth = frac(i + t);
                    float scale = lerp(20.0, 0.5, depth);
                    float fade = depth * smoothstep(1.0, 0.9, depth);
                    color += StarLayer(input.uv * scale + i * 245.4) * fade; 
                }

                return fixed4(color, 1.0);
            }

            ENDCG
        }
    }
}
