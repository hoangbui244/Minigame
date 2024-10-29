using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace SpriteSlicer
{
    public class SliceEngine
    {
        Transform target;
        Transform second;
        Vector2 startPos;
        Vector2 endPos;

        float alpha = 5.0f;
        bool suspend = false;

        Vector2 velocity;
        float angularVelocity;

        Rigidbody2D r1;
        Rigidbody2D r2;
        PolygonCollider2D targetCollider;
        Jelly targetJelly;

        public SliceEngine(Transform _target, Vector2 _startPos, Vector2 _endPos)
        {
            target = _target;
            startPos = _startPos;
            endPos = _endPos;

            r1 = target.GetComponent<Rigidbody2D>();
            targetCollider = target.GetComponent<PolygonCollider2D>();
            targetJelly = target.GetComponent<Jelly>();
        }

        public void Slice()
        {
            StopMovement();
            CalcAccuratePoints();
            UpdateCollider();

            if (suspend) return;
            UpdateShader();
            UpdateRigidBody();
        }

        void StopMovement()
        {
            velocity = r1.velocity;
            r1.velocity = Vector2.zero;

            angularVelocity = r1.angularVelocity;
            r1.angularVelocity = 0;
        }

        void CalcAccuratePoints()
        {
            var dir = endPos - startPos;
            var extentedStartPoint = startPos - (alpha * dir);
            var extentedEndPoint = endPos + (alpha * dir);

            var newPoint = GetHitPoint(extentedStartPoint, -dir);
            startPos = (newPoint != Vector2.zero) ? newPoint : startPos;

            newPoint = GetHitPoint(extentedEndPoint, dir);
            endPos = (newPoint != Vector2.zero) ? newPoint : endPos;
        }

        Vector2 GetHitPoint(Vector2 start, Vector2 dir)
        {
            var hits = Physics2D.RaycastAll(start, -dir);
            foreach (var hit in hits)
            {
                if (hit.transform == target)
                {
                    return hit.point;
                }
            }

            return Vector2.zero;
        }

        void UpdateShader()
        {
            targetCollider.enabled = false;

            var degree = CalcAngle() + 90f - target.eulerAngles.z;
            var edge = CalcDistance(startPos, (endPos - startPos), target.position);

            var _params = targetJelly.parameters;
            if (_params != null && _params.Count > 0)
            {
                second.GetComponent<Jelly>().parameters.AddRange(_params);
            }

            targetJelly.SetNewShaderParameters(degree, edge);
            second.GetComponent<Jelly>().SetNewShaderParameters(degree + 180, edge * -1);
        }

        void UpdateCollider()
        {
            targetCollider.enabled = false;

            var colliderPoints = targetCollider.points;

            var points1 = new List<Vector2>();
            var points2 = new List<Vector2>();
            var dir = 0;

            var rotFactor = Quaternion.AngleAxis(target.eulerAngles.z, Vector3.back);

            var _startPos = startPos - (Vector2)target.position;
            var _endPos = endPos + (Vector2)target.position;

            _startPos = rotFactor * _startPos;
            _endPos = rotFactor * _endPos;

            bool splitPointsAdded1 = false;
            bool splitPointsAdded2 = false;

            foreach (var point in colliderPoints)
            {
                if (CalcDistance(_startPos, (_endPos - _startPos), point) > 0)
                {
                    if (dir == -1)
                    {
                        points1.Add(_endPos);
                        points1.Add(_startPos);
                        splitPointsAdded1 = true;
                    }
                    points1.Add(point);
                    dir = 1;
                }
                else
                {
                    if (dir == 1)
                    {
                        points2.Add(_startPos);
                        points2.Add(_endPos);
                        splitPointsAdded2 = true;
                    }
                    points2.Add(point);
                    dir = -1;
                }
            }

            if(!splitPointsAdded1)
            {
                points1.Add(_endPos);
                points1.Add(_startPos);
            }
            if (!splitPointsAdded2)
            {
                points2.Add(_startPos);
                points2.Add(_endPos);
            }

            if (points1.Count > 2 && points2.Count > 2)
            {
                targetCollider.points = points1.ToArray();

                second = GameObject.Instantiate(target, target.position, target.rotation);
                second.GetComponent<PolygonCollider2D>().points = points2.ToArray();
                second.GetComponent<Jelly>().InvokeEnableCollider();
            }
            else
            {
                suspend = true;
                r1.velocity = velocity;
            }

            targetJelly.InvokeEnableCollider();
            targetJelly.MoveObj1();
            second.GetComponent<Jelly>().MoveObj2();
        }

        void UpdateRigidBody()
        {
            r2 = second.GetComponent<Rigidbody2D>();

            r1.velocity = velocity;
            r2.velocity = velocity;

            r1.angularVelocity = angularVelocity;
            r2.angularVelocity = angularVelocity;

            var forceDir = Vector2.Perpendicular((endPos - startPos).normalized);

            targetJelly.ExertForce(forceDir * -1f);
            second.GetComponent<Jelly>().ExertForce(forceDir);
        }

        float CalcAngle()
        {
            var lineVec = endPos - startPos;
            return Vector2.SignedAngle(Vector2.right, lineVec);
        }

        float CalcDistance(Vector2 linePoint, Vector2 lineVec, Vector2 point)
        {
            Vector2 lineToCentre = (linePoint - point);
            return Vector2.Dot(lineToCentre, Vector2.Perpendicular(lineVec.normalized));
        }
    }
}
