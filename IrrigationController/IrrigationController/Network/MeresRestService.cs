using IrrigationController.Model;
using IrrigationController.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IrrigationController.Network
{
    public class MeresRestService : IMeresService
    {
        private HttpAPI _httpAPI;
        private List<Meres> Meresek { get; set; }

        public MeresRestService(HttpAPI httpAPI)
        {
            _httpAPI = httpAPI;
        }
        public async Task<Response<List<Meres>>> GetAllMeresAsync()
        {
            Meresek = new List<Meres>();

            var uri = new Uri(_httpAPI.MeresGetAllUrl());
            try
            {
                var response = await _httpAPI.HttpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Meresek = JsonConvert.DeserializeObject<List<Meres>>(content);
                    return Response.Success(Meresek);
                }
            }
            catch (Exception ex)
            {
                return Response.Error(ex.Message, Meresek);
            }
            return Response.Error("", Meresek);
        }

        public async Task<Response<List<Meres>>> GetAllMeresBySzenzorIdAsync(int szenzorId)
        {
            Meresek = new List<Meres>();
            var uri = new Uri(_httpAPI.MeresGetAllBySzenzorIdUrl(szenzorId));
            try
            {
                var response = await _httpAPI.HttpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Meresek = JsonConvert.DeserializeObject<List<Meres>>(content);
                    return Response.Success(Meresek);
                }
            }
            catch (Exception ex)
            {
                return Response.Error(ex.Message, Meresek);
            }
            return Response.Error("", Meresek);
        }
    }
}
