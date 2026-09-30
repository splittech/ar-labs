using Game.Menu;
using R3;
using UnityEngine;
using UnityEngine.Video;

namespace Game.Gameplay
{
    public class VideoPlayerView : MonoBehaviour
    {
        [Header("Video Player")]
        [SerializeField] private VideoPlayer _videoPlayer;

        [Header("Canvas")]
        [SerializeField] private Canvas _menuCanvas;

        [Header("Buttons")]
        [SerializeField] private ButtonView _playButton;
        [SerializeField] private ButtonView _pauseButton;
        [SerializeField] private ButtonView _stopButton;
        [SerializeField] private ButtonView _stepForwardButton;
        [SerializeField] private ButtonView _stepBackwardsButton;
        [SerializeField] private ButtonView _moreSpeedButton;
        [SerializeField] private ButtonView _lessSpeedButton;

        private void Awake()
        {
            _menuCanvas.worldCamera = Camera.main;

            _playButton.OnActionPerformed
                .Where(action => action == ButtonAction.Click)
                .Subscribe(_ => _videoPlayer.Play())
                .AddTo(this);

            _pauseButton.OnActionPerformed
                .Where(action => action == ButtonAction.Click)
                .Subscribe(_ => _videoPlayer.Pause())
                .AddTo(this);

            _stopButton.OnActionPerformed
                .Where(action => action == ButtonAction.Click)
                .Subscribe(_ => _videoPlayer.Stop())
                .AddTo(this);

            _stepForwardButton.OnActionPerformed
                .Where(action => action == ButtonAction.Click)
                .Subscribe(_ => ChangeTimeBy(5f))
                .AddTo(this);

            _stepBackwardsButton.OnActionPerformed
                .Where(action => action == ButtonAction.Click)
                .Subscribe(_ => ChangeTimeBy(-5f))
                .AddTo(this);

            _moreSpeedButton.OnActionPerformed
                .Where(action => action == ButtonAction.Click)
                .Subscribe(_ => ChangeSpeedBy(0.5f))
                .AddTo(this);

            _lessSpeedButton.OnActionPerformed
                .Where(action => action == ButtonAction.Click)
                .Subscribe(_ => ChangeSpeedBy(-0.5f))
                .AddTo(this);
        }

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
            transform.SetPositionAndRotation(pose.position + Vector3.up * 0.01f, pose.rotation);
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        private void ChangeTimeBy(float delta)
        {
            if (!_videoPlayer.canSetTime)
                return;

            double targetTime = _videoPlayer.time + delta;
            _videoPlayer.time = System.Math.Clamp(targetTime, 0d, _videoPlayer.length);
        }

        private void ChangeSpeedBy(float delta)
        {
            if (!_videoPlayer.canSetPlaybackSpeed)
                return;

            _videoPlayer.playbackSpeed = Mathf.Clamp(_videoPlayer.playbackSpeed + delta, 0.5f, 2f);
        }
    }
}
