using Itau.Common.Wrapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Itau.Common.Helpers
{
    public class ApiService
    {
        /// <summary>
        /// For Task Method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="action"></param>
        /// <param name="headers"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async static Task<Response<T>> PostAsync<T>(string url, string action, Dictionary<string, string> headers, dynamic model,
            AuthenticationHeaderValue auth = null
            )
        {
            Response<T> data = new Response<T>();
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(url + "/" + action),
                };
                if (auth != null)
                {
                    request.Headers.Authorization = auth;
                }
                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        request.Headers.Add(item.Key, item.Value);
                    }
                }
                if (model != null)
                {
                    request.Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                }

                HttpResponseMessage response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var dataObjects = await response.Content.ReadAsStringAsync();
                    Common.Diagnostics.ExceptionLogging.LogInfo($"Petición {dataObjects}");
                    ;
                    var result = JsonConvert.DeserializeObject<T>(dataObjects);
                    data.IsSuccess = true;
                    data.Result = result;
                }
                else
                {
                    data.IsSuccess = false;
                    data.Message = $"Lo sentimos ha ocurrido un error, {response.StatusCode}";
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }

            return data;
        }
    }
}