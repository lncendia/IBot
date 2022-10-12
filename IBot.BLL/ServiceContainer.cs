using IBot.Core.Interfaces.Repositories;
using IBot.Core.Interfaces.Services.Infrastructure;

namespace IBot.BLL;

public class ServiceContainer
{
    public Configuration Configuration { get; }
    public IUnitOfWork UnitOfWork { get; }
    public IPaymentCreationService PaymentCreationService { get; }
    public ServiceContainer(IUnitOfWork unitOfWork, Configuration configuration, IPaymentCreationService paymentCreationService)
    {
        UnitOfWork = unitOfWork;
        Configuration = configuration;
        PaymentCreationService = paymentCreationService;
    }
}