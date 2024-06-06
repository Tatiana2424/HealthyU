using AutoMapper;
using BarberShop.DAL.Repositories.Interfaces.Base;
using FluentResults;
using MediatR;

namespace BarberShop.BLL.MediatR.Barber.Create;

public class CreateBarberHandler : IRequestHandler<CreateBarberCommand, Result<Unit>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public CreateBarberHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<Unit>> Handle(CreateBarberCommand request, CancellationToken cancellationToken)
    {
        var barber = _mapper.Map<DAL.Entities.Barber>(request.Barber);

        if (barber is null)
        {
            return Result.Fail(new Error("Cannot convert null to Fact"));
        }

        _repositoryWrapper.BarberRepository.Create(barber);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;

        return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error("Failed to create a barber"));
    }
}