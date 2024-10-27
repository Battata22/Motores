using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamDistance : MonoBehaviour
{
    [SerializeField] CamMovement _cam;
    [SerializeField] LayerMask _layerMask;

    //destino del raycast
    [SerializeField] Transform _lookingAt;

    //largo del rayo
    [SerializeField] float _rayDistance;

    //Vector3 _origin;

    //pinto de colision
    Vector3 _point;

    private void Awake()
    {
        _rayDistance = (transform.position - _lookingAt.position).magnitude;
    }

    //private IEnumerator Start()
    //{


    //    yield return new WaitForEndOfFrame();
    //    _cam = GameManager.Instance.Camera;
    //    //_rayDistance = (new Vector3(transform.position.x, transform.position.y + 2, transform.position.z) - _lookingAt.position).magnitude;
    //}

    private void Update()
    {
        //_origin = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        //guardar pos dle pj, en teoria pos del mesh pero no queria usar mesh de una porque soy especial

        //direccion del ray
        Vector3 _ray = (_lookingAt.position - transform.position).normalized;

        //crea rayo verde solo para el editor en unity
        Debug.DrawRay(transform.position, _ray * _rayDistance, Color.green);

        RaycastHit hit;

        //Crea Raycast desde el player al camera holder (pos default de la camara)
        //Cuando el raycast es cortado por la pared entre medio se pone a hacer magia

        // Crea Raycast     origen  / direccion   /devuelve algo/  largo del ray/  layer que lo puede cortar/
        if (Physics.Raycast(transform.position, _ray.normalized, out hit, _rayDistance, _layerMask))
        {
            //Al colicionar con una pared, guarda el liugar y activa el uso de punto para la posicion d ela camara
            _point = hit.point;
            _cam.usePoint = true;
        }
        else if (_cam.usePoint)
        {
            //Desactiva uso de punto
            _cam.usePoint = false;
        }
    }

    private void LateUpdate()
    {
        if (_cam.usePoint)
        {            
            //Da cordenadas del punto en la pared
            _cam.point = _point;
        }
    }
}
