using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrastaProjetil : MonoBehaviour
{

    private bool _clicou;

    private SpringJoint2D _mola;

    private Transform _estilingue;

    public float esticadaMaxima = 3.0f;

    public float esticadaMaximaQuadrada;

    private Rigidbody2D _rb;

    private CircleCollider2D _colisor;

    private float _medidaCirculo;

    /*
     Desenha uma linha e cria um raio
         */
    private Ray _raioParaMouse, _raioEstilingueFrente;

    private Vector2 _velocidadeAnterior;

    // -- Irão receber as duas imagens do estilingue
    public LineRenderer linhaFrente, linhaTras;

    private void Awake()
    {
        _mola = GetComponent<SpringJoint2D>();
        _rb = GetComponent<Rigidbody2D>();
        _colisor = GetComponent<CircleCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _estilingue = _mola.connectedBody.transform; // Transform do estilingueTras
        esticadaMaximaQuadrada = esticadaMaxima * esticadaMaxima;

        _raioParaMouse = new Ray(_estilingue.position, Vector3.zero);
        _raioEstilingueFrente = new Ray(linhaFrente.transform.position, Vector3.zero);

        _medidaCirculo = _colisor.radius;

        ConfiguraLinha();
    }

    // Update is called once per frame
    void Update()
    {
        if (_clicou)
        {
            Arrastar();
        }

        if (_mola)
        {
            /*
             O SpringJoint2D da mola pode fazer com que o projetil desacelere quando passar pela metade do caminho.
             Para evitar que isso aconteça, quando começar a desaceleração, destruímos a mola e settamos
             a velocidade do rigidbody para a velocidade imediatamente anterior (antes de desacelerar)
             */
            if (!_rb.isKinematic && _velocidadeAnterior.sqrMagnitude > _rb.velocity.sqrMagnitude)
            {
                Destroy(_mola);
                _rb.velocity = _velocidadeAnterior;
            }

            if (!_clicou)
            {
                _velocidadeAnterior = _rb.velocity;
            }

            AtualizaLinha();
        }
        else
        {

        }
    }

    void ConfiguraLinha()
    {
        linhaFrente.SetPosition(0, linhaFrente.transform.position + Vector3.forward * -0.03f); // -- Seta local da posição 0 - Inicial da linhaFrente
        linhaTras.SetPosition(0, linhaTras.transform.position + Vector3.forward * -0.01f); // -- Seta local da posição 0 - Inicial da linhaTras

        linhaFrente.sortingLayerName = "Front";
        linhaTras.sortingLayerName = "Front";

        linhaFrente.sortingOrder = 3;
        linhaTras.sortingOrder = 1;
    }

    void AtualizaLinha()
    {
        Vector2 estilingueParaProjetil = transform.position - linhaFrente.transform.position;
        _raioEstilingueFrente.direction = estilingueParaProjetil;

        Vector3 pontoDeAmarra = _raioEstilingueFrente.GetPoint(estilingueParaProjetil.magnitude + _medidaCirculo);

        pontoDeAmarra.z = -0.03f;
        linhaFrente.SetPosition(1, pontoDeAmarra);
        pontoDeAmarra.z = -0.01f;
        linhaTras.SetPosition(1, pontoDeAmarra);
    }

    private void OnMouseDown()
    {
        _clicou = true;
        _mola.enabled = false;
    }

    private void OnMouseUp()
    {
        _clicou = false;
        _mola.enabled = true;
        _rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void Arrastar()
    {
        /*
         ScreenToWorldPoint: Transforma uma coordenada de tela (+x -> -x && +y -> -y) 
         em uma coordenada de mundo (zerando o 'posicion', o elemento vai para o centro).
         Usando como parâmetro a posição do mouse.
         */
        Vector3 posicaoMouseMundo = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 estilingueParaMouse = posicaoMouseMundo - _estilingue.position;

        // -- sqrMagnitude: Magnitude (distância) ao quadrado
        if (estilingueParaMouse.sqrMagnitude > esticadaMaximaQuadrada)
        {
            _raioParaMouse.direction = estilingueParaMouse;
            posicaoMouseMundo = _raioParaMouse.GetPoint(esticadaMaxima);
        }

        posicaoMouseMundo.z = -0.02f; // -- Caso não existisse esta linha, iria ficar em z = -10, ou seja, por detrás do plano.
        transform.position = posicaoMouseMundo;

    }
}
