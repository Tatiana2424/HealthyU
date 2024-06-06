using AutoMapper;
using BarberShop.BLL.DTO;
using BarberShop.DAL.Repositories.Interfaces.Base;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace BarberShop.BLL.MediatR.Barber.GetAll;

public class GetAllBarbersHandler : IRequestHandler<GetAllBarbersQuery, Result<IEnumerable<BarberDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public GetAllBarbersHandler(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<BarberDTO>>> Handle(GetAllBarbersQuery request, CancellationToken cancellationToken)
    {
        var barbers = await _repositoryWrapper
            .BarberRepository
            .GetAllAsync(
                include: scl => scl.Include(sc => sc.Image)!);

        if (barbers is null)
        {
            return Result.Fail(new Error($"Cannot find any categories"));
        }

        var barbersDtos = _mapper.Map<IEnumerable<BarberDTO>>(barbers);
        return Result.Ok(barbersDtos);
    }
}
