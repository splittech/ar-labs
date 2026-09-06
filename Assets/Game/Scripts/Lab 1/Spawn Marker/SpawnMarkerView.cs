using UnityEngine;

namespace Game.Lab1
{
    public class SpawnMarkerView : MonoBehaviour
    {
        public void SetTransformPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            transform.SetPositionAndRotation(position, rotation);
        }

        public void DestroyObject()
        {
            Destroy(gameObject);
        }
    }
}
