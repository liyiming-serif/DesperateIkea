// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "FX/Title Fire" {
   Properties {
       _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
       _MainTex ("Particle Texture", 2D) = "white" {}
       _FallTex ("Particle Falloff", 2D) = "white" {}
       _InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
       _VertThresh("Vertical Thresh", Range(0, 2)) = 0.2
       _AlphaNoiseStr("Alpha Noise Strength", float) = 0.1
       _AlphaNoiseBlend("Alpha Noise Blend", float) = 0.5
       _AlphaMult("Alpha Mult", float) = 1.3
       _Dist("Distortion", float) = 0.1
   }

   Category {
       Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
       // Blend SrcAlpha One
       Blend One One
       // Blend One OneMinusSrcAlpha
       // Blend OneMinusDstColor One
       AlphaTest Greater .01
       ColorMask RGB
       // ZTest Always
       Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }

       BindChannels {
           Bind "Color", color
           Bind "Vertex", vertex
           Bind "TexCoord", texcoord
       }

       // ---- Fragment program cards
       SubShader {
           Pass {

               CGPROGRAM
               #pragma vertex vert
               #pragma fragment frag alpha:blend
               #pragma fragmentoption ARB_precision_hint_fastest
               #pragma multi_compile_particles

               #include "UnityCG.cginc"
               #include "SimplexNoise2D.hlsl"
               #include "shader_utils.hlsl"

               sampler2D _MainTex;
               sampler2D _FallTex;
               fixed4 _TintColor;

               struct appdata_t {
                   float4 vertex : POSITION;
                   fixed4 color : COLOR;
                   float2 texcoord : TEXCOORD0;
                   float2 center : TEXCOORD0;
               };

               struct v2f {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                
                UNITY_FOG_COORDS(3)
                float4 worldPos : TEXCOORD3;
                #ifdef SOFTPARTICLES_ON                
                float4 projPos : TEXCOORD4;
                #endif  
                UNITY_VERTEX_OUTPUT_STEREO
            };

               float4 _MainTex_ST;

               v2f vert (appdata_t v)
               {
                   v2f o;
                   o.vertex = UnityObjectToClipPos(v.vertex);
                   #ifdef SOFTPARTICLES_ON
                   o.projPos = ComputeScreenPos (o.vertex);                   
                   COMPUTE_EYEDEPTH(o.projPos.z);
                   #endif
                   o.worldPos = mul (unity_ObjectToWorld, v.vertex);
                   o.color = v.color;
                   o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
                   return o;
               }

               sampler2D _CameraDepthTexture;
               float _VertThresh;
               float _InvFade;
               float _AlphaNoiseStr, _AlphaNoiseBlend, _AlphaMult;
               float _Dist;

               fixed4 frag (v2f i) : COLOR
               {
                   #ifdef SOFTPARTICLES_ON
                   float sceneZ = LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos))));
                   float partZ = i.projPos.z;
                   float fade = saturate (_InvFade * (sceneZ-partZ));
                   i.color.a *= fade;
                   #endif

                   float sn1 = snoise(i.worldPos.xy * 1.5 - _Time.y * half2(-1, 1.3)*0.75 - snoise(- _Time.y*.75 + i.worldPos.xy * 2)*0.1) ;
                   sn1 = (lerp(1, sn1, 0.5) - 0.5) * _Dist;
                   
                   
                   float snf = snoise(i.worldPos * half2(1.5, 1) *0.5 - _Time.y * half2(.6, 2.6)* 1.2 - snoise(i.worldPos * half2(1.5, 1) *1.2 - _Time.y) * 0.2  + i.texcoord * 0.1);                   
                   float fader =  lerp(1, snf*_AlphaNoiseStr, _AlphaNoiseBlend)*_AlphaMult;
                   
                   fixed4 col = 2.0f * i.color * _TintColor * tex2D(_MainTex, saturate(i.texcoord + sn1));
                   
                   col.a *= fader;
                   col.rgb *= col.a;
                   col.rgb = lerp(col.rgb, grid1(col.rgb, half3(1, 1, 1)*3), .5);

                   
                   return col;
               }
               ENDCG 
           }
       }     

   }
}
