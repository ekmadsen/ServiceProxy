using JetBrains.Annotations;


namespace ErikTheCoder.ServiceProxy
{
    public class Proxy<T>
    {
        [UsedImplicitly]
        public T AsAdmin { get; }


        [UsedImplicitly]
        public T AsUser { get; }


        public Proxy(T AdminService, T UserService)
        {
            AsAdmin = AdminService;
            AsUser = UserService;
        }
    }
}
