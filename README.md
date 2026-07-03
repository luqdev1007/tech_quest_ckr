тестовое задание Unity

Мобильное Unity-приложение с нижней навигацией на три вкладки (Кликер / Погода / Породы собак)
и единой последовательной очередью HTTP-запросов.

## Стек

- Unity 2022.3 LTS (C# 9), мобильный портрет, reference resolution 1080×1920
- Zenject - весь DI
- UniTask - весь async
- UniRx - реактивное состояние и биндинги View-Presenter
- DOTween - все анимации
- UnityWebRequest - HTTP
- Newtonsoft.Json - парсинг JSON

Zenject и DOTween закоммичены целиком в "Assets/Plugins" (не через Package Manager):
проект открывается и собирается без ручного импорта пакетов.

## Как запустить

1. Открыть проект в Unity 2022.3 LTS (Package Manager подтянет UniTask/UniRx/Newtonsoft по Packages/manifest.json).
2. Открыть сцену Assets/Scenes/Main.
3. Нажать Play.

Canvas настроен на Scale With Screen Size, reference 1080×1920, портретная ориентация.

## Архитектура

**UI - MVP** 
View - MonoBehaviour без логики: только [SerializeField]-ссылки,
методы отображения и исходящие события.

Presenter - чистый C# класс, связывает View с моделями и сервисами,
владеет подписками и отменой запросов своей вкладки. Model/Service - чистый
C#, состояние в ReactiveProperty.

**DI - Zenject** 
Одна сцена Main, один SceneContext. 
Инсталлеры: ConfigsInstaller, CoreInstaller, UiInstaller, ClickerInstaller, WeatherInstaller, BreedsInstaller

**Очередь запросов (Core/Network)**
единственная точка сети в приложении. 
Один воркер-цикл (RequestQueue.RunWorker) обрабатывает запросы строго последовательно: пока не завершится текущий,
следующий не стартует. 

Enqueue<T> возвращает IRequestHandle<T> с Task и Cancel().
- отмена запроса, ещё не взятого воркером в работу: у элемента выставляется
  флаг/CTS, физически из Queue<IQueueItem> он не убирается, но при попытке исполнения воркер
  видит отмену и пропускает его, не вызывая IWebRequest<T>.Execute(), и Task
  переходит в Canceled;
- отмена уже исполняющегося запроса: Cancel() дёргает CancellationTokenSource, связанный
  (CreateLinkedTokenSource) с токеном воркера - это прерывает UnityWebRequest (WithCancellation
  внутри JsonGetRequest/TextureGetRequest абортит запрос), Task также переходит в Canceled;
- ошибка запроса или отмена одного элемента не останавливают воркер, он продолжает разбирать очередь дальше.

**Пулы и фабрики** 
Летящие «+1» (FlyingCurrencyView) и партиклы разлёта (ClickerBurstView):
Zenject MemoryPool (FlyingCurrencyPool, ClickerBurstPool), переиспользуются без роста числа
инстансов в иерархии. 
Строки списка пород (BreedRowView) - PlaceholderFactory (BreedRowView.Factory).

**Сеть по фичам**
Вкладка Weather не заводит отдельный класс запроса - WeatherPresenter кладёт в
очередь JsonGetRequest<WeatherForecastResponse> напрямую, с заголовками User-Agent/Accept,
собранными на месте. Вкладка Breeds оборачивает JsonGetRequest<T> в тонкие
BreedsListRequest/BreedDetailsRequest, чтобы скрыть построение URL из конфига.


## Структура папок

Assets/Scripts/
  Core/Network/    - очередь запросов, IWebRequest, JsonGetRequest/TextureGetRequest
  Core/Tabs/        - TabType, TabsService
  Core/Audio/       - SoundService
  Configs/          - ScriptableObject-конфиги (Clicker, ClickerVfx, Weather, Breeds)
  Features/Clicker/ - валюта/энергия/автоклик, VFX-пулы, презентер, вью
  Features/Weather/ - DTO прогноза, презентер с поллингом, вью
  Features/Breeds/  - DTO JSON:API, презентер списка/деталей, строка списка, вью
  UI/               - таб-бар, попап, спиннер, интерфейс вкладки
  Installers/       - Zenject-инсталлеры (Configs/Core/Ui/Clicker/Weather/Breeds)


