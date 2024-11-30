using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public string nomeCenaJogo; // Nome da cena do jogo

    public void NovoJogo()
    {
        SceneManager.LoadScene("Farm");
    }
}