using InternetBankingRESTfulService.Api;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace InternetBankingRESTfulService.Web
{
    [Route("/bank/api/")]
    [ApiController]
    public class InternetBankingApiController : Controller, IInternetBankingApi
    {
        [HttpGet("version")]
        [HttpGet("api-version")]
        public string GetApiVersion()
        {
            DateTime currentUtc = DateTime.UtcNow;
            string version = $"{DateTime.UtcNow.ToString("yyyy.MM.dd", CultureInfo.InvariantCulture)}.1.0";
            //string version = $"Version: {currentUtc.Year}{currentUtc.Month:D2}{currentUtc.Day:D2}";
            return version;
        }

        [HttpGet("calc/MD5/{value}")]
        [HttpGet("calc/{value}/MD5")]
        public string CalculateMD5(string value)
        {
            try
            {
                string modifiedValue = Regex.Replace(value, @"\d", "X");
                Console.Write(modifiedValue);
                using MD5 md5 = MD5.Create();
                byte[] inputBytes = Encoding.UTF8.GetBytes(modifiedValue);
                Console.WriteLine(inputBytes);
                byte[] hasByte = md5.ComputeHash(inputBytes);
                Console.WriteLine(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hasByte.Length; i++)
                {
                    sb.Append(hasByte[i].ToString("x2"));

                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                return $"Erro ao calcular o MD5: {ex.Message}";
            }
        }

        [HttpGet("password/strong/{value}")]
        [HttpGet("is-password-strong/{value}")]
        public bool IsPasswordStrong(string value)
        {

            string originalPassword = ConvertFromMd5Value(value);

            bool isStrongPassword = IsStrongPassword(originalPassword);
            return isStrongPassword;
        }

        private bool IsStrongPassword(string password)
        {
            return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@!*+-]).{12,}$");
        }

        private string ConvertFromMd5Value(string value)
        {
            return value.Replace("X", "0");
        }
    }
}
