using Microsoft.AspNetCore.Http;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace chuyennganh.Domain.Helpers
{
    public class VnPayLibrary
    {
        public const string VERSION = "2.1.0";
        public const string COMMAND = "pay";

        private readonly SortedList<string, string> _requestData = new();
        private readonly SortedList<string, string> _responseData = new();

        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _requestData.Add(key, value);
            }
        }

        public string CreateRequestUrl(string baseUrl, string hashSecret)
        {
            var data = BuildQuery(_requestData);
            string signData = string.Join('&', data.Select(kvp => $"{kvp.Key}={WebUtility.UrlEncode(kvp.Value)}"));
            string hashValue = HmacSHA512(signData, hashSecret);
            string queryUrl = $"{baseUrl}?{signData}&vnp_SecureHash={hashValue}";
            return queryUrl;
        }

        public bool ValidateSignature(IQueryCollection collection, string hashSecret)
        {
            var response = collection
                .Where(kvp => kvp.Key.StartsWith("vnp_"))
                .ToDictionary(k => k.Key, v => v.Value.ToString());

            string vnpSecureHash = response["vnp_SecureHash"];
            response.Remove("vnp_SecureHash");
            response.Remove("vnp_SecureHashType");

            string signData = string.Join('&', response.OrderBy(kvp => kvp.Key)
                .Select(kvp => $"{kvp.Key}={WebUtility.UrlEncode(kvp.Value)}"));

            string hashCheck = HmacSHA512(signData, hashSecret);
            return string.Equals(vnpSecureHash, hashCheck, StringComparison.InvariantCultureIgnoreCase);
        }

        private static string HmacSHA512(string input, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            using var hmac = new HMACSHA512(keyBytes);
            byte[] hashBytes = hmac.ComputeHash(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        private SortedList<string, string> BuildQuery(SortedList<string, string> data)
        {
            return new SortedList<string, string>(data);
        }
    }
}
