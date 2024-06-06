using BarberShop.BLL.DTO;
using FluentResults;
using MediatR;

namespace BarberShop.BLL.MediatR.Barber.GetAll;

public record GetAllBarbersQuery : IRequest<Result<IEnumerable<BarberDTO>>>;
