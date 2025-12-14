namespace Online.Applications.Model.RFBulkPayments
{
    public class ConfirmedDataManualRequest
    {
        public string ReferenceNumber { get; set; } = null!;
        public decimal? Amount { get; set; }
    }
}
