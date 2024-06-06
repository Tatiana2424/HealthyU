using AutoMapper;
using BarberShop.BLL.DTO;

using HealthuU.BLL.Services.Interfaces;

using HealthyU.DAL.Entities;
using HealthyU.DAL.Repositories.Interfaces.Base;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthuU.BLL.Services.Realizations
{
    public class ImageService : IImageService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public ImageService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<ImageDTO> CreateOrUpdateImageAsync(ImageDTO imageDTO)
        {
            Image image;
            if (imageDTO.Id > 0)
            {
                image = await _repositoryWrapper.ImageRepository.GetFirstOrDefaultAsync(a => a.Id == imageDTO.Id);
                if (image == null)
                {
                    throw new Exception("Image not found.");
                }
                _mapper.Map(imageDTO, image);
            }
            else
            {
                image = _mapper.Map<Image>(imageDTO);
                _repositoryWrapper.ImageRepository.Create(image);
            }

            await _repositoryWrapper.SaveChangesAsync();
            return _mapper.Map<ImageDTO>(image);
        }
    }
}
