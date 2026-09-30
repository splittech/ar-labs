using UnityEngine;

namespace Game.Gameplay
{
    public class FaceSpawnerView : MonoBehaviour
    {
        [SerializeField] private GameObject _ARFacePrefab;

        public ARFaceView CreateARFaceView()
        {
            GameObject ARFaceObject = Instantiate(_ARFacePrefab, transform);
            ARFaceView ARFaceView = ARFaceObject.GetComponent<ARFaceView>();
            return ARFaceView;
        }
    }
}
