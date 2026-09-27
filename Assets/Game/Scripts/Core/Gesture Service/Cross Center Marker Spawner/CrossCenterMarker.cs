using UnityEngine;

namespace Game.Core
{
    public class CrossCenterMarker : MonoBehaviour
    {
        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}