using UnityEngine;

namespace Game.Gameplay
{
    public class FaceSpawnerView : MonoBehaviour
    {
        [SerializeField] private GameObject _ARFacePrefab;

        private GameObject _ARFace;

        public void CreateFace()
        {
            _ARFace = Instantiate(_ARFacePrefab);
        }

        public void DestroyFace()
        {
            if (_ARFace == null)
                return;

            Destroy(_ARFace);
        }

        public void UpdateFacePose()
        {

        }
    }
}
