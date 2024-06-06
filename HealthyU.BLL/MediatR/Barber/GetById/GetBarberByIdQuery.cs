using BarberShop.BLL.DTO;
using FluentResults;
using MediatR;

namespace BarberShop.BLL.MediatR.Barber.GetById;

public record GetBarberByIdQuery(int Id) : IRequest<Result<BarberDTO>>;
