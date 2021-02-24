using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecebeDano : MonoBehaviour
{

    public float pontosDeVida = 2;
    public float velocidadeParaDano = 3;
    public Sprite imagemMachucado;

    private float _pontosDeVidaAtuais;
    private float _velocidadeParaDanoQuadrado;
    private SpriteRenderer _sRenderer;

    private void Awake()
    {
        _sRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _pontosDeVidaAtuais = pontosDeVida;
        _velocidadeParaDanoQuadrado = velocidadeParaDano * velocidadeParaDano;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Arma"))
        {
            return;
        }    

        if(collision.relativeVelocity.sqrMagnitude < _velocidadeParaDanoQuadrado)
        {
            return;
        }

        _sRenderer.sprite = imagemMachucado;
        _pontosDeVidaAtuais--;

        if(_pontosDeVidaAtuais <= 0)
        {
            Matar();
        }
    }

    private void Matar()
    {
        _sRenderer.enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        //Destroy(this.gameObject);

        GetComponent<ParticleSystem>().Play();
    }

}
