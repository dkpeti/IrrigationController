﻿using IrrigationController.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IrrigationController.Service
{
    public interface IZonaService
    {
        Task<Response<List<Zona>>> GetAllZonaAsync();
        Task<Response<Zona>> GetOneZonaByIdAsync(int id);
        Task<Response<Zona>> CreateZonaItemAsync(Zona item);
        Task<Response<object>> EditZonaItemAsync(Zona item);
        Task<Response<object>> DeleteTodoItemAsync(Zona item);
    }
}
