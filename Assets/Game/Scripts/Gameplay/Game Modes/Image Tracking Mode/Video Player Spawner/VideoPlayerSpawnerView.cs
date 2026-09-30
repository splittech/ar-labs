using System;
using System.Collections.Generic;
using Alchemy.Serialization;
using UnityEngine;
using UnityEngine.Video;

namespace Game.Gameplay
{
    [AlchemySerialize]
    public partial class VideoPlayerSpawnerView : MonoBehaviour
    {
        [SerializeField] private GameObject _videoPlayerPrefab;

        [AlchemySerializeField, NonSerialized] private Dictionary<string, VideoClip> _imageVideos;

        public VideoPlayerView CreateVideoPlayerView(string imageName)
        {
            GameObject videoPlayerObject = Instantiate(_videoPlayerPrefab, transform);
            VideoPlayerView videoPlayerView = videoPlayerObject.GetComponent<VideoPlayerView>();
            videoPlayerView.SetVideoClip(_imageVideos[imageName]);
            return videoPlayerView;
        }
    }
}
