using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : Caractere
{

    float pontosVida;           // equivalente a sa�de do inimigo
    public int forcaDano;       // poder de dano

    Coroutine danoCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Player player = collision.gameObject.GetComponent<Player>();
            if(danoCoroutine == null) {
                danoCoroutine = StartCoroutine(player.DanoCaractere(forcaDano, 1.0f));
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            if(danoCoroutine != null) {
                StopCoroutine(danoCoroutine);
                danoCoroutine = null;
            }
        }
    }

    private void OnEnable() {
        ResetCaractere();
    }

    public override IEnumerator DanoCaractere(int dano, float intervalo) {
        while (true) {
            StartCoroutine(FlickerCaractere());
            pontosVida = pontosVida - dano;
            if(pontosVida <= float.Epsilon) {
                KillCaractere();
                break;
            }
            if(intervalo > float.Epsilon) {
                yield return new WaitForSeconds(intervalo);
            } else {
                break;
            }
        }
    }

    public override void ResetCaractere() {
        pontosVida = inicioPontosDano;
    }
}
