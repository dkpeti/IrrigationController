using IrrigationController.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IrrigationController.Service
{
    public interface IMeresService
    {
        Task<Response<List<Meres>>> GetAllMeresAsync();
        Task<Response<List<Meres>>> GetAllMeresBySzenzorIdAsync(int szenzorId);
    }
}
