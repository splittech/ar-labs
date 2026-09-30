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
            transform.SetPositionAndRotation(pose.position, pose.rotation);
        }
    }
}
