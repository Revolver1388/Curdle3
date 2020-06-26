﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleMove : MonoBehaviour
{
    [SerializeField] private string group = "groupName";
    public string GetGroup()
    {
        return group;
    }

    private Vector3 startPos;
    [SerializeField] private Vector3 endPos;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float gizmosSize = 1f;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + endPos, gizmosSize);
    }

    private void Start()
    {
        startPos = transform.position;
        endPos += startPos;
        Debug.Log("S: " + startPos + " E: " + endPos);
    }

    public void NextPosition()
    {
        if (c != null)
        {
            StopCoroutine(c);
        }

        c = StartCoroutine(Move(endPos));
    }

    public void ResetPosition()
    {
        if(c != null)
        {
            StopCoroutine(c);
        }
        
        c = StartCoroutine(Move(startPos));
    }

    private Coroutine c = null;

    IEnumerator Move(Vector3 destination)
    {
        Vector3 begin = transform.position;
        float count = 0f;
        while(begin != destination)
        {
            count += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(begin, destination, count);
            yield return null;
        }
    }
}
