using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xamarin.Forms;

namespace IrrigationController.Network
{
    public class HttpAPI
    {
        private readonly string ServerUrl;

        public string LoginCheckUrl() => $"{ServerUrl}/felhasznalo";
        public string GoogleLoginUrl() => $"{ServerUrl}/felhasznalo/google";
               
        public string PiGetAllUrl() => $"{ServerUrl}/pi";
        public string PiGetOneUrl(int id) => $"{ServerUrl}/pi/{id}";
        public string PiCreateUrl() => $"{ServerUrl}/pi";
        public string PiEditUrl(int id) => $"{ServerUrl}/pi/{id}";
        public string PiDeleteUrl(int id) => $"{ServerUrl}/pi/{id}";
               
        public string ZonaGetAllUrl() => $"{ServerUrl}/zona";
        public string ZonaGetAllByPiIdUrl(int piId) => $"{ServerUrl}/zona?piId={piId}";
        public string ZonaGetOneUrl(int id) => $"{ServerUrl}/zona/{id}";
        public string ZonaCreateUrl() => $"{ServerUrl}/zona";
        public string ZonaEditUrl(int id) => $"{ServerUrl}/zona/{id}";
        public string ZonaDeleteUrl(int id) => $"{ServerUrl}/zona/{id}";

        public string SzenzorGetAllByZonaIdUrl(int zonaId) => $"{ServerUrl}/szenzor?zonaId={zonaId}";
        public string SzenzorGetAllByPiIdUrl(int piId) => $"{ServerUrl}/szenzor?piId={piId}";
        public string SzenzorGetOneUrl(int id) => $"{ServerUrl}/szenzor/{id}";

        public string MeresGetAllUrl() => $"{ServerUrl}/meres";
        public string MeresGetAllBySzenzorIdUrl(int szenzorId) => $"{ServerUrl}/meres?szenzorId={szenzorId}";

        private string _authToken;
        public HttpClient HttpClient { get; private set; }

        public HttpAPI(string ServerUrl)
        {
            this.ServerUrl = ServerUrl;

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            HttpClient = new HttpClient(clientHandler);
        }

        public void SetLoginToken(string token)
        {
            _authToken = token;
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authToken);
        }
    }
}
