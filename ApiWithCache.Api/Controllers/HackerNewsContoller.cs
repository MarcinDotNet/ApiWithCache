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

        [HttpGet("bestStories/{limit}")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status412PreconditionFailed)]
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