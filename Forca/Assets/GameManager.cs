﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject letra;                // Prefab da letra
    public GameObject centro;               // Centro da tela

    private string palavraOculta = "";      // Palavra oculta a ser descoberta
    private string [] palavrasOcultas = new string [] {"carro", "elefante", "futebol"}; // Array de palavras ocultas
    
    private int tamanhoPalavraOculta;       // Tamanho desta palavra
    char []letrasOcultas;                   // Letras da palavra
    bool [] letrasDescobertas;              // Indicador das letras descobertas

    // Start is called before the first frame update
    void Start()
    {
        centro = GameObject.Find("centroDaTela");
        InitGame();
        InitLetras();
    }

    // Update is called once per frame
    void Update()
    {
        checkTeclado();
    }

    void InitLetras() {
        int numLetras = tamanhoPalavraOculta;
        for(int i = 0; i < numLetras; i++) {
            Vector3 novaPosicao;
            novaPosicao = new Vector3(centro.transform.position.x + ((i-numLetras/2.0f) * 80), centro.transform.position.y, centro.transform.position.z);
            GameObject l = (GameObject) Instantiate(letra, novaPosicao, Quaternion.identity);
            l.name = "letra" + (i + 1);
            l.transform.SetParent(GameObject.Find("Canvas").transform);
        }
    }

    void InitGame() {

        int numeroAleatorio = Random.Range(0, palavrasOcultas.Length);
        palavraOculta = palavrasOcultas[numeroAleatorio];
        tamanhoPalavraOculta = palavraOculta.Length;
        palavraOculta = palavraOculta.ToUpper();
        letrasOcultas = new char[tamanhoPalavraOculta];
        letrasDescobertas = new bool[tamanhoPalavraOculta];
        letrasOcultas = palavraOculta.ToCharArray();
    }

    void checkTeclado() {
        if (Input.anyKeyDown) {
            char letraTeclada = Input.inputString.ToCharArray()[0];
            int letraTecladaComoInt = System.Convert.ToInt32(letraTeclada);

            if(letraTecladaComoInt >= 97 && letraTecladaComoInt <= 122) {
                for (int i = 0 ; i < tamanhoPalavraOculta ; i++) {
                    if (!letrasDescobertas[i]) {
                        letraTeclada = System.Char.ToUpper(letraTeclada);
                        if (letraTeclada == letrasOcultas[i]) {
                            letrasDescobertas[i] = true;
                            GameObject.Find("letra" + (i + 1)).GetComponent<Text>().text = letraTeclada.ToString();
                        }
                    }
                }
            }
        }
    }

}
