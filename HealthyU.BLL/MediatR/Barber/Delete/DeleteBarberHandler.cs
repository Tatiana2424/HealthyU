using BarberShop.DAL.Repositories.Interfaces.Base;
using FluentResults;
using MediatR;

namespace BarberShop.BLL.MediatR.Barber.Delete;

public class DeleteBarberHandler : IRequestHandler<DeleteBarberCommand, Result<Unit>>
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public DeleteBarberHandler(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Result<Unit>> Handle(DeleteBarberCommand request, CancellationToken cancellationToken)
    {
        var barber = await _repositoryWrapper.BarberRepository.GetFirstOrDefaultAsync(f => f.Id == request.Id);

        if (barber is null)
        {
            return Result.Fail(new Error($"Cannot find a barber with id : {request.Id}"));
        }

        _repositoryWrapper.BarberRepository.Delete(barber);

        var resultIsSuccess = await _repositoryWrapper.SaveChangesAsync() > 0;
        return resultIsSuccess ? Result.Ok(Unit.Value) : Result.Fail(new Error("Failed to delete a barber"));
    }
}