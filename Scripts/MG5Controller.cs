using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tale
{
    public class MG5Controller : MonoBehaviour
    {
        #region Properties

        public static MG5Controller Instance
        {
            get
            {
                if (s_Instance != null)
                    return s_Instance;

                s_Instance = FindObjectOfType<MG5Controller>();

                if (s_Instance != null)
                    return s_Instance;

                return null;
            }
        }

        private static MG5Controller s_Instance;
        private const int timeToSpawnAnt = 2;

        [SerializeField]
        private List<MG5Origin> origins;
        [SerializeField]
        private List<MG5Node> nodesList;
        [SerializeField]
        private int targetsAmount;

        private ObjectPooler pooler;
        private List<MG5Ant> antsList;
        private int fullTargets;

        #endregion

        #region MonoBehaviour Functions

        private void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            pooler = GetComponent<ObjectPooler>();   
        }

        private void Start()
        {
            antsList = new List<MG5Ant>();
            fullTargets = 0;
        }

        #endregion

        #region GameController Functions

        public void MGStart()
        {
            foreach (MG5Node node in nodesList)
            {
                node.StartRotation(true);
            }

            foreach (MG5Origin origin in origins)
            {
                SpawnAnt(origin.Type, origin.transform.position, origin.transform.rotation);
            }
        }

        public void MGPause()
        {
            foreach (MG5Ant ant in antsList)
            {
                ant.StartWalking(false);
            }

            foreach (MG5Node node in nodesList)
            {
                node.StartRotation(false);
            }
        }

        public void MGResume()
        {
            foreach (MG5Ant ant in antsList)
            {
                ant.StartWalking(true);
            }

            foreach (MG5Node node in nodesList)
            {
                node.StartRotation(true);
            }
        }

        #endregion

        #region Gameplay Functions

        public void FullTargets()
        {
            fullTargets++;
            
            if (fullTargets >= targetsAmount)
            {
                MGWin();
            }
        }       

        public void MGLose()
        {
            MGPause();
            Debug.Log("YOU LOSE");
        }

        private void MGWin()
        {
            Debug.Log("You Win");
        }

        private void SpawnAnt(string type, Vector2 position, Quaternion rotation)
        {
            MG5Ant ant = pooler.Get(type, position, rotation).GetComponent<MG5Ant>();
            ant.StartWalking(true);
            antsList.Add(ant);
        }

        #endregion

        #region Support Function

        public void RemoveAnt(MG5Ant ant)
        {
            ant.gameObject.GetComponent<PooledObject>().Recycle();
            antsList.Remove(ant);
        }

        public IEnumerator AntIntoTarget(int id)
        {
            yield return new WaitForSeconds(timeToSpawnAnt);
            SpawnAnt(origins[id].Type, origins[id].transform.position, origins[id].transform.rotation);
        }

        #endregion
    }
}

