Shader "Lava Flowing Shader/Unlit/Scrolling"
{
    Properties
    {
        _MainTex ("_MainTex RGBA", 2D) = "white" {}
        _LavaTex ("_LavaTex RGB", 2D) = "white" {}
        _ScrollSpeed ("Scroll Speed", Range(0, 1)) = 0.1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Lighting Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _LavaTex;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };

            float4 _MainTex_ST;
            float4 _LavaTex_ST;
            float _ScrollSpeed;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Calculate texture coordinates with scrolling effect
                float2 offset = float2(_Time.y * _ScrollSpeed, 0);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex) + offset;
                o.texcoord1 = TRANSFORM_TEX(v.texcoord, _LavaTex);

                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 tex = tex2D(_MainTex, i.texcoord);
                half4 tex2 = tex2D(_LavaTex, i.texcoord1);

                // Blend based on the alpha value of MainTex
                tex = lerp(tex2, tex, tex.a);

                return tex;
            }
            ENDCG
        }
    }
}
