using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Control : MonoBehaviour
{
    Animator _anim;
    Rigidbody _rb;
    Vector2 _inputs;
    Vector2 _lookCoOrds;
    Vector2 _lookStorage;
    
    [SerializeField] GameObject _body;
    float _groundCheckDistance = 0.6f;
    [SerializeField] float _jumpStrength;
    private readonly float _lookSensitivity = 2;
    private readonly float _speedWalk = 3;
    private readonly float _speedRun = 5;
    float _speedCur;
    bool _isRun;
    bool _isInvert = false;

    GameObject _obj;
    RaycastHit _hit;
    private void Awake()
    {
        if (!_anim) _anim = GetComponent<Animator>();
        if (!_rb) _rb = GetComponentInParent<Rigidbody>();
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
        if (_isRun) _speedCur = _speedRun;
        else _speedCur = _speedWalk;

      
        if (IsGrounded())
        {
            if (Input.GetButton("Run")) _isRun = true;
            else
                _isRun = false;

            if (Input.GetButtonDown("Jump")) Jump();
        }
        if (Input.GetMouseButton(1)) Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;

        if (Input.GetMouseButtonDown(0))
            MouseFunctions();
        else if (Input.GetMouseButton(0))
            _obj.transform.parent = transform;
        if (Input.GetMouseButtonUp(0))
          _obj.transform.parent = null;

        _anim.SetBool("isRun", _isRun);
        _anim.SetBool("isGrounded", IsGrounded());
        _anim.SetFloat("speed", Mathf.Abs(_inputs.y));
    }

    private void FixedUpdate()
    {
        Walking(_speedCur);
    }

    private void LateUpdate()
    {
        if (!_isInvert)
            transform.rotation = Quaternion.Euler(_lookStorage.y * -1, _lookStorage.x, 0.0f);
        else 
            transform.rotation = Quaternion.Euler(_lookStorage.y * 1, _lookStorage.x, 0.0f);
    }

    public bool IsGrounded()
    {
        Debug.DrawRay(_body.transform.position, -transform.up * _groundCheckDistance, Color.red);
        return Physics.Raycast(_body.transform.position, -transform.up, _groundCheckDistance);
    }

    void Walking(float speed)
    {
        _lookStorage += _lookCoOrds * _lookSensitivity;
        _body.transform.Translate(((Vector3.right * _inputs.x) + (Vector3.forward * _inputs.y)) * speed * Time.deltaTime);
        _body.transform.rotation = Quaternion.Euler(0.0f, _lookStorage.x, 0.0f);
    }

    void Jump()
    {
        _rb.AddForce(Vector3.up * _jumpStrength, ForceMode.Impulse);
    }

    RaycastHit MouseFunctions()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            _obj = hit.collider.gameObject;
        }

        return hit;
    }
}
