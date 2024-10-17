using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using SpriteSlicer;
using UnityEngine;

public class Cut : MonoBehaviour
{
    Dictionary<Transform, (Vector2, Vector2)> records = new();

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SlicedObj"))
        {
            Vector2 contactPoint = collision.ClosestPoint(transform.position);
            RecordTargetStart(collision.transform, contactPoint);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("SlicedObj"))
        {
            if (collision.GetComponent<Jelly>().maxSlicesReached)
            {
                return;
            }

            Vector2 contactPoint = collision.ClosestPoint(transform.position);
            RecordTargetEnd(collision.transform, contactPoint);
            InformSliceManager(collision.transform);
        }
    }

    void RecordTargetStart(Transform _transform, Vector2 _start)
    {
        if (!records.ContainsKey(_transform))
        {
            records.Add(_transform, (_start, Vector2.zero));
        }
    }

    void RecordTargetEnd(Transform _transform, Vector2 _end)
    {
        if (records.ContainsKey(_transform))
        {
            var _start = records[_transform].Item1;
            records[_transform] = (_start, _end);
        }
    }

    void InformSliceManager(Transform _transform)
    {
        var pos = records[_transform];
        records.Remove(_transform);

        SliceManager.Instance.Slice(_transform, pos.Item1, pos.Item2);
    }
}
