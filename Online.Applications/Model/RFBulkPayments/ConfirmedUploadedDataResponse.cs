namespace Online.Applications.Model.RFBulkPayments
{
    public class ConfirmedUploadedDataResponse
    {
        public bool Success { get; set; }
        public List<ConfirmedUploadedDataRequest>? Data { get; set; }
        public string? Error { get; set; }
    }
}
