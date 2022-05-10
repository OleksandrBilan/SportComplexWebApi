using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.ApiModels.GroupTrainingSubscription;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/subscriptionType")]
    public class SubscriptionTypeController : Controller
    {
        private readonly SubscriptionTypeService _subscriptionTypeService;

        public SubscriptionTypeController(SubscriptionTypeService subscriptionService)
        {
            _subscriptionTypeService = subscriptionService;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetSubscriptionTypesAsyn()
        {
            return Ok(await _subscriptionTypeService.GetAllAsync());
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateSubscriptionTypeAsync([FromBody] SubscriptionTypeApiModel subscriptionType)
        {
            var createdSubscriptionType = await _subscriptionTypeService.CreateAsync(subscriptionType);

            if (createdSubscriptionType is null)
            {
                return BadRequest();
            }

            return Ok(createdSubscriptionType);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateSubscriptionTypeAsync([FromBody] SubscriptionTypeApiModel subscriptionType)
        {
            var updatedSubscriptionType = await _subscriptionTypeService.UpdateAsync(subscriptionType);

            if (updatedSubscriptionType is null)
            {
                return BadRequest();
            }

            return Ok(updatedSubscriptionType);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteSubscriptionTypeAsync(int id)
        {
            bool result = await _subscriptionTypeService.DeleteAsync(id);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
