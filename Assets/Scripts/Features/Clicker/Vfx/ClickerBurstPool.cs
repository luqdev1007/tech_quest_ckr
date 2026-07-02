using UnityEngine;
using Zenject;

namespace TestTask.Features.Clicker.Vfx
{
    public sealed class ClickerBurstPool : MonoMemoryPool<Vector3, ClickerBurstView>
    {
        protected override void Reinitialize(Vector3 worldPos, ClickerBurstView item)
        {
            item.Play(worldPos, () => Despawn(item));
        }
    }
}
