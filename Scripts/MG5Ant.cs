using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tale
{
    public class MG5Ant : MonoBehaviour
    {
        #region Properties
        private const float timeToTurn = 0.55f;        

        public int id;
        [SerializeField]
        [Range(0,5)]
        private int speed;

        private bool isStartWalking;

        #endregion

        void Update()
        {
            if (isStartWalking)
            {
                transform.Translate(Vector2.up * speed * Time.deltaTime);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Node"))
            {
                StartCoroutine(TimeLess(collision));
            }
        }

        public void StartWalking(bool value)
        {
            isStartWalking = value;
        }

        private IEnumerator TimeLess(Collider2D collision)
        {
            yield return new WaitForSeconds(timeToTurn / speed);
            transform.rotation = Quaternion.Euler(collision.GetComponent<MG5Node>().GetRotation());
        }
    }
}

