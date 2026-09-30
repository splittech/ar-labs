using UnityEngine;
using UnityEngine.Video;

namespace Game.Gameplay
{
    public class VideoPlayerView : MonoBehaviour
    {
        [SerializeField] private VideoPlayer _videoPlayer;

        public void SetVideoClip(VideoClip videoClip)
        {
            _videoPlayer.clip = videoClip;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public void SetPose(Pose pose)
        {
            transform.SetPositionAndRotation(pose.position + Vector3.up * 0.001f, pose.rotation);
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}
