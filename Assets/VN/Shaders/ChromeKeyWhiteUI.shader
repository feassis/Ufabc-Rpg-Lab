Shader "Custom/ChromaKeyWhiteUI"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        
        // Parâmetros do Chroma Key
        _KeyColor ("Key Color (Branco)", Color) = (1,1,1,1) // Cor a remover (Branco por padrão)
        _Range ("Range (Tolerância)", Range(0, 1)) = 0.01      // Quão próximo do branco deve ser removido
        _Fuzziness ("Fuzziness (Suavidade)", Range(0, 1)) = 0.1 // Suavidade da borda
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha // Habilita a transparência

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
            };

            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            sampler2D _MainTex;
            
            // Variáveis do Chroma Key
            fixed4 _KeyColor;
            float _Range;
            float _Fuzziness;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.worldPosition = IN.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                // 1. Pega a cor PURA da imagem (onde o branco ainda é branco)
                fixed4 tex = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd);
                
                // 2. LÓGICA DO CHROMA KEY (Calcula a máscara na cor original)
                float d = distance(tex.rgb, _KeyColor.rgb);
                float alphaMask = smoothstep(_Range, _Range + _Fuzziness, d);
                
                // 3. Agora aplica o Tint do Unity (o cinza ou branco) e a transparência
                fixed4 col = tex * IN.color;
                col.a *= alphaMask;
                
                // Suporte para o sistema de clipping da UI
                col.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                
                return col;
            }
            ENDCG
        }
    }
}