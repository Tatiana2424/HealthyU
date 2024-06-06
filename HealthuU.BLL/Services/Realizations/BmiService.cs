using AutoMapper;

using CSharpFunctionalExtensions;

using HealthuU.BLL.DTO;
using HealthuU.BLL.Services.Interfaces;

using HealthyU.DAL.Entities;
using HealthyU.DAL.Repositories.Interfaces.Base;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthuU.BLL.Services.Realizations
{
    public class BmiService : IBmiService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public BmiService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<Result<List<BmiDTO>>> GetBmiByUserId(int userId)
        {
            var bmi = await _repositoryWrapper.BMIRepository.GetAllAsync(a => a.UserId == userId);
            var bmiDTO = _mapper.Map<List<BmiDTO>>(bmi);
            return Result.Success(bmiDTO);
        }

        public async Task<Result<BmiDTO>> CreateBMI(BmiDTO bmiDTO)
        {
            var bmi = _mapper.Map<BMIData>(bmiDTO);
            bmi.DateTime = DateTime.Now;
            _repositoryWrapper.BMIRepository.Create(bmi);
            await _repositoryWrapper.SaveChangesAsync();
            return Result.Success(_mapper.Map<BmiDTO>(bmi));
        }

        public async Task<Result<bool>> DeleteBMI(int id)
        {
            var bmi = await _repositoryWrapper.BMIRepository.GetFirstOrDefaultAsync(a => a.Id == id);
            if (bmi == null)
            {
                return Result.Failure<bool>("Bmi not found.");
            }

            _repositoryWrapper.BMIRepository.Delete(bmi);
            await _repositoryWrapper.SaveChangesAsync();
            return Result.Success(true);
        }
    }
}
