using Newtonsoft.Json;
using Sales.Common.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Plugin.Connectivity;
using Sales.Helpers;
using System.Net.Http.Headers;

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
        public async Task<Response> GetList<T>(string pvUrlBase, string pvStrPrefix, string pvStrController, string pvTokenType, string pvAccessToken)
        {
            HttpClient vObjHttpClient;
            HttpResponseMessage vObjResponse;
            string vStrUrlMethod = $"/{pvStrPrefix}/{pvStrController}";

            try
            {
                vObjHttpClient = new HttpClient();
                vObjHttpClient.BaseAddress = new Uri(pvUrlBase);
                vObjHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(pvTokenType, pvAccessToken);
                vObjResponse = await vObjHttpClient.GetAsync(vStrUrlMethod);
                var vObjAnswer = await vObjResponse.Content.ReadAsStringAsync();

                if (!vObjResponse.IsSuccessStatusCode)
                {
                    return new Response
                    {
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
        public async Task<Response> Post<T>(string pvUrlBase, string pvStrPrefix, string pvStrController, T pvObjClassModel, string pvTokenType, string pvAccessToken)
        {

            HttpClient vObjHttpClient;
            HttpResponseMessage vObjResponse;
            HttpContent vObjContent;

            string vStrUrlMethod = $"/{pvStrPrefix}/{pvStrController}";

            try
            {
                vObjHttpClient = new HttpClient();
                vObjHttpClient.BaseAddress = new Uri(pvUrlBase);
                vObjHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(pvTokenType, pvAccessToken);
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

        public async Task<Response> Put<T>(string pvUrlBase, string pvStrPrefix, string pvStrController, T pvObjClassModel, int vStrId)
        {
            HttpClient vObjHttpClient;
            HttpResponseMessage vObjResponse;
            HttpContent vObjContent;

            string vStrUrlMethod = $"/{pvStrPrefix}/{pvStrController}/{vStrId}";

            try
            {
                vObjHttpClient = new HttpClient();
                vObjHttpClient.BaseAddress = new Uri(pvUrlBase);
                var vJSONClass = JsonConvert.SerializeObject(pvObjClassModel);
                vObjContent = new StringContent(vJSONClass, Encoding.UTF8, "application/json");
                vObjResponse = await vObjHttpClient.PutAsync(vStrUrlMethod, vObjContent);
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
        public async Task<Response> Put<T>(string pvUrlBase, string pvStrPrefix, string pvStrController, T pvObjClassModel, int vStrId, string pvTokenType, string pvAccessToken)
        {
            HttpClient vObjHttpClient;
            HttpResponseMessage vObjResponse;
            HttpContent vObjContent;

            string vStrUrlMethod = $"/{pvStrPrefix}/{pvStrController}/{vStrId}";

            try
            {
                vObjHttpClient = new HttpClient();
                vObjHttpClient.BaseAddress = new Uri(pvUrlBase);
                vObjHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(pvTokenType, pvAccessToken);
                var vJSONClass = JsonConvert.SerializeObject(pvObjClassModel);
                vObjContent = new StringContent(vJSONClass, Encoding.UTF8, "application/json");
                vObjResponse = await vObjHttpClient.PutAsync(vStrUrlMethod, vObjContent);
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
        public async Task<Response> Delete(string pvUrlBase, string pvStrPrefix, string pvStrController, int vStrId, string pvTokenType, string pvAccessToken)
        {
            HttpClient vObjHttpClient;
            HttpResponseMessage vObjResponse;
            string vStrUrlMethod = $"/{pvStrPrefix}/{pvStrController}/{vStrId}";
            try
            {
                vObjHttpClient = new HttpClient();
                vObjHttpClient.BaseAddress = new Uri(pvUrlBase);
                vObjHttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(pvTokenType, pvAccessToken);
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

        public async Task<TokenResponse> GetToken(string pvUrlBase, string pvStrUser, string pvStrPwd)
        {
            HttpContent vObjContent;
            HttpClient vObjHttpClient;
            try
            {
                vObjHttpClient = new HttpClient();
                vObjHttpClient.BaseAddress = new Uri(pvUrlBase);
                vObjContent = new StringContent($"grant_type=password&username={pvStrUser}&password={pvStrPwd}",
                                                Encoding.UTF8, 
                                                "application/x-www-form-urlencoded");
                var response = await vObjHttpClient.PostAsync("Token", vObjContent);
                var vStrResultJSON = await response.Content.ReadAsStringAsync();
                var vObjResult = JsonConvert.DeserializeObject<TokenResponse>(vStrResultJSON);
                return vObjResult;
            }
            catch
            {
                return null;
            }
        }
    }
}
