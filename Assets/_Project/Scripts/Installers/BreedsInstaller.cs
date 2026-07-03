using TestTask.Features.Breeds;
using TestTask.UI;
using UnityEngine;
using Zenject;

namespace TestTask.Installers
{
    public sealed class BreedsInstaller : MonoInstaller
    {
        [SerializeField] private BreedsView _breedsView;
        [SerializeField] private PopupView _popupView;
        [SerializeField] private BreedRowView _rowPrefab;
        [SerializeField] private Transform _rowsContainer;

        public override void InstallBindings()
        {
            Container.BindInstance(_breedsView).AsSingle();
            Container.BindInstance(_popupView).AsSingle();

            Container.BindFactory<BreedRowView, BreedRowView.Factory>()
                .FromComponentInNewPrefab(_rowPrefab)
                .UnderTransform(_rowsContainer);

            Container.BindInterfacesAndSelfTo<BreedsPresenter>().AsSingle();
        }
    }
}
