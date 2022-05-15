using Microsoft.AspNetCore.Mvc;

namespace PackagingAndDelivery_Microservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PackagingAndDeliveryController : ControllerBase
    {
        [HttpGet("GetPackagingDeliveryCharge")]
        public ActionResult<double> GetPackagingDeliveryCharge(string componentType, int count)
        {
            double packagingAndDeliveryCharge = 0;

            if (componentType == "Accessory")
            {
                packagingAndDeliveryCharge = (50 + 100 + 50) * count;

            }

            if (componentType == "Integral")
            {
                packagingAndDeliveryCharge = (100 + 200 + 50) * count;
            }

            return packagingAndDeliveryCharge;
        }
    }
}
