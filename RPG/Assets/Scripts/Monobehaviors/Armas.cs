using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class Armas : MonoBehaviour
{

    public  GameObject  municaoPrefab;              // armazena o prefab da muni��o
    static  List<GameObject>    municaoPiscina;     // Pool de muni��o
    public  int     tamanhoPiscina;                 // tamanho da piscina
    public  float   velocidadeArma;                 // velocidade da muni��o

    public  bool    atirando;
    public  Animator    animator;

    Camera  cameraLocal;

    float   slopePositivo;
    float   slopeNegativo;

    enum Quadrante {
        Leste,
        Sul,
        Oeste,
        Norte
    }

    public void Awake() {
        if(municaoPiscina == null) {
            municaoPiscina = new List<GameObject>();
            for(int i = 0; i < tamanhoPiscina; i++) {
                GameObject municaoO = Instantiate(municaoPrefab);
                municaoO.SetActive(false);
                municaoPiscina.Add(municaoO);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !pauseMenu.GameIsPaused) {                // verifica antes se o jogo est� pausado
            atirando = true;
            DisparaMunicao();
        }
        UpdateEstado();
    }

    public GameObject SpawnMunicao(Vector3 posicao) {

        foreach (GameObject municao in municaoPiscina) {
            if (municao.activeSelf == false) {
                municao.SetActive(true);
                municao.transform.position = posicao;
                return municao;
            }
        }
        return null;
    }

    private void DisparaMunicao() {
        Vector3 posicaoMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject municao = SpawnMunicao(transform.position);
        if(municao != null) {
            Arco arcoScript = municao.GetComponent<Arco>();
            float duracaoTrajetoria = 1.0f / velocidadeArma;
            StartCoroutine(arcoScript.arcoTrajetoria(posicaoMouse, duracaoTrajetoria));
        }
    }

    private void OnDestroy() {
        municaoPiscina = null;
    }

    private void Start() {
        animator = GetComponent<Animator>();
        atirando = false;
        cameraLocal = Camera.main;
        Vector2 abaixoEsquerda = cameraLocal.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 acimaDireita = cameraLocal.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 acimaEsquerda = cameraLocal.ScreenToWorldPoint(new Vector2(0, Screen.height));
        Vector2 abaixoDireita = cameraLocal.ScreenToWorldPoint(new Vector2(Screen.width, 0));
        slopePositivo = PegaSlope(abaixoEsquerda, acimaDireita);
        slopeNegativo = PegaSlope(acimaEsquerda, abaixoDireita);
    }

    float PegaSlope(Vector2 ponto1, Vector2 ponto2) {
        return (ponto2.y - ponto1.y)/(ponto2.x - ponto1.x);
    }

    bool AcimaSlopePositivo(Vector2 posicaoEntrada) {
        Vector2 posicaoPlayer = gameObject.transform.position;
        Vector2 posicaoMouse = cameraLocal.ScreenToWorldPoint(posicaoEntrada);
        float interseccaoY = posicaoPlayer.y - (slopePositivo * posicaoPlayer.x);
        float entradaInterseccao = posicaoMouse.y - (slopePositivo * posicaoMouse.x);
        return entradaInterseccao > interseccaoY;
    }

    bool AcimaSlopeNegativo(Vector2 posicaoEntrada) {
        Vector2 posicaoPlayer = gameObject.transform.position;
        Vector2 posicaoMouse = cameraLocal.ScreenToWorldPoint(posicaoEntrada);
        float interseccaoY = posicaoPlayer.y - (slopeNegativo * posicaoPlayer.x);
        float entradaInterseccao = posicaoMouse.y - (slopeNegativo * posicaoMouse.x);
        return entradaInterseccao > interseccaoY;
    }

    Quadrante PegaQuadrante() {
        Vector2 posicaoMouse = Input.mousePosition;
        Vector2 posicaoPlayer = transform.position;
        bool acimaSlopePositivo = AcimaSlopePositivo(Input.mousePosition);
        bool acimaSlopeNegativo = AcimaSlopeNegativo(Input.mousePosition);
        if(!acimaSlopePositivo && acimaSlopeNegativo) {
            return Quadrante.Leste;
        }
        if (!acimaSlopePositivo && !acimaSlopeNegativo) {
            return Quadrante.Sul;
        }
        if (acimaSlopePositivo && !acimaSlopeNegativo) {
            return Quadrante.Oeste;
        } else {

        return Quadrante.Norte;
        }
    }

    void UpdateEstado() {
        if (atirando) {
            Vector2 vetorQuadrante;
            Quadrante quadranteEnum = PegaQuadrante();
            switch (quadranteEnum) {
                case Quadrante.Leste:
                    vetorQuadrante = new Vector2(1.0f, 0.0f);
                    break;
                case Quadrante.Sul:
                    vetorQuadrante = new Vector2(0.0f, -1.0f);
                    break;
                case Quadrante.Oeste:
                    vetorQuadrante = new Vector2(-1.0f, 0.0f);
                    break;
                case Quadrante.Norte:
                    vetorQuadrante = new Vector2(0.0f, 1.0f);
                    break;
                default:
                    vetorQuadrante = new Vector2(0.0f, 0.0f);
                    break;
            }
            animator.SetBool("Atirando", true);
            animator.SetFloat("AtiraX", vetorQuadrante.x);
            animator.SetFloat("AtiraY", vetorQuadrante.y);
            atirando = false;
        } else {
            animator.SetBool("Atirando", false); 
        }
    }
}
