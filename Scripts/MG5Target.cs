using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tale
{
    public class MG5Target : MonoBehaviour
    {
        [SerializeField]
        private int idAccepted;
        [SerializeField]
        private int antsToCompleteTarget;

        private int antsIntoTarget;

        private void Start()
        {
            antsIntoTarget = 0;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<MG5Ant>().id == idAccepted)
            {
                antsIntoTarget++;

                if (antsIntoTarget >= antsToCompleteTarget)
                {
                    MG5Controller.Instance.FullTargets();
                }
                else
                {
                    StartCoroutine(MG5Controller.Instance.AntIntoTarget(idAccepted));
                }

                MG5Controller.Instance.RemoveAnt(collision.GetComponent<MG5Ant>());
            }
            else
            {
                MG5Controller.Instance.MGLose();
            }            
        }
    }
}

