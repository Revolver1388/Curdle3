using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Experimental.GlobalIllumination;

public class p_underwaterSub : MonoBehaviour
{
    Animator _anim;
    [SerializeField] GameObject _spot;
    [SerializeField] Transform _grabber;
    Vector2 _inputs;
    Vector2 _lookCoOrds;
    Vector2 _lookStorage;
    Vector3 _objPos;
    [SerializeField] GameObject _body;
    [SerializeField] float _ballast;
    [SerializeField] float _carryDistance;
    [SerializeField] float _carrySpeed = 1f;
    private readonly float _lookSensitivity = 2;
    private readonly float _speedWalk = 3;
    private readonly float _speedRun = 5;
    float _speedCur;
    bool _isRun;
    bool _isInvert = false;

    GameObject _obj;
    private void Awake()
    {
        if (!_anim) _anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        _inputs = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _lookCoOrds = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        _objPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _grabber.localPosition = new Vector3(_objPos.x, _objPos.y, _carryDistance);
        
        if (_isRun) _speedCur = _speedRun;
        else _speedCur = _speedWalk;

        if (Input.GetButton("Run")) _isRun = true;
        else _isRun = false;

        if (Input.GetMouseButton(1)) Cursor.lockState = CursorLockMode.None;
        else Cursor.lockState = CursorLockMode.Locked;
       


        if (Input.GetMouseButtonDown(0))
            MouseFunctions();
        else if (Input.GetMouseButton(0))        
            _obj.transform.localPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + _carryDistance);
        
        _anim.SetBool("isRun", _isRun);
        _anim.SetFloat("speed", Mathf.Abs(_inputs.y));
    }

    private void FixedUpdate()
    {
        Walking(_speedCur);
        if (Input.GetAxis("Mouse ScrollWheel") > 0) _carryDistance += _carrySpeed * Time.deltaTime;
        else if (Input.GetAxis("Mouse ScrollWheel") < 0) _carryDistance -= _carrySpeed * Time.deltaTime;
    }

    private void LateUpdate()
    {

        if (!_isInvert)
        {
            _spot.transform.rotation = Quaternion.Euler(_lookStorage.y * -1, _lookStorage.x, 0.0f);
            if (Cursor.lockState == CursorLockMode.Locked)
                _body.transform.rotation = Quaternion.Slerp(_body.transform.rotation, Quaternion.Euler(0.0f, _lookStorage.x, 0.0f), 1 * Time.deltaTime);
        }
        else
        {
            _spot.transform.rotation = Quaternion.Euler(_lookStorage.y * 1, _lookStorage.x, 0.0f);
            if (Cursor.lockState == CursorLockMode.Locked)
                _body.transform.rotation = Quaternion.Slerp(_body.transform.rotation, Quaternion.Euler(0.0f, _lookStorage.x, 0.0f), 1 * Time.deltaTime);
        }
    }

    void Walking(float speed)
    {
        _lookStorage += _lookCoOrds * _lookSensitivity;
        _body.transform.Translate(((Vector3.right * _inputs.x) + (Vector3.forward * _inputs.y)) * speed * Time.deltaTime);
    }

    RaycastHit MouseFunctions()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (Vector3.Distance(hit.collider.transform.position,transform.position) < _carryDistance + 0.5f || Vector3.Distance(hit.collider.transform.position, transform.position) > _carryDistance - 0.5f)
            {
                _obj = hit.collider.gameObject;
                _objPos = hit.transform.position;
            }
        }
        return hit;
    }
}
