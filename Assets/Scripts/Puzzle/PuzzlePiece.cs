using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PuzzlePiece : MonoBehaviour
{
    [SerializeField] private string group = "groupName";
    public string GetGroup()
    {
        return group;
    }
    [SerializeField] private bool isActive = false;
    public bool GetIsActive()
    {
        return isActive;
    }
    [SerializeField] private LayerMask stoneLayer;


    public static event Action<string> activated;

    private void OnCollisionEnter(Collision collision)
    {
        if(stoneLayer == (stoneLayer | (1 << collision.gameObject.layer)))
        {
            isActive = true;
            activated?.Invoke(group);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (stoneLayer == (stoneLayer | (1 << collision.gameObject.layer)))
        {
            isActive = false;
            activated(group);
        }
    }
}
