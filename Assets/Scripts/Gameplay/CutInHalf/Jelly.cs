using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpriteSlicer
{
    [RequireComponent(typeof(PolygonCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Jelly : MonoBehaviour
    {
        public bool maxSlicesReached = false;

        public List<(float, float)> parameters = new ();
        [Range(2, 8)] [SerializeField] int maxSliceAllowed = 8;

        SpriteRenderer sr;

        void Start()
        {
            NullCheck();
        }

        void NullCheck()
        {
            if (sr == null)
            {
                sr = GetComponent<SpriteRenderer>();
                sr.material = SliceManager.Instance.sliceMaterial;
            }
        }

        public void SetNewShaderParameters(float _degree, float _edge)
        {
            parameters.Add((_degree, _edge));
            maxSlicesReached = (parameters.Count >= maxSliceAllowed) ? true : false;

            UpdateShaderParameters();
        }

        void UpdateShaderParameters()
        {
            NullCheck();

            var sliceIndex = 1;
            foreach (var param in parameters)
            {
                sr.material.SetFloat("_Degree_" + sliceIndex, param.Item1);
                sr.material.SetFloat("_Edge_" + sliceIndex, param.Item2);

                sliceIndex++;
            }

        }

        public void InvokeEnableCollider()
        {
            Invoke(nameof(EnableCollider), 0.15f);
        }

        void EnableCollider()
        {
            GetComponent<PolygonCollider2D>().enabled = true;
        }

        public void ExertForce(Vector2 dir)
        {
            GetComponent<Rigidbody2D>().AddForce(dir * SliceManager.Instance.force);
        }
    }
}

