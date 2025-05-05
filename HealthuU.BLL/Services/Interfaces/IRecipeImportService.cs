using CSharpFunctionalExtensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthuU.BLL.Services.Interfaces;

public interface IRecipeImportService
{
    Task ImportRecipesAsync();

    Task ImportRecipesFromDesktopWithIDisposableAsync();

    Task ImportRecipesFromDesktopAsync();
}
