using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Gameplay
{
    public class PudgeMergerView : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private GameObject _finalEffectPrefab;
        [SerializeField] private Transform _effectsRootTransform;

        [Header("Effects")]
        [SerializeField] private float _addScale = 1f;
        [SerializeField] private float _scaleToDestroy = 3f;

        public float AddScale => _addScale;
        public float ScaleToDestroy => _scaleToDestroy;

        public void CreateFinalEffect(Vector3 position)
        {
            PlayEffectAsync(position, destroyCancellationToken).Forget();
        }

        public async UniTask PlayEffectAsync(Vector3 position, CancellationToken cancellationToken)
        {
            GameObject effectInstance = Instantiate(_finalEffectPrefab, _effectsRootTransform);
            effectInstance.transform.position = position;

            ParticleSystem particleSystem = effectInstance.GetComponent<ParticleSystem>();

            particleSystem.Play(withChildren: true);

            await UniTask.WaitUntil(
                () => !particleSystem.IsAlive(withChildren: true),
                cancellationToken: cancellationToken);

            Destroy(effectInstance);
        }
    }
}