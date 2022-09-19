using Ingenio.Core.Constans;
using Ingenio.Core.Services.Interfaces;
using Ingenio.Data.Repositories;
using Ingenio.Domain.Dto;
using Ingenio.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


using Utilities.Http;
using Utilities.Logger;

namespace Ingenio.Core.Services
{
    public class AuthorServices : IAuthorServices
    {
        private readonly IHttpClientServices services;
        private readonly IAuthorRepository repository;
        private readonly ILogger<AuthorServices> logger;

        public AuthorServices(
            IHttpClientServices services,
            IAuthorRepository repository,
            ILogger<AuthorServices> logger
            )
        {
            this.services = services ?? throw new ArgumentNullException(nameof(IHttpClientServices));
            this.repository = repository ?? throw new ArgumentNullException(nameof(IAuthorRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(ILogger<AuthorServices>));
        }

        public async Task<IEnumerable<AuthorsDto>> GetAuthorsAsync()
        {
            DateTime startTime = DateTime.Now;
            logger.LogInformation($"Method: {nameof(GetAuthorsAsync)} start: {startTime}");
            var result = await GetAuthorsFromRestOrDbAsync();
            return result;
        }

        private async Task<IEnumerable<AuthorsDto>> GetAuthorsFromRestOrDbAsync()
        {
            DateTime startTime = DateTime.Now;
            logger.LogInformation($"Method: {nameof(GetAuthorsFromRestOrDbAsync)} start: {startTime}");
            var result = await services.GetUnAuthAsync<IEnumerable<AuthorsDto>>(UrlConstans.Authors);
            if (result != null)
            {
                await repository.RemovePhysicalAllElementsAsync();
                await repository.AddRangeAsync(result.Select(m => (AuthorEntity)m));
            }
            else
            {
                result = GetAuthorsFromDb();
            }
            logger.LogInformation($"Method: {nameof(GetAuthorsFromRestOrDbAsync)} end: {((DateTime.Now - startTime)).TotalMilliseconds}");
            return result;
        }

        public IEnumerable<AuthorsDto> GetAuthorsFromDb()
        {
            DateTime startTime = DateTime.Now;
            logger.LogInformation($"Method: {nameof(GetAuthorsFromDb)} start: {startTime}");
            var result = repository.Find().ToList();
            if (!result.Any())
            {
                result = repository.Find().ToList();
            }
            logger.LogInformation($"Method: {nameof(GetAuthorsFromDb)} end: {((DateTime.Now - startTime)).TotalMilliseconds}");
            return result.Select(m => (AuthorsDto)m).ToList();
        }
    }
