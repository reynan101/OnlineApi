namespace Online.Applications.Model.RFBulkPayments
{
    public class ConfirmedUploadedDataRequest
    {
        public string ReferenceNumber { get; set; } = null!;
        public string? BatchId { get; set; }
        public string? Source { get; set; }
        public decimal? Amount { get; set; }
        public string? DateRangeTxt { get; set; }
        public int? ProcessStatusLookUpId { get; set; }
        public string? DateCreated { get; set; }
    }
}
