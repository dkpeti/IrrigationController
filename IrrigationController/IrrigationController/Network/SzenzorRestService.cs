using IrrigationController.Model;
using IrrigationController.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        public Task<Response<object>> DeleteTodoItemAsync(Szenzor item)
        {
            throw new NotImplementedException();
        }

        public Task<Response<object>> EditSzenzorItemAsync(Szenzor item)
        {
            throw new NotImplementedException();
        }

        public Task<Response<List<Szenzor>>> GetAllSzenzorAsync()
        {
            throw new NotImplementedException();
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
            }
            catch (Exception ex)
            {
                return Response.Error(ex.Message, Szenzorok);
            }
            return Response.Error("", Szenzorok);
        }

        public Task<Response<Szenzor>> GetOneSzenzorByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
