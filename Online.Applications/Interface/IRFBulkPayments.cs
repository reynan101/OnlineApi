using FluentResults;
using Online.Applications.Model.RFBulkPayments;

namespace Online.Applications.Interface
{
    public interface IRFBulkPayments
    {
        Task<Result<ConfirmedUploadedDataResponse?>> BulkInsertConfirmedUploadedDataAsync(
            List<ConfirmedUploadedDataRequest> rows);
    }
}
