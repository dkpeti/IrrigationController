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
    class PiRestService : IPiService
    {
        private HttpAPI _httpAPI;
        private List<Pi> Pik { get; set; }

        public PiRestService(HttpAPI httpAPI)
        {
            this._httpAPI = httpAPI;
        }
        public async Task<Response<List<Pi>>> GetAllPiAsync()
        {
            Pik = new List<Pi>();

            var uri = new Uri(_httpAPI.PiGetAllUrl());
            try
            {
                var response = await _httpAPI.HttpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Pik = JsonConvert.DeserializeObject<List<Pi>>(content);
                    return Response.Success(Pik);
                }
            }
            catch (Exception ex)
            {
                return Response.Error(ex.Message, Pik);
            }
            return Response.Error("", Pik);
        }
        public async Task<Response<Pi>> GetOnePiByIdAsync(int id)
        {
            var uri = new Uri(_httpAPI.PiGetOneUrl(id));
            try
            {
                var response = await _httpAPI.HttpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var pi = JsonConvert.DeserializeObject<Pi>(content);
                    return Response.Success(pi);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return Response.NotFound<Pi>("Nincs ilyen pi");
                }
            }
            catch (Exception ex)
            {
                return Response.Error<Pi>(ex.Message);
            }
            return Response.Error<Pi>("");
        }
        public async Task<Response<Pi>> CreatePiItemAsync(Pi item)
        {
            var uri = new Uri(_httpAPI.PiCreateUrl());
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
                var response = await _httpAPI.HttpClient.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    var rescontent = await response.Content.ReadAsStringAsync();
                    var pi = JsonConvert.DeserializeObject<Pi>(rescontent);
                    return Response.Success(pi);
                }
            }
            catch (Exception ex)
            {
                return Response.Error<Pi>(ex.Message);
            }

            return Response.Error<Pi>("");
        }
        public async Task<Response<object>> EditPiItemAsync(Pi item)
        {
            var uri = new Uri(_httpAPI.PiEditUrl(item.Id));
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
        public async Task<Response<object>> DeleteTodoItemAsync(Pi item)
        {
            var uri = new Uri(_httpAPI.PiDeleteUrl(item.Id));
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
