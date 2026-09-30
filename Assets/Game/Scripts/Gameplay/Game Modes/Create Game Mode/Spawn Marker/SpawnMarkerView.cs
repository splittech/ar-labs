using UnityEngine;

namespace Game.Gameplay
{
    public class SpawnMarkerView : MonoBehaviour
    {
        public Vector3 Position => transform.position;
        public Quaternion Rotation => transform.rotation;

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
