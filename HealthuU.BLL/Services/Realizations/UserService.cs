using AutoMapper;

using CSharpFunctionalExtensions;

using HealthuU.BLL.DTO;

using HealthyU.DAL.Repositories.Interfaces.Base;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthuU.BLL.Services.Interfaces;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IImageService _imageService;
    public UserService(IRepositoryWrapper repositoryWrapper, IMapper mapper, IImageService imageService)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _imageService = imageService;
    }
    public async Task<Result<UserDTO>> GetUserById(int userId)
    {
        var user = await _repositoryWrapper.UserRepository.GetFirstOrDefaultAsync(a => a.Id == userId, include: a => a.Include(x => x.Image));
        var userDTO = _mapper.Map<UserDTO>(user);
        return Result.Success(userDTO);
    }

    public async Task<Result<UserDTO>> UpdateUser(int userId, UserDTO userDTO)
    {
        var user = await _repositoryWrapper.UserRepository.GetFirstOrDefaultAsync(a => a.Id == userId);
        if (user == null)
        {
            return Result.Failure<UserDTO>("User not found.");
        }

        if (userDTO.Image != null)
        {
            var imageDTO = await _imageService.CreateOrUpdateImageAsync(userDTO.Image);
            userDTO.ImageId = imageDTO.Id;
        }
        else if (userDTO.Image == null && userDTO.ImageId== 0)
        {
            userDTO.ImageId = null;
        }

        _mapper.Map(userDTO, user);
        _repositoryWrapper.UserRepository.Update(user);
        await _repositoryWrapper.SaveChangesAsync();
        return Result.Success(_mapper.Map<UserDTO>(user));
    }
}
