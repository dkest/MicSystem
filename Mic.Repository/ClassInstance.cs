namespace Mic.Repository
{
    public static class ClassInstance<T> where T : new()
    {
        static ClassInstance()
        {
            Instance = new T();
        }
        public static T Instance { get; private set; }
    }
}