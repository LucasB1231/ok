using Unity.VisualScripting;
using UnityEngine;

public class PlantGrowth : MonoBehaviour
{
    public GameObject estagio1;
    public GameObject estagio2;
    public GameObject estagio3;
    public GameObject spawnlocal;
    public GameObject dropItem; // Item que será instanciado após a coleta

    public float tempotroca = 3f;
    private float tempoAtual = 0f;
    private int estagioAtual = 0; // 0: Estágio 1, 1: Estágio 2, 2: Estágio 3
    private GameObject objetoAtual;
    private bool podeColetar = false;

    private void Start()
    {
        Vector3 local = spawnlocal.transform.position;
        objetoAtual = Instantiate(estagio1, local, Quaternion.identity); // Inicia no estágio 1
    }

    private void Update()
    {
        if (estagioAtual < 3)
        {
            tempoAtual += Time.deltaTime;
            if (tempoAtual >= tempotroca)
            {
                TrocarEstagio();
                tempoAtual = 0f;
            }
        }
        else if (podeColetar && Input.GetKeyDown(KeyCode.E)) // Verifica se pode coletar e se "E" foi pressionado
        {
            Coletar();
        }
    }

    private void TrocarEstagio()
    {
        if (estagioAtual <= 1)
        {
            Destroy(objetoAtual);
        }

        estagioAtual++;
        Vector3 local = spawnlocal.transform.position;

        if (estagioAtual == 1)
        {
            objetoAtual = Instantiate(estagio2, local, Quaternion.identity);
        }
        else if (estagioAtual == 2)
        {
            objetoAtual = Instantiate(estagio3, local, Quaternion.identity);
        }
        else
        {
            Debug.Log("Crescimento completo.");
        }
    }

    private void Coletar()
    {
        Debug.Log("Objeto coletado!");
        Destroy(objetoAtual); // Destrói o objeto do estágio 3
        Instantiate(dropItem, spawnlocal.transform.position, Quaternion.identity); // Instancia o dropItem no local do spawn
        podeColetar = false; // Desabilita a coleta após instanciar o item

        Destroy(this.gameObject, 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && estagioAtual == 3) // Verifica se o Player entrou no trigger e se está no estágio 3
        {
            podeColetar = true;
            Debug.Log("Pressione 'E' para coletar.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            podeColetar = false;
        }
    }
}
