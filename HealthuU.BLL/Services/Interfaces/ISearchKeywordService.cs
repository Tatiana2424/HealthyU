using CSharpFunctionalExtensions;

using HealthuU.BLL.DTO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthuU.BLL.Services.Interfaces;

public interface ISearchKeywordService
{
    Task<Result<List<SearchKeywordDTO>>> GetAllSearchKeywords();
}
