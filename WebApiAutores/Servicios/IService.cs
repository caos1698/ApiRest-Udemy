namespace WebApiAutores.Servicios
{
    public interface IService
    {
        void DoTask();
        Guid ObtainScoped();
        Guid ObtainSingleton();
        Guid ObtainTransient();
    }

    public class ServiceA : IService
    {
        private readonly ILogger<ServiceA> logger;
        private readonly TransientService transientService;
        private readonly ScopedService scopedService;
        private readonly SingletonService singletonService;

        public ServiceA(ILogger<ServiceA> logger, TransientService transientService, ScopedService scopedService, SingletonService singletonService)
        {
            this.logger = logger;
            this.transientService = transientService;
            this.scopedService = scopedService;
            this.singletonService = singletonService;
        }

        public Guid ObtainTransient() { return transientService.Guid; }
        public Guid ObtainScoped() { return scopedService.Guid; }
        public Guid ObtainSingleton() { return singletonService.Guid; }

        public void DoTask()
        {
        }
    }

    public class ServiceB : IService
    {
        public void DoTask()
        {    
        }

        public Guid ObtainScoped()
        {
            throw new NotImplementedException();
        }

        public Guid ObtainSingleton()
        {
            throw new NotImplementedException();
        }

        public Guid ObtainTransient()
        {
            throw new NotImplementedException();
        }
    }

    public class TransientService
    {
        public Guid Guid = Guid.NewGuid();
    }
    public class ScopedService
    {
        public Guid Guid = Guid.NewGuid();
    }
    public class SingletonService
    {
        public Guid Guid = Guid.NewGuid();
    }
}
