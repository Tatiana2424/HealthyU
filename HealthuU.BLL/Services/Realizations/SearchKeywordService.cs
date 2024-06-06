using AutoMapper;

using CSharpFunctionalExtensions;

using HealthuU.BLL.DTO;

using HealthyU.DAL.Repositories.Interfaces.Base;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthuU.BLL.Services.Interfaces;

public class SearchKeywordService : ISearchKeywordService
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    public SearchKeywordService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }
    public async Task<Result<List<SearchKeywordDTO>>> GetAllSearchKeywords()
    {
        var searchKeywords = await _repositoryWrapper.SearchKeywordRepository.GetAllAsync();

        var randomSearchKeywords = searchKeywords
            .OrderBy(k => Guid.NewGuid())
            .Take(25)
            .ToList();
        var searchKeywordsDTO = _mapper.Map<List<SearchKeywordDTO>>(randomSearchKeywords);
        return Result.Success(searchKeywordsDTO);
    }
}
