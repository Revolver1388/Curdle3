using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Build.Pipeline.Tasks;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Experimental.GlobalIllumination;

public class p_underwaterSub : MonoBehaviour
{
    public static p_underwaterSub _instance;
    public enum Submarine {  one, two};
    public Submarine _whichSub;

    Animator _anim;
    
    GameObject _obj;

    Vector2 _inputs;
    Vector2 _lookCoOrds;
    Vector2 _lookStorage;
    Vector3 _objPos;

    [SerializeField] GameObject[] _cams;
    [SerializeField] public GameObject _spot;
    [SerializeField] Transform _grabber;
    [SerializeField] public GameObject _subOne;
    [SerializeField] GameObject _subTwo;
    [SerializeField] float _carryDistance;
    [SerializeField] float _carrySpeed = 1f;

    bool _isRun;
    [SerializeField] float _speedCur;
    bool _isInvert = false;
    bool _isSubOne = true;
    [SerializeField] float _ballast;
    [SerializeField] float _turnSpeed;
    private readonly float _lookSensitivity = 2;
    private readonly float _speedWalk = 3;
    private readonly float _speedRun = 5;


    private void Awake()
    {
        if (!_anim) _anim = GetComponent<Animator>();
        if(!_instance) _instance = this;
        else if (_instance != this) Destroy(_instance); 
        
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
        _anim.SetBool("isRun", _isRun);
        _anim.SetFloat("speed", Mathf.Abs(_inputs.y));
        _anim.SetBool("switchSub", _isSubOne);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_whichSub == Submarine.one) _whichSub = Submarine.two;
            else
                _whichSub = Submarine.one;
        }
        switch (_whichSub)
        {
            case Submarine.one:
                this.transform.parent = _subOne.transform;
               
                _isSubOne = true;
                //_grabber.localPosition = new Vector3(_objPos.x, _objPos.y, _carryDistance);


                //if (_isRun) _speedCur = _speedRun;
                //else _speedCur = _speedWalk;

                if (Input.GetButton("Run")) _isRun = true;
                else _isRun = false;

                if (Input.GetMouseButton(1)) Cursor.lockState = CursorLockMode.None;
                else Cursor.lockState = CursorLockMode.Locked;

                if (Input.GetMouseButtonDown(0))
                {
                    MouseFunctions();
                    if(_obj)
                    _obj.transform.parent = this.transform;
                }
                else if (Input.GetMouseButton(0) && _obj)
                    _obj.transform.localPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + _carryDistance);
                else if (Input.GetMouseButtonUp(0) && _obj) { _obj.transform.parent = null; _obj = null; }
               
                break;

            case Submarine.two:
                transform.parent = _subTwo.transform;
                transform.localRotation = Quaternion.identity;

                _isSubOne = false;


                break;
            default:
                break;
        }
        
    }

    private void FixedUpdate()
    {

        switch (_whichSub)
        {
            case Submarine.one:
                Walking(_speedCur,_subOne);
                if (Input.GetAxis("Mouse ScrollWheel") > 0) _carryDistance += _carrySpeed * Time.deltaTime;
                else if (Input.GetAxis("Mouse ScrollWheel") < 0) _carryDistance -= _carrySpeed * Time.deltaTime;
                break;

            case Submarine.two:
                Walking(_speedCur, _subTwo);
                transform.localPosition = new Vector3(0, 0.7360001f, 0.5599999f);

                break;
            default:
                break;
        }

    }

    private void LateUpdate()
    {
        switch (_whichSub)
        {
            case Submarine.one:
                if (!_isInvert)
                {
                    _spot.transform.localRotation = Quaternion.Euler(_lookStorage.y * -1, _lookStorage.x, 0.0f);
                    _subOne.transform.localRotation = Quaternion.Slerp(_subOne.transform.rotation, Quaternion.Euler(0.0f, _lookStorage.x, 0.0f), _turnSpeed * Time.deltaTime);
                }
                else
                {
                    _spot.transform.localRotation = Quaternion.Euler(_lookStorage.y * 1, _lookStorage.x, 0.0f);
                    _subOne.transform.localRotation = Quaternion.Slerp(_subOne.transform.rotation, Quaternion.Euler(0.0f, _lookStorage.x, 0.0f), _turnSpeed * Time.deltaTime);
                }
                break;
            case Submarine.two:
                _spot.transform.localRotation = Quaternion.identity;
                _subTwo.transform.localRotation = Quaternion.Euler(0.0f, _lookStorage.x, 0.0f);

                break;
            default:
                break;
        }

    }

    void Walking(float speed, GameObject obj)
    {
        _lookStorage += _lookCoOrds * _lookSensitivity;
        if (_whichSub == Submarine.one)
        {
           // _lookStorage.y = Mathf.Clamp(_lookStorage.y, -85, 85);
           // _lookStorage.x = Mathf.Clamp(_lookStorage.x, -160, 160);
        }
        obj.transform.Translate(((Vector3.right * _inputs.x) + (Vector3.forward * _inputs.y)) * speed * Time.deltaTime);
        if (Input.GetButton("BallastUp")) obj.transform.Translate(Vector3.up *_ballast * Time.deltaTime);
        else if(Input.GetButton("BallastDown")) obj.transform.Translate(Vector3.up * -_ballast * Time.deltaTime);
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
                print(hit.collider.gameObject.name);
            }
        }
        return hit;
    }
}
