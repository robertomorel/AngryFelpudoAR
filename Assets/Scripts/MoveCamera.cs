using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform projetil, limiteEsquerdo, limiteDireito;

    private Vector3 novaPosicao;

    // Update is called once per frame
    void Update()
    {
        novaPosicao = transform.position;
        // -- A câmera deve acompanhar o projetil apenas pelo eixo X
        novaPosicao.x = projetil.position.x;
        // -- Limitar a posição da câmera nas direções esquerda (-x) e direita (+x)
        novaPosicao.x = Mathf.Clamp(novaPosicao.x, limiteEsquerdo.position.x, limiteDireito.position.x);
        transform.position = novaPosicao;
    }
}
