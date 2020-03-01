using IrrigationController.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IrrigationController.Service
{
    public interface IPiService
    {
        Task<Response<List<Pi>>> GetAllPiAsync();
        Task<Response<Pi>> GetOnePiByIdAsync(int id);
        Task<Response<Pi>> CreatePiItemAsync(Pi item);
        Task<Response<object>> EditPiItemAsync(Pi item);
        Task<Response<object>> DeleteTodoItemAsync(Pi item);
    }
}
