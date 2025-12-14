using FluentResults;
using Microsoft.Extensions.Options;
using Online.Applications.Interface;
using Online.Applications.Model.Configuration;
using Online.Applications.Model.RFBulkPayments;
using System.Text;
using System.Text.Json;

namespace Online.Infrastructure.Client.Supabase
{
    public class RFBulkPayments : BaseSupabaseClient, IRFBulkPayments
    {
        public RFBulkPayments(HttpClient httpClient,
            IOptions<SupabaseConfig> supabaseConfig) : base(httpClient, supabaseConfig)
        {

        }
/*
    * 
[
    {
    "ReferenceNumber": "REF123456",
    "BatchId": "28719595-d591-45e6-a51a-ea1f47b0e66d",
    "Source": "SystemA",
    "Amount": 1500.75,
    "DateRangeTxt": "2025-11-01 to 2025-11-30",
    "ProcessStatusLookUpId": 1,
    "DateCreated": "2025-11-30T15:00:00"
    },
    {
    "ReferenceNumber": "REF123457",
    "BatchId": "28719595-d591-45e6-a51a-ea1f47b0e66d",
    "Source": "SystemB",
    "Amount": 2500.00,
    "DateRangeTxt": "2025-11-15 to 2025-11-30",
    "ProcessStatusLookUpId": 1
    }
]
*/ 

        public async Task<Result<ConfirmedUploadedDataResponse?>> BulkInsertConfirmedUploadedDataAsync(
            List<ConfirmedUploadedDataRequest> rows)
        {
            try
            {
                if (rows == null || rows.Count == 0)
                    return Result.Fail<ConfirmedUploadedDataResponse?>("Input array cannot be empty.");

                var json = JsonSerializer.Serialize(rows);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"{_supabaseConfig.Url}/functions/v1/insert_bulk_ConfirmedUploadedData";
                var response = await Client.PostAsync(url, content);

                var body = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return Result.Fail<ConfirmedUploadedDataResponse?>(
                        $"Failed to call bulk_insert_confirmed_uploaded_data: {response.StatusCode} - {body}"
                    );
                }

                var result = JsonSerializer.Deserialize<ConfirmedUploadedDataResponse>(
                    body,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                return Result.Ok(result);
            }
            catch (Exception ex)
            {
                return Result.Fail<ConfirmedUploadedDataResponse?>(
                    $"Exception while calling bulk_insert_confirmed_uploaded_data: {ex.Message}"
                );
            }
        }

    }

}
