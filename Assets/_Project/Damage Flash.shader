Shader "Custom/Damage Flash"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
        _FlashColor ("Flash Color", Color) = (1, 0, 0, 1)
        _FlashAmount ("Flash Amount", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        LOD 100

         Pass
        {
           Blend SrcAlpha OneMinusSrcAlpha // Set the blend mode
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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            fixed4 _BaseColor;
            fixed4 _FlashColor;
            float _FlashAmount;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample texture and get base color
                fixed4 baseColor = _BaseColor;

                // Lerp between base color and flash color based on flash amount
                fixed4 finalColor = lerp(baseColor, _FlashColor, _FlashAmount);

                // Ensure alpha transparency, fade in based on _flashAmount
                finalColor.a = lerp(0,1,_FlashAmount);

               return finalColor;
            }
            ENDCG
        }
    }
}
