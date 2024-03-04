using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{ 
    public class Disposable : MonoBehaviour
    {
        public float lifetime = 1.0f;

        // Start is called before the first frame update
        void Start()
        {
            Destroy(gameObject, lifetime);
        }
    }
}

