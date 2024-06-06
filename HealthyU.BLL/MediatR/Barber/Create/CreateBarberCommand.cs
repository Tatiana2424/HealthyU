using BarberShop.BLL.DTO;
using FluentResults;
using MediatR;

namespace BarberShop.BLL.MediatR.Barber.Create;

public record CreateBarberCommand(BarberDTO Barber) : IRequest<Result<Unit>>;