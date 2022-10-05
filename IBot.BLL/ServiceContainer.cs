using IBot.Core.Interfaces.Repositories;

namespace IBot.BLL;

public class ServiceContainer
{
    public Configuration Configuration { get; }
    public IUnitOfWork UnitOfWork { get; }
    public ServiceContainer(IUnitOfWork unitOfWork, Configuration configuration)
    {
        UnitOfWork = unitOfWork;
        Configuration = configuration;
    }
}