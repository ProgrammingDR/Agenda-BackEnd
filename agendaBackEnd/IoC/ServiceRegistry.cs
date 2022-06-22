using agendaBackEnd.Services;

namespace agendaBackEnd.IoC
{
    public static class ServiceRegistry
    {
        public static void AddServiceRegistry(this IServiceCollection service)
        {
            service.AddTransient<IAuthService, AuthService>();
        }
    }
}
