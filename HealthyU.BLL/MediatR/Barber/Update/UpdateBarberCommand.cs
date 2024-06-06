using BarberShop.BLL.DTO;
using FluentResults;
using MediatR;

namespace BarberShop.BLL.MediatR.Barber.Update;

public record UpdateBarberCommand(BarberDTO Barber) : IRequest<Result<Unit>>;