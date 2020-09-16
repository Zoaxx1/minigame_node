using System;
using UnityEngine;

namespace Tale
{
    public class MG5Origin : MonoBehaviour
    {
        [SerializeField]
        private string type;

        public string Type
        {
            get { return type; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException();

                type = value;
            }
        }
    }
}

