using AspWithCache.Model.Exceptions;
using AspWithCache.Model.Interfaces;
using AspWithCache.Model.Model;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiWithCache.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HackerNewsController : ControllerBase
    {
        private readonly IAspWithCacheLogger _logger;
        private readonly IStoryDataService _service;
        private readonly IApiConfigurationProvider _configurationPrivider;

        public HackerNewsController(IAspWithCacheLogger logger, IStoryDataService service, IApiConfigurationProvider configurationProvider)
        {
            _logger = logger;
            _service = service;
            _configurationPrivider = configurationProvider;
        }

        /// <summary>
        /// Return 
        /// </summary>
        /// <param name="limit"> limit the number of elements that should be returned</param>
        /// <returns> Array of stories</returns>
        /// <response code="200">When returns data</response>
        /// <response code="404">If no data</response>
        /// <response code="412">If cache is not loaded already</response>
        /// <response code="413">When limit bigger then possible</response>
        /// <response code="500">Other errors</response>
        [HttpGet("bestStories/{limit}")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status412PreconditionFailed,"When data in cache is not ready")]
        [SwaggerResponse(StatusCodes.Status503ServiceUnavailable)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        [SwaggerResponse(StatusCodes.Status413PayloadTooLarge)]
        public IActionResult Get([FromRoute] int limit)
        {
            try
            {
                var data = _service.GetStoryInformations(limit, _configurationPrivider.GetConfiguration().HackerRankApiProviderId);
                return data == null || data.Length == 0 ? NotFound(null) : Ok(data.Select(x => (HackerDataStoryInformation)x).ToArray());
            }
            catch (NoDataFromProviderException)
            {
                return StatusCode(412, "No data. Try again later.");//maybe 503 will be better
            }
            catch (NotKnowProviderException)
            {
                return StatusCode(500, "Not known provider");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return StatusCode(413, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
    }
}