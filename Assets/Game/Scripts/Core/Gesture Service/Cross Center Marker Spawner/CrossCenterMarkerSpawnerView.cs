using UnityEngine;

namespace Game.Core
{
    public class CrossCenterMarkerSpawnerView : MonoBehaviour
    {
        [SerializeField] private GameObject _crossCenterMarkerPrefab;
        [SerializeField] private bool _showCrossCenterMarker;
        [SerializeField] private float _markerShowTime = 1f;

        public bool ShowCrossCenterMarker => _showCrossCenterMarker;
        public float MarkerShowTime => _markerShowTime;

        public CrossCenterMarker CreateCrossCenterMarker(Vector2 screenPosition)
        {
            GameObject markerObject = Instantiate(_crossCenterMarkerPrefab, transform);
            markerObject.transform.position = screenPosition;
            return markerObject.GetComponent<CrossCenterMarker>();
        }
    }
}