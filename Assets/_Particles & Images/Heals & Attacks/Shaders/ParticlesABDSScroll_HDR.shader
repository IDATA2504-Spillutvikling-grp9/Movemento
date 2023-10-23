Shader "The Developer/ParticlesABDSScroll_HDR"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR] _MainColor ("Main Color", Color) = (255,255,255,255)
        _MainSpeed ("Main Speed", Vector) = (0, .1, 0, 0)
        _NoiseTex ("Texture", 2D) = "white" {}
        _DistortionAmount ("Distortion Amount", Float) = 0.1
        _NoiseSpeed ("Noise Speed", Vector) = (0, .1, 0, 0)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        ZTest LEqual
        ZWrite Off
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR0;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 vertex_color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _MainColor;
            float2 _MainSpeed;
            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;
            float _DistortionAmount;
            float2 _NoiseSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv2 = TRANSFORM_TEX(v.uv2, _NoiseTex);
                o.vertex_color = v.color;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 mt_color = tex2D(
                                    _MainTex, 
                                    lerp(i.uv, i.uv + tex2D(_NoiseTex, i.uv2 + _Time.y * _NoiseSpeed), _DistortionAmount) +
                                    _Time.y * _MainSpeed
                                );

                fixed4 col = mt_color * _MainColor * i.vertex_color * mt_color.a;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
