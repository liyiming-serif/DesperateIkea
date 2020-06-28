Shader "Screen/fadescreen" {
  Properties {
    _MainTex("Texture", 2D) = "white" {}
    _AnimKnob("AnimKnob", Range(0, 1)) = 0
    _Type("Type", Range(0, 8)) = 0
    _DispType("Displacement Type", Range(0, 9)) = 0
    [Toggle] _DispOn("DispOn", Float) = 0
  }
  SubShader {
    // No culling or depth
    Cull Off ZWrite Off ZTest Always

    Pass {
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"

      #include "SimplexNoise2D.hlsl"

      #include "shader_utils.hlsl"

      struct appdata {
        float4 vertex: POSITION;
        float2 uv: TEXCOORD0;
      };

      struct v2f {
        float2 uv: TEXCOORD0;
        float4 vertex: SV_POSITION;
      };

      v2f vert(appdata v) {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv = v.uv;
        return o;
      }

      sampler2D _MainTex;
      float _Amt;
      float4 _Color1, _Color2;

      fixed _Type;
      fixed _AnimKnob;

      fixed2 mirror(fixed2 uv) {
        return lerp(fmod(-uv, fixed2(1, 1)), fmod(uv, fixed2(1, 1)), sign(sin(2*PI*uv)));
      }

      fixed _DispType;
      fixed _DispOn;

      fixed4 frag(v2f i): SV_Target {
        _Type = floor(_Type);
        _DispType = floor(_DispType);

        float pct = 0;
        fixed4 col = fixed4(1, 1, 1, 1);

        fixed2 norm_uv = i.uv;

        norm_uv.y *= _ScreenParams.y / _ScreenParams.x;

        if (_Type < 6) {
          if (_Type == 0) {
            pct = i.uv.xx;
          } else if (_Type == 1) {
            pct = norm_uv.yy;
          } else if (_Type == 2) {
            pct = distance(norm_uv, fixed2(0.5, 0.5 * _ScreenParams.y / _ScreenParams.x));
          } else if (_Type == 3) {
            pct = 1.0 - lerp(snoise(norm_uv.xx * 115.0), 1.0, 0.5);
          } else if (_Type == 4) {
            pct = 1.0 - lerp(snoise(norm_uv.yy * 115.0), 1.0, 0.5);
          } else {
            fixed res = 50;
            pct = 1.0 - lerp(snoise(grid1(norm_uv, fixed2(res, res)) * 15.0), 1.0, 0.5);
          }

          fixed2 disp = lerp(fixed2(0, 0), i.uv, step(0.1, _DispOn));
          if(_DispOn) {
                        if(_DispType == 0) {
                disp = sin(50 * i.uv * pct);
            } else if(_DispType == 1) {
                disp = sin(50 * pct);
            } else if(_DispType == 2) {
                disp = pct;
            } else if(_DispType == 3) {
                disp = pct * 1 / i.uv;
            } else if(_DispType == 4) {
                disp = pct * pct;
            } else if(_DispType == 5) {
                disp = i.uv * pct;
            } else if(_DispType == 6) {
                disp = fmod(pct, 0.2);
            } else if(_DispType ==7) {
                disp = fmod(norm_uv, 0.2);
            } else if(_DispType == 8) {
                disp = grid1(-i.uv / pct, fixed2(8, 8));
            } else {
                disp = sin(10 * distance(i.uv, fixed2(pct, pct)));
            }
          }



          col = tex2D(_MainTex, i.uv + ((1 - _AnimKnob) * disp)) * (1.0 - step(_AnimKnob, pct))  ;

        } else {
          fixed2 coord = i.uv;

          fixed2 disp = i.uv;
          float res = 15;
          float pct = 1.0 - lerp(snoise(grid1(norm_uv, fixed2(res, res)) * 15.0), 1.0, 0.5);
           disp = lerp(fixed2(0, 0), i.uv, step(0.1, _DispOn));

          if(_DispOn) {
                        if(_DispType == 0) {
                disp = sin(50 * i.uv * pct);
            } else if(_DispType == 1) {
                disp = sin(50 * pct);
            } else if(_DispType == 2) {
                disp = pct;
            } else if(_DispType == 3) {
                disp = pct * 1 / i.uv;
            } else if(_DispType == 4) {
                disp = pct * pct;
            } else if(_DispType == 5) {
                disp = i.uv * pct;
            } else if(_DispType == 6) {
                disp = fmod(pct, 0.2);
            } else if(_DispType ==7) {
                disp = fmod(norm_uv, 0.2);
            } else if(_DispType == 8) {
                disp = grid1(-i.uv / pct, fixed2(8, 8));
            } else {
                disp = sin(10 * distance(i.uv, fixed2(pct, pct)));
            }
          }



          if (_Type == 6) {
            coord = lerp((i.uv),  fixed2(0.5, 0.5), 1.0 - _AnimKnob);
            col = tex2D(_MainTex, coord + ((1 - _AnimKnob) * disp)) * _AnimKnob;
            col = lerp(fixed4(0, 0, 0, 0), col, 1 - step(1.0, coord.y));
          } else if (_Type == 7) {
            coord = (i.uv) + (1 - _AnimKnob) * fixed2(0., 5);
            col = tex2D(_MainTex, coord + ((1 - _AnimKnob) * disp)) * _AnimKnob;
            col = lerp(fixed4(0, 0, 0, 0), col, 1 - step(1.0, coord.y));
          } else if (_Type == 8) {
            // coord = (i.uv);
            col = tex2D(_MainTex, coord + (1 - _AnimKnob) * 0.5 * sin(distance(i.uv + disp, fixed2(0.5, 0.5) - _Time.y *0.7) * 30.30));

            // col = lerp(fixed4(0, 0, 0, 0), col, step(0, coord.y));
          }

        }
        return col;
      }
      ENDCG
    }
  }
}