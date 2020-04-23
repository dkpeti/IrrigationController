using IrrigationController.Model;
using IrrigationController.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IrrigationController.Network
{
    public class SzenzorRestService : ISzenzorService
    {
        private HttpAPI _httpAPI;
        private List<Szenzor> Szenzorok { get; set; }

        public SzenzorRestService(HttpAPI httpAPI)
        {
            _httpAPI = httpAPI;
        }

        public async Task<Response<object>> DeleteTodoItemAsync(Szenzor item)
        {
            var uri = new Uri(_httpAPI.SzenzorDeleteUrl(item.Id));
            try
            {
                var response = await _httpAPI.HttpClient.DeleteAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    return Response.Success<object>(null);
                }
                else
                {
                    var rescontent = await response.Content.ReadAsStringAsync();
                    return Response.Error<object>(rescontent);
                }
            }
            catch (Exception ex)
            {
                return Response.Error<object>(ex.Message);
            }
        }

        public async Task<Response<object>> EditSzenzorItemAsync(Szenzor item)
        {
            var uri = new Uri(_httpAPI.SzenzorEditUrl(item.Id));
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
                var response = await _httpAPI.HttpClient.PutAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    return Response.Success<object>(null);
                }
                else
                {
                    var rescontent = await response.Content.ReadAsStringAsync();
                    return Response.Error<object>(rescontent);
                }
            }
            catch (Exception ex)
            {
                return Response.Error<object>(ex.Message);
            }
        }

        public Task<Response<List<Szenzor>>> GetAllSzenzorAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Response<List<Szenzor>>> GetAllSzenzorByPiIdAsync(int piId)
        {
            Szenzorok = new List<Szenzor>();

            var uri = new Uri(_httpAPI.SzenzorGetAllByPiIdUrl(piId));
            try
            {
                var response = await _httpAPI.HttpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Szenzorok = JsonConvert.DeserializeObject<List<Szenzor>>(content);
                    return Response.Success(Szenzorok);
                }
                else
                {
                    var rescontent = await response.Content.ReadAsStringAsync();
                    return Response.Error<List<Szenzor>>(rescontent);
                }
            }
            catch (Exception ex)
            {
                return Response.Error(ex.Message, Szenzorok);
            }
        }

        public async Task<Response<List<Szenzor>>> GetAllSzenzorByZonaIdAsync(int zonaId)
        {
            Szenzorok = new List<Szenzor>();

            var uri = new Uri(_httpAPI.SzenzorGetAllByZonaIdUrl(zonaId));
            try
            {
                var response = await _httpAPI.HttpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Szenzorok = JsonConvert.DeserializeObject<List<Szenzor>>(content);
                    return Response.Success(Szenzorok);
                }
                else
                {
                    var rescontent = await response.Content.ReadAsStringAsync();
                    return Response.Error<List<Szenzor>>(rescontent);
                }
            }
            catch (Exception ex)
            {
                return Response.Error(ex.Message, Szenzorok);
            }
        }

        public async Task<Response<Szenzor>> GetOneSzenzorByIdAsync(int id)
        {
            var uri = new Uri(_httpAPI.SzenzorGetOneUrl(id));
            try
            {
                var response = await _httpAPI.HttpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var szenzor = JsonConvert.DeserializeObject<Szenzor>(content);
                    return Response.Success(szenzor);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return Response.NotFound<Szenzor>("Nincs ilyen szenzor");
                } 
                else
                {
                    var rescontent = await response.Content.ReadAsStringAsync();
                    return Response.Error<Szenzor>(rescontent);
                }
            }
            catch (Exception ex)
            {
                return Response.Error<Szenzor>(ex.Message);
            }
        }
    }
}
