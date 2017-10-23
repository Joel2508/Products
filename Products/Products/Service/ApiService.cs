
namespace Products.Service
{
    using Plugin.Connectivity;
    using Models;
    using System.Threading.Tasks;
    using Response;
    using System.Net.Http;
    using System;
    using System.Text;
    using Newtonsoft.Json;
    using System.Net.Http.Headers;
    using System.Collections.Generic;

    public class ApiService
    {
        public async Task<Responses> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Responses
                {
                    IsSuccess = true,
                    Message = "Please turn on your internet setting.",
                };
            }

            var isRechable = await CrossConnectivity.Current.IsRemoteReachable("google.com");

            if (!isRechable)
            {
                return new Responses
                {
                    IsSuccess = false,
                    Message = "Check your connection the internet",
                };
            }

            return new Responses
            {
                IsSuccess = true,
                Message = "OK",
            };
        }


        public async Task<TokenResponse> GetToken(string urlBase, string useName, string password)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var response = await client.PostAsync("Token",
                    new StringContent(string.Format(
                   "grant_type=password&username={0}&password={1}", useName, password),
                    Encoding.UTF8, "application/x-www-form-urlencoded"));
                var resulJson = await response.Content.ReadAsStringAsync();
                var resutl = JsonConvert.DeserializeObject<TokenResponse>(resulJson);
                return resutl;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Responses> Get<T>(string urlBase, string servicePrefix, string controller,
            string tokenType, string accentToken, int id)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}/{2}", servicePrefix, controller, id);
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new Responses
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }
                var result = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<T>(result);
                return new Responses
                {
                    IsSuccess = true,
                    Message = "OK",
                    Result = model,
                };

            }
            catch (Exception ex)
            {
                return new Responses
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Responses> GetList<T>(string urlBase, string servicePrefix,
            string controller, string tokenType, string accentToken)
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue(tokenType,
                    accentToken);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Responses
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }
                var list = JsonConvert.DeserializeObject<List<T>>(result);
                return new Responses
                {
                    IsSuccess = true,
                    Message = "OK",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Responses
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Responses> GetList<T>(string urlBase, string serivePrefix, string controller,
            string tokenType, string accentToken, int id)
        {
            try
            {

                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}/{2}", serivePrefix, controller, id);
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new Responses
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }
                var result = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<T>>(result);

                return new Responses
                {
                    IsSuccess = true,
                    Message = "OK",
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Responses
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        public async Task<Responses> Post<T>(string urlBase, string servicePrefix, string controller,
            string tokenType, string accentToken, T model)
        {
            try
            {
                var requet = JsonConvert.SerializeObject(model);
                var content = new StringContent(requet, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url,content);

                if (!response.IsSuccessStatusCode)
                {
                    return new Responses
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var models = JsonConvert.DeserializeObject<T>(result);

                return new Responses
                {
                    IsSuccess = true,
                    Message = "OK",
                    Result = models,
                };
            }
            catch (Exception ex)
            {
                return new Responses
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Responses> Post<T>(string urlBase, string servicePrefix, string controller, T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}", servicePrefix, controller);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return new Responses
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var models = JsonConvert.DeserializeObject<T>(result);

                return new Responses
                {
                    IsSuccess = true,
                    Message = "OK",
                    Result = models,
                };

            }
            catch (Exception ex)
            {
                return new Responses
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
        public async Task<Responses> Put<T>(string urlBase, string servicePrefix, string controller,
            string tokenType, string accentType, T model)
        {
            try
            {
                var request = JsonConvert.SerializeObject(model);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType);
                client.BaseAddress = new Uri(urlBase);
                var url = string.Format("{0}{1}/{2}", servicePrefix, controller, model.GetHashCode());
                var response = await client.PutAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return new Responses
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }
                var result = await response.Content.ReadAsStringAsync();
                var newRecords = JsonConvert.DeserializeObject<T>(result);

                return new Responses
                {
                    IsSuccess=true,
                    Message="Record update OK",
                    Result=newRecords,                    
                };
            }
            catch (Exception ex)
            {
                return new Responses
                {
                    IsSuccess = false,
                    Message=ex.Message
                };
            }
        }

        public async Task<Responses> Delete<T>(string urlBase, string servicePrefix, string controller,
            string tokenType, string accentType, T model)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType);
                var url = string.Format("{0}{1}/{2}", servicePrefix, controller, model.GetHashCode());
                var response = await client.DeleteAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return new Responses
                    {
                        IsSuccess = false,
                        Message = response.StatusCode.ToString(),
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var deleteModel = JsonConvert.DeserializeObject<T>(result);

                return new Responses
                {
                    IsSuccess=true,
                    Message= "Record delete OK",
                    Result = deleteModel,
                };
            }
            catch (Exception ex)
            {
                return new Responses
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
    }
}
