using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ingenio.Domain.Dto;


namespace Ingenio.Core.Services.Interfaces
{
    public interface IAuthorServices
    {
        Task<IEnumerable<AuthorsDto>> GetAuthorsAsync();
    }
}
