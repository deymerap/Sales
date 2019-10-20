using Newtonsoft.Json;
using Sales.Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Sales.Helpers;

namespace Sales.Services
{
    class ApiService
    {
        public async Task<Response> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message= Languages.TurnOnInternet,
                };
            }

            var vStrIsReachable = await CrossConnectivity.Current.IsRemoteReachable("google.com");
            if (!vStrIsReachable)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = Languages.NoInternet,
                };
            }

            return new Response
            {
                IsSuccess = true,
            };
        }

        public async Task<Response>GetList<T>(string pvUrlBase ,string pvStrPrefix, string pvStrController)
        {
            HttpClient vObjHttpClient;
            HttpResponseMessage vObjResponse;
            string vStrUrlMethod = $"/{pvStrPrefix}/{pvStrController}";

            try
            {
                vObjHttpClient = new HttpClient();
                vObjHttpClient.BaseAddress = new Uri(pvUrlBase);

                vObjResponse = await  vObjHttpClient.GetAsync(vStrUrlMethod);
                var vObjAnswer = await vObjResponse.Content.ReadAsStringAsync();

                if (!vObjResponse.IsSuccessStatusCode)
                {
                    return new Response {
                        IsSuccess = false,
                        Message = vObjAnswer,
                    };
                }

                var vLista = JsonConvert.DeserializeObject<List<T>>(vObjAnswer);

                return new Response
                {
                    IsSuccess = true,
                    Message = "",
                    Result = vLista,
                };            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> Post<T>(string pvUrlBase, string pvStrPrefix, string pvStrController, T pvObjClassModel)
        {

            HttpClient vObjHttpClient;
            HttpResponseMessage vObjResponse;
            HttpContent vObjContent;

            string vStrUrlMethod = $"/{pvStrPrefix}/{pvStrController}";

            try
            {
                vObjHttpClient = new HttpClient();
                vObjHttpClient.BaseAddress = new Uri(pvUrlBase);
                var vJSONClass = JsonConvert.SerializeObject(pvObjClassModel);
                vObjContent = new StringContent(vJSONClass, Encoding.UTF8, "application/json");
                vObjResponse = await vObjHttpClient.PostAsync(vStrUrlMethod, vObjContent);
                var vObjAnswer = await vObjResponse.Content.ReadAsStringAsync();

                if (!vObjResponse.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = vObjAnswer,
                    };
                }

                var vObjClassRespnse = JsonConvert.DeserializeObject<T>(vObjAnswer);

                return new Response
                {
                    IsSuccess = true,
                    Message = "",
                    Result = vObjClassRespnse,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }


        public async Task<Response> Delete(string pvUrlBase, string pvStrPrefix, string pvStrController, int vStrId)
        {
            HttpClient vObjHttpClient;
            HttpResponseMessage vObjResponse;
            string vStrUrlMethod = $"/{pvStrPrefix}/{pvStrController}/{vStrId}";

            try
            {
                vObjHttpClient = new HttpClient();
                vObjHttpClient.BaseAddress = new Uri(pvUrlBase);

                vObjResponse = await vObjHttpClient.DeleteAsync(vStrUrlMethod);
                var vObjAnswer = await vObjResponse.Content.ReadAsStringAsync();

                if (!vObjResponse.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = vObjAnswer,
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = "",
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

    }
}
