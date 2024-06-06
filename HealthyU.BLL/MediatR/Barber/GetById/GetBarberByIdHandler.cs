using AutoMapper;
using BarberShop.BLL.DTO;
using BarberShop.DAL.Repositories.Interfaces.Base;
using FluentResults;
using MediatR;


namespace BarberShop.BLL.MediatR.Barber.GetById;

public class GetBarberByIdHandler : IRequestHandler<GetBarberByIdQuery, Result<BarberDTO>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetBarberByIdHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<BarberDTO>> Handle(GetBarberByIdQuery request, CancellationToken cancellationToken)
    {
        var barber = await _repositoryWrapper
            .BarberRepository
            .GetSingleOrDefaultAsync(c => c.Id == request.Id);

        if (barber is null)
        {
            return Result.Fail(new Error($"Cannot find any category with corresponding id: {request.Id}"));
        }

        var barberDto = _mapper.Map<BarberDTO>(barber);
        return Result.Ok(barberDto);
    }
}