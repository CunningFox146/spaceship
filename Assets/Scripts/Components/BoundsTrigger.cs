using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Components;
using UnityEngine;

public class BoundsTrigger : MonoBehaviour
{
    [SerializeField] private float _offset = 1f;
    private BoundsManager _bounds;
    private Camera _camera;

    void Start()
    {
        _bounds = BoundsManager.Inst;
        _camera = Camera.main;

        InitTrigger();
    }

    void OnTriggerExit(Collider col)
    {
        _bounds.Teleport(col.transform.root.gameObject);
    }

    // I still don't know how to cast a ray without target :\
    // SO: we'll be casting it on out trigger, at least for now
    private void InitTrigger()
    {
        Vector3 minPos = ScreenToWorldPos(Vector3.zero);
        Vector3 maxPos = ScreenToWorldPos(new Vector3(_camera.pixelWidth, _camera.pixelHeight, 0f));

        transform.localScale = new Vector3(maxPos.x - minPos.x + _offset, 10f, maxPos.z - minPos.z + _offset);

        _bounds.maxPos = maxPos;
        _bounds.minPos = minPos;
    }

    private Vector3 ScreenToWorldPos(Vector3 pos)
    {
        Ray ray = _camera.ScreenPointToRay(pos);
        if (Physics.Raycast(ray, out var hit, int.MaxValue, LayerMask.GetMask("Bounds"))) // Idk if it's better to use 1<<8 or GetMask()
        {
            return hit.point;
        }

        return Vector3.zero;
    }
}
