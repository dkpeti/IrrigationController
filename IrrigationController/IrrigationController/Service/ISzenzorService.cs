using IrrigationController.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IrrigationController.Service
{
    public interface ISzenzorService
    {
        Task<Response<List<Szenzor>>> GetAllSzenzorAsync();
        Task<Response<List<Szenzor>>> GetAllSzenzorByPiIdAsync(int piId);
        Task<Response<List<Szenzor>>> GetAllSzenzorByZonaIdAsync(int zonaId);
        Task<Response<Szenzor>> GetOneSzenzorByIdAsync(int id);
        Task<Response<object>> EditSzenzorItemAsync(Szenzor item);
        Task<Response<object>> DeleteTodoItemAsync(Szenzor item);
    }
}
