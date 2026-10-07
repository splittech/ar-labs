using UnityEngine;

namespace Game.Core.AR
{
    public interface IRaycastService
    {
        bool RaycastOnFloor(Vector2 screenPosition, out Pose pose);

        bool RaycastOnObject(
            Vector2 screenPosition,
            LayerMask interactableLayer,
            out Collider hitCollider,
            float maxDistance = 100f);

        bool TryRaycastOnComponent<T>(
            Vector2 screenPosition,
            LayerMask interactableLayer,
            out T component,
            float maxDistance = 100f);
    }
}
