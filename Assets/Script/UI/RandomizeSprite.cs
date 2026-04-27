using UnityEngine;

public class RandomizeSprite : MonoBehaviour
{
    [Tooltip("Arraste seus 5 sprites de árvore para cá")]
    public Sprite[] treeSprites; 
    
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Pega o componente que desenha a imagem na tela
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Segurança: só faz o sorteio se a lista não estiver vazia
        if (treeSprites.Length > 0 && spriteRenderer != null)
        {
            // Sorteia um índice aleatório (de 0 até a quantidade de sprites)
            int randomIndex = Random.Range(0, treeSprites.Length);
            
            // Troca a imagem atual pela imagem sorteada
            spriteRenderer.sprite = treeSprites[randomIndex];
        }
    }
}