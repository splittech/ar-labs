using UnityEngine;

namespace Game.Gameplay
{
    public class ARFaceView : MonoBehaviour
    {
        public void SetPose(Pose pose)
        {
            transform.SetPositionAndRotation(pose.position, pose.rotation);
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}