//using AutoMapper;
//using BarberShop.BLL.DTO;
//using BarberShop.DAL.Entities;
//using FluentResults;
//using HealthuU.BLL.Services.Interfaces;
//using HealthyU.DAL.Repositories.Interfaces.Base;
//using HealthyU.DAL.Repositories.Realizations.Base;
//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace HealthuU.BLL.Services.Realizations;

//public class OrderService : IOrderService
//{
//    private readonly IMapper _mapper;
//    private readonly IRepositoryWrapper _repositoryWrapper;
//    public OrderService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
//    {
//        _repositoryWrapper = repositoryWrapper;
//        _mapper = mapper;
//    }

//    public async Task<Result<List<TimeSpan>>> GetByBarberIdArrayOfFreeTime(int sumTime, string dateString, int barberId)
//    {
//        if (!DateTime.TryParse(dateString, out DateTime date))
//        {
//            return Result.Fail<List<TimeSpan>>(new Error("Invalid date format"));
//        }

//        var order = await _repositoryWrapper.OrderRepository
//          .GetAllAsync(o => o.BarberId == barberId && o.date == date,
//              include: o => o.Include(o => o.Service));

//        if (order is null)
//        {
//            return Result.Fail(new Error($"Cannot find any order with barber id: {barberId}"));
//        }
//        var startOrderTimes = order.Select(o => o.time).ToList();
//        var tim = order.Select(o => o.Service.TimeToMake).ToList();
//        var finishorderTime = order.Select(o => o.time.Add(o.Service.TimeToMake)).ToList();

//        var allTimes = new List<TimeSpan>();
//        var startTime = new TimeSpan(10, 0, 0); // start time of available time slots
//        var endTime = new TimeSpan(18, 0, 0); // end time of available time slots
//        var currentTime = startTime;
//        var totalTime = TimeSpan.FromMinutes(sumTime);
//        int i = 1;

//        while (currentTime + totalTime <= endTime)
//        {
//            bool isFreeTime = true;
//            foreach (var orderTime in startOrderTimes)
//            {
//                if (currentTime < orderTime && currentTime + totalTime > orderTime)
//                {
//                    isFreeTime = false;
//                    break;
//                }
//            }
//            if (isFreeTime)
//            {
//                allTimes.Add(currentTime);
//            }
//            currentTime += TimeSpan.FromMinutes(30);
//        }

//        var freeTimes = allTimes.Where(t => !startOrderTimes.Any(s => t >= s && t < finishorderTime[startOrderTimes.IndexOf(s)])).ToList();

//        return Result.Ok(freeTimes);

//    }
//}
