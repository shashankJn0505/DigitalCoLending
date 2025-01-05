using CoLending.Core.Options;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace CoLending.API.Middleware
{
    public class AesEncryptDecryptMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly byte[] _key;
        private readonly byte[] _IV;
        private readonly ConfigurationOptions _configuration;
        private readonly string[] _pathData;
        public AesEncryptDecryptMiddleware(RequestDelegate next, IOptions<ConfigurationOptions> configuration)
        {
            _next = next;
            _configuration = configuration.Value;
            _key = Encoding.UTF8.GetBytes(_configuration.ReqEncryptKey);
            _IV = Encoding.UTF8.GetBytes(_configuration.ReqEncryptIV);
            _pathData = new string[4] { "/EncryptString", "/DecryptString", "/EncryptObject", "/DecryptObject" };
        }

        public async Task Invoke(HttpContext context)
        {
            if (!string.IsNullOrEmpty(context.Request.Path) && !_pathData.Contains(context.Request.Path.Value))
            {
                // Intercept the request body
                context.Request.EnableBuffering();

                var decryptedRequestBody = string.Empty;
                var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();

                if (context.Request.Path == "/api/HunterFeedback/SaveFeedbackRequset")  // condition add for hunterfeedback api for remove encryption and decryptio
                {
                    decryptedRequestBody = requestBody;
                }
                else if (context.Request.Path == "/api/Login/fetch-tokenDetails")  // condition add for hunterfeedback api for remove encryption and decryptio
                {
                    decryptedRequestBody = requestBody;
                }

                else
                {
                    // Decrypt the request body
                    decryptedRequestBody = _configuration.EncryptDecryptEnable == 1 ? Decrypt(requestBody) : requestBody;
                }
                // Create a new request stream with the decrypted body
                var requestStream = new MemoryStream(Encoding.UTF8.GetBytes(decryptedRequestBody));
                context.Request.Body = requestStream;


            }

            // Continue the pipeline
            await _next(context);

            if (!string.IsNullOrEmpty(context.Request.Path) && !_pathData.Contains(context.Request.Path.Value))
            {
                var encryptedResponseBody = string.Empty;
                // Encrypt the response body
                var responseBody = await GetResponseBody(context.Response);
                if (context.Request.Path == "/api/HunterFeedback/SaveFeedbackRequset")  // condition add for hunterfeedback api for remove encryption and decryptio
                {
                    encryptedResponseBody = responseBody;
                }
                if (context.Request.Path == "/api/Login/fetch-tokenDetails")  // condition add for hunterfeedback api for remove encryption and decryptio
                {
                    encryptedResponseBody = responseBody;
                }
                else
                {
                    encryptedResponseBody = _configuration.EncryptDecryptEnable == 1 ? Encrypt(responseBody) : responseBody;
                }



                // Set the encrypted response body
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(encryptedResponseBody);
            }
        }

        private string Decrypt(dynamic body)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _IV;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;
                aes.FeedbackSize = 128 / 8;
                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    var cipherBytes = Convert.FromBase64String(body);
                    var decryptedBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }

        private string Encrypt(dynamic body)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _IV;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;
                aes.FeedbackSize = 128 / 8;
                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    var plainBytes = Encoding.UTF8.GetBytes(body);
                    var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                    string encryptedstring = Convert.ToBase64String(encryptedBytes);
                    return JsonSerializer.Serialize(encryptedstring);
                }
            }
        }

        private async Task<string> GetResponseBody(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return responseBody;
        }
    }
}
