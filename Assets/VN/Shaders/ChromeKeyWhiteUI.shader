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
                // Pega a cor do pixel da imagem (com o fundo branco)
                fixed4 col = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
                
                // --- LÓGICA DO CHROMA KEY ---
                // Calcula a "distância" (diferença) entre a cor do pixel e o branco
                float d = distance(col.rgb, _KeyColor.rgb);
                
                // Cria uma máscara de Alpha (transparência) baseada na distância
                // Se a distância for menor que o Range, o Alpha será 0 (transparente)
                float alphaMask = smoothstep(_Range, _Range + _Fuzziness, d);
                
                // Aplica a máscara ao Alpha original da imagem
                col.a *= alphaMask;

                // Suporte para o sistema de clipping da UI (máscaras do Unity)
                col.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                
                // Retorna o pixel final com a transparência aplicada
                return col;
            }
            ENDCG
        }
    }
}