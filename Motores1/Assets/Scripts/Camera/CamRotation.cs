using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CamRotation : MonoBehaviour
{
    float _mouseX, _mouseY;

    float _xRotation, _yRotation;

    [SerializeField] Transform _playerOrientation;
    [SerializeField] Transform _meshOrientation;

    protected delegate void VoideDelegate();
    VoideDelegate _rotateCam, _rotateMesh, _mouseInput;

    private void Awake()
    {
        _rotateCam = RotateCam;
        _rotateMesh = RotateMesh;
        _mouseInput = GetMouseInput;
    }

    private void Start()
    {
        GameManager.Instance.CamRotation = this;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameManager.Instance.OnCombatEnter += DisableRotation;
        GameManager.Instance.OnShopActive += DisableRotation;
    }

    private void Update()
    {
        #region Json commented
        //string json = File.ReadAllText(Application.dataPath + "/SensDataFile.json");
        //CamData data = JsonUtility.FromJson<CamData>(json);

        //if (data != null)
        //{
        //    _mouseX = Input.GetAxis("Mouse X") * Time.fixedDeltaTime * data._xSens;
        //    _mouseY = Input.GetAxis("Mouse Y") * Time.fixedDeltaTime * data._ySens;
        //}
        //else
        //{
        //    SensGuardarJSON();
        //} 
        #endregion

        _mouseInput();

        //_mouseX = Input.GetAxis("Mouse X") * Time.fixedDeltaTime * 90f;
        //_mouseY = Input.GetAxis("Mouse Y") * Time.fixedDeltaTime * 90f;

        //if (_mouseX != 0 || _mouseY != 0)
        //{
        //    _yRotation += _mouseX;
        //    _xRotation -= _mouseY;
        //}

        //_xRotation = Mathf.Clamp(_xRotation, -89f, 89f);

    }

    private void FixedUpdate()
    {
        //transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        _rotateCam();

    }

    private void LateUpdate()
    {
        _rotateMesh();
        //_playerOrientation.rotation = Quaternion.Euler(0, _yRotation, 0);
        //if (_meshOrientation != null)
        //    _meshOrientation.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
    }

    void GetMouseInput()
    {
        _mouseX = Input.GetAxis("Mouse X") * Time.fixedDeltaTime * 90f;
        _mouseY = Input.GetAxis("Mouse Y") * Time.fixedDeltaTime * 90f;

        if (_mouseX != 0 || _mouseY != 0)
        {
            _yRotation += _mouseX;
            _xRotation -= _mouseY;
        }

        _xRotation = Mathf.Clamp(_xRotation, -89f, 89f);
    }

    void RotateCam()
    {
        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
    }

    void RotateMesh()
    {
        _playerOrientation.rotation = Quaternion.Euler(0, _yRotation, 0);
    }

    void DisableRotation()
    {
        _rotateCam = delegate { };
        _rotateMesh = delegate { };
        _mouseInput = delegate { };

        GameManager.Instance.OnCombatEnter -= DisableRotation;
        GameManager.Instance.OnCombatExit += EnableRotation;

        GameManager.Instance.OnShopActive -= DisableRotation;
        GameManager.Instance.OnShopDisable += EnableRotation;
    }

    void EnableRotation()
    {
        _mouseInput = GetMouseInput;
        _rotateCam = RotateCam;
        _rotateMesh = RotateMesh;

        GameManager.Instance.OnCombatExit -= EnableRotation;
        GameManager.Instance.OnCombatEnter += DisableRotation;

        GameManager.Instance.OnShopDisable   -= EnableRotation;
        GameManager.Instance.OnShopActive += DisableRotation;
    }

    public void LookAtMe(Transform target)
    {
        _playerOrientation.LookAt(target.position + new Vector3(0f, _playerOrientation.position.y, 0f), Vector3.up);
        transform.LookAt(target.position + new Vector3(0f, transform.position.y, 0f), Vector3.up);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnCombatExit -= EnableRotation;
        GameManager.Instance.OnCombatEnter -= DisableRotation;
    }

    //private void SensGuardarJSON()
    //{
    //    CamData camDataScript = new CamData();
    //    camDataScript._xSens = 90;
    //    camDataScript._ySens = 90;

    //    string json = JsonUtility.ToJson(camDataScript, true);
    //    File.WriteAllText(Application.dataPath + "/SensDataFile.json", json);
    //}
}
