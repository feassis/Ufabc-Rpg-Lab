using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [Header("Configurações do Painel")]
    public GameObject painelTutorial;
    public TutorialStep passoInicial;

    [Header("Referências da Direita")]
    public TextMeshProUGUI txtTitulo;
    public TextMeshProUGUI txtInstrucao;
    public VideoPlayer vPlayer;

    //Função para abrir o painel do tutorial
    public void AbrirTutorial()
    {
        painelTutorial.SetActive(true);
        
        //Sempre que abrir, mostra o primeiro tutorial por padrão
        if (passoInicial != null)
        {
            AtualizarDisplay(passoInicial);
        }
    }

    //Função para fechar o painel do tutorial
    public void FecharTutorial()
    {
        vPlayer.Stop();
        painelTutorial.SetActive(false);
    }

    //Funcao para atualizar o conteúdo do painel do tutorial, de acordo com os dados do ScriptableObject
    public void AtualizarDisplay(TutorialStep dados)
    {
        txtTitulo.text = dados.titulo;
        txtInstrucao.text = dados.instrucao;
        
        if (dados.videoExemplo != null)
        {
            vPlayer.clip = dados.videoExemplo;
            vPlayer.Play();
        }
    }
}