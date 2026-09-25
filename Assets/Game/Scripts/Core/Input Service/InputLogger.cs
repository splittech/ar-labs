using R3;
using UnityEngine;

namespace Game.Core.Input
{
    public class InputLogger
    {
        private readonly InputService _inputService;

        public InputLogger(InputService inputService)
        {
            _inputService = inputService;
        }

        public void Initialize()
        {
            _inputService.OnInputActionPerformed.Subscribe(OnInputActionPerformed);
        }

        private void OnInputActionPerformed(InputContext context)
        {
            Debug.Log($"[INPUT LOGGER] Action performed: {context}.");
        }
    }
}