using Fabric.Api.Dtos;
using Fabric.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fabric.Api.Controllers
{
    [ApiController]
    [Route("contract")]
    public class ContractController : ControllerBase
    {
        private readonly ContractEvaluationService _evaluator;

        public ContractController(ContractEvaluationService evaluator)
        {
            _evaluator = evaluator;
        }

        [HttpPost("value")]
        public ActionResult<double> EvaluateContract([FromBody] EvaluateContractRequest request)
        {
            try
            {
                var value = _evaluator.Evaluate(request.Contract);
                return Ok(value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
