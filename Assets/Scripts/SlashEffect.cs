using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SlashEffect : MonoBehaviour
{
    public float captureSpeed = 0.00001f;
    public float removeSpeed = 0.025f;

    private LineRenderer lineRenderer;
    private bool canCapture = true;
    private bool canRemove = false;
    private bool lineFinished = false;

    public List<Vector3> pointList;
    // Start is called before the first frame update
    void Start()
    {
        pointList = new List<Vector3>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!lineFinished)
        {
            CaptureNewPoint();
            EndMouseCheck();
        }
        else
        {
            CheckForDestroy();
        }

        RemoveOldPoint();
        RenderLinePoints();

    }

    private void CheckForDestroy()
    {
        if (pointList.Count == 0)
        {
            Destroy(gameObject);
        }
    }

    private void CaptureNewPoint()
    {
        if (canCapture && Input.GetMouseButton((int)MouseButton.LeftMouse))
        {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            wp.z = 0;
            pointList.Add(wp);
            StartCoroutine(CaptureTimeOut());
            if (pointList.Count == 1)
            {
                StartCoroutine(RemoveTimeOut());
            }
        }
    }

    private void EndMouseCheck()
    {
        if (Input.GetMouseButtonUp((int)MouseButton.LeftMouse))
        {
            Debug.Log("MouseUp was called");
            lineFinished = true;
        }
    }

    IEnumerator CaptureTimeOut()
    {
        canCapture = false;
        yield return new WaitForSeconds(captureSpeed);
        canCapture = true;
    }

    private void RemoveOldPoint()
    {
        if (canRemove)
        {
            if (pointList.Count > 0)
            {
                pointList.RemoveAt(0);
                StartCoroutine(RemoveTimeOut());
            }
            else
            {
                StopCoroutine(RemoveTimeOut());
                canRemove = false;
            }
        }
    }

    IEnumerator RemoveTimeOut() 
    {
        canRemove = false;
        yield return new WaitForSeconds(removeSpeed);
        canRemove = true;
    }

    private void RenderLinePoints()
    {
        lineRenderer.positionCount = pointList.Count;
        lineRenderer.SetPositions(pointList.ToArray());
    }

    
}
