using UnityEngine;

namespace Tale
{
    public class PooledObject : MonoBehaviour
    {
        private string objectType;
        private ObjectPooler objectPooler;

        public void SetObjectPooler(ObjectPooler _objectPooler)
        {
            objectPooler = _objectPooler;
        }

        public void SetObjectType(string _objectType)
        {
            objectType = _objectType;
        }

        public string GetObjectType()
        {
            return objectType;
        }

        public void Recycle()
        {
            objectPooler.Recycle(this);
        }
    }

}
