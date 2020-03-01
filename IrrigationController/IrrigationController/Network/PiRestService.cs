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
        private HttpClient _client;
        public List<Pi> Pik { get; private set; }

        public PiRestService()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            _client = new HttpClient(clientHandler);
        }
        public async Task<Response<List<Pi>>> GetAllPiAsync()
        {
            Pik = new List<Pi>();

            var uri = new Uri(Network.PiGetAllUrl());
            try
            {
                var response = await _client.GetAsync(uri);
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
            var uri = new Uri(Network.PiGetOneUrl(id));
            try
            {
                var response = await _client.GetAsync(uri);
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
            var uri = new Uri(Network.PiCreateUrl());
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(uri, content);
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
            var uri = new Uri(Network.PiEditUrl(item.Id));
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
                var response = await _client.PutAsync(uri, content);
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
            var uri = new Uri(Network.PiDeleteUrl(item.Id));
            try
            {
                var response = await _client.DeleteAsync(uri);
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
