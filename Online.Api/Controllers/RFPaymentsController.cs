using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online.Applications.Interface;
using Online.Applications.Model.RFBulkPayments;

namespace Online.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class RFPaymentsController : ControllerBase
    {
        private readonly IRFBulkPayments _rfBulkPayments;

        public RFPaymentsController(IRFBulkPayments rfBulkPayments)
        {
            _rfBulkPayments = rfBulkPayments;
        }

        [HttpPost]
        public async Task<IActionResult> ManualPayment([FromBody] ConfirmedDataManualRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.ReferenceNumber))
                return BadRequest("Bad Input cannot be empty.");

            List<ConfirmedUploadedDataRequest> rows = new List<ConfirmedUploadedDataRequest>();

            var item = new ConfirmedUploadedDataRequest()
            {
                ReferenceNumber = request.ReferenceNumber,
                Amount = request.Amount,
                BatchId = Guid.NewGuid().ToString(),
                DateRangeTxt = DateTime.Today.ToLongDateString(),
                ProcessStatusLookUpId = 1,
                Source = "Gcash Manual"
            };
            rows.Add(item);

            var result = await _rfBulkPayments.BulkInsertConfirmedUploadedDataAsync(rows);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result.Errors);
        }

        [HttpPost]
        public async Task<IActionResult> BulkInsert([FromBody] List<ConfirmedUploadedDataRequest> rows)
        {
            if (rows == null || rows.Count == 0)
                return BadRequest("Input array cannot be empty.");

            var result = await _rfBulkPayments.BulkInsertConfirmedUploadedDataAsync(rows);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result.Errors);
        }
    }
}
