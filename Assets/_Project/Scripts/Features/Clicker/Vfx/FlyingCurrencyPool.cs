using UnityEngine;
using Zenject;

namespace TestTask.Features.Clicker.Vfx
{
    public sealed class FlyingCurrencyPool : MonoMemoryPool<Vector3, int, FlyingCurrencyView>
    {
        protected override void Reinitialize(Vector3 worldPos, int amount, FlyingCurrencyView item)
        {
            item.Play(worldPos, amount, () => Despawn(item));
        }
    }
}
