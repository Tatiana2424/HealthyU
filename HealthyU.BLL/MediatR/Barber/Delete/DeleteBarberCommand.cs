using FluentResults;
using MediatR;

namespace BarberShop.BLL.MediatR.Barber.Delete;

public record DeleteBarberCommand(int Id) : IRequest<Result<Unit>>;