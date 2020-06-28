using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSub : PlayerController
{
    //???
    [SerializeField] float _speedCur = 2;
    float _carryDistance;
    private readonly float _carrySpeed = 15f;
    bool _isInvert = false;

    //grabber
    [SerializeField] private LayerMask grabLayer;
    [SerializeField] private GameObject grabbedObject = null;

    //spotlight
    public GameObject _spotlight;
    private bool isLocked = true;


    protected override void Start()
    {
        base.Start();
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    public override void RunUpdate()
    {
        //get input info
        _inputs = new Vector2(0f, Input.GetAxis("Vertical"));

        //get rotation info
        _lookCoOrds = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));


        //check for cursor unlock, make sure not grabbing anything
        if (Input.GetMouseButtonUp(1))
        {
            if (isLocked)
            {
                Cursor.lockState = CursorLockMode.Confined;
                isLocked = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                isLocked = true;
            }
        }

        //try to grab object...
        if (isLocked == false && Input.GetMouseButtonDown(0) && grabbedObject == null)
        {
            //see if grab object
            grabbedObject = GrabObject();
            if (grabbedObject != null)
            {
                //set as child
                grabbedObject.transform.parent = transform;
                //grabbedObject.transform.localPosition = grabbedObject.transform.position - transform.position;
                //turn off gravity from rigidbody?
                grabbedObject.GetComponent<Rigidbody>().useGravity = false;
            }
        }
        //release grabbed object...
        else if (Input.GetMouseButtonDown(0) && grabbedObject != null)
        {
            //turn on gravity from rigidbody?
            grabbedObject.transform.parent = grabbedObject.transform;
            grabbedObject.GetComponent<Rigidbody>().useGravity = true;
            grabbedObject = null;
        }
    }

    public override void RunFixedUpdate()
    {
        //grabbed object movement
        if(isLocked == false && grabbedObject != null)
        {
            if (_lookCoOrds != Vector2.zero)
            {
                grabbedObject.transform.localPosition += ((transform.right * _lookCoOrds.x) + (transform.up * _lookCoOrds.y)) * Time.fixedDeltaTime;
            }
        }
        //spotlight/sub rotation
        if(isLocked)
        {
            if (_lookCoOrds != Vector2.zero)
            {
                _lookStorage += _lookCoOrds * Time.deltaTime * _lookSensitivity;
                Debug.Log(_lookStorage);
            }
            _spotlight.transform.localRotation = Quaternion.Euler(_lookStorage.y * -1, _lookStorage.x, 0.0f);
            transform.localRotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0.0f, _lookStorage.x, 0.0f), _turnSpeed * Time.fixedDeltaTime);
        }

        //sub movement        
        if(_inputs == Vector2.zero)
        {
            //slow down sub by -velocity
            if(rb.velocity != Vector3.zero)
            {
                rb.AddForce(-rb.velocity * speed);
            }
        }
        else
        {
            rb.AddForce((transform.forward * _inputs.y + transform.right * _inputs.x).normalized * speed);
            if(rb.velocity.magnitude > maxVelocity)
            {
                rb.velocity = rb.velocity.normalized * maxVelocity;
            }
        }  
    }

    public override void RunLateUpdate()
    {
        //if(!_isInvert)
        //{
        //    _spot.transform.localRotation = Quaternion.Euler(_lookStorage.y * -1, _lookStorage.x, 0.0f);
        //    _subOne.transform.localRotation = Quaternion.Slerp(_subOne.transform.rotation, Quaternion.Euler(0.0f, _lookStorage.x, 0.0f), _turnSpeed * Time.deltaTime);
        //}
        //else
        //{
        //    _spot.transform.localRotation = Quaternion.Euler(_lookStorage.y * 1, _lookStorage.x, 0.0f);
        //    _subOne.transform.localRotation = Quaternion.Slerp(_subOne.transform.rotation, Quaternion.Euler(0.0f, _lookStorage.x, 0.0f), _turnSpeed * Time.deltaTime);
        //}
    }
    

    private GameObject GrabObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            //check for geod layer...
            if (grabLayer == (grabLayer | (1 << hit.collider.gameObject.layer)))
            {
                if (Vector3.Distance(hit.collider.transform.position, transform.position) < _carryDistance + 0.5f ||
                Vector3.Distance(hit.collider.transform.position, transform.position) > _carryDistance - 0.5f)
                {
                    Debug.Log("Grabbed Object: " + hit.collider.gameObject.name);
                    return hit.collider.gameObject;
                }
            }
        }

        return null;
    }

    public override void Walking(float speed, GameObject obj)
    {
        //_lookStorage += _lookCoOrds * _lookSensitivity;
        //..
    }
}
