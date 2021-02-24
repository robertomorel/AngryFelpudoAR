using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Resetador : MonoBehaviour
{

    public Rigidbody2D alvo;

    public float velocidadeParada = 0.025f;
    float velocidadeParadaQuadrada;

    SpringJoint2D mola;

    private void Awake()
    {
        mola = alvo.GetComponent<SpringJoint2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        velocidadeParadaQuadrada = velocidadeParada * velocidadeParada;
    }

    // Update is called once per frame
    void Update()
    {
        if (!mola && alvo.velocity.sqrMagnitude < velocidadeParadaQuadrada)
        {
            Resetar();
        }
    }

    void Resetar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Rigidbody2D>() == alvo)
        {
            Resetar();
        }
    }
}
