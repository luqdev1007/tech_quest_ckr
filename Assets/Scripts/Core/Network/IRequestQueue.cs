namespace TestTask.Core.Network
{
    public interface IRequestQueue
    {
        IRequestHandle<T> Enqueue<T>(IWebRequest<T> request);
    }
}
