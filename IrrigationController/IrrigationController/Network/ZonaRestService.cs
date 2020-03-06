using IrrigationController.Model;
using IrrigationController.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IrrigationController.Network
{
    class ZonaRestService : IZonaService
    {
        private HttpAPI _httpAPI;
        public List<Zona> Zonak { get; private set; }

        public ZonaRestService(HttpAPI httpAPI)
        {
            this._httpAPI = httpAPI;
        }

        public async Task<Response<List<Zona>>> GetAllZonaAsync()
        {
            Zonak = new List<Zona>();

            var uri = new Uri(_httpAPI.ZonaGetAllUrl());
            try
            {
                var response = await _httpAPI.HttpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Zonak = JsonConvert.DeserializeObject<List<Zona>>(content);
                    return Response.Success(Zonak);
                }
            }
            catch (Exception ex)
            {
                return Response.Error(ex.Message, Zonak);
            }
            return Response.Error("", Zonak);
        }

        public async Task<Response<Zona>> GetOneZonaByIdAsync(int id)
        {
            var uri = new Uri(_httpAPI.ZonaGetOneUrl(id));
            try
            {
                var response = await _httpAPI.HttpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var zona = JsonConvert.DeserializeObject<Zona>(content);
                    return Response.Success(zona);
                }
                else if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return Response.NotFound<Zona>("Nincs ilyen zóna");
                }
            }
            catch (Exception ex)
            {
                return Response.Error<Zona>(ex.Message);
            }
            return Response.Error<Zona>("");
        }

        public async Task<Response<Zona>> CreateZonaItemAsync(Zona item)
        {
            var uri = new Uri(_httpAPI.ZonaCreateUrl());
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
                var response = await _httpAPI.HttpClient.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    var rescontent = await response.Content.ReadAsStringAsync();
                    var zona = JsonConvert.DeserializeObject<Zona>(rescontent);
                    return Response.Success(zona);
                }
            }
            catch (Exception ex)
            {
                return Response.Error<Zona>(ex.Message);
            }

            return Response.Error<Zona>("");
        }

        public async Task<Response<object>> EditZonaItemAsync(Zona item)
        {
            var uri = new Uri(_httpAPI.ZonaEditUrl(item.Id));
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
                var response = await _httpAPI.HttpClient.PutAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    return Response.Success<object>(null);
                }
            }
            catch (Exception ex)
            {
                return Response.Error<object>(ex.Message);
            }

            return Response.Error<object>("");
        }

        public async Task<Response<object>> DeleteTodoItemAsync(Zona item)
        {
            var uri = new Uri(_httpAPI.ZonaDeleteUrl(item.Id));
            try
            {
                var response = await _httpAPI.HttpClient.DeleteAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    return Response.Success<object>(null);
                }
            }
            catch (Exception ex)
            {
                return Response.Error<object>(ex.Message);
            }

            return Response.Error<object>("");
        }
    }
}
