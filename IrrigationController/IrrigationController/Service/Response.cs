using System;
using System.Collections.Generic;
using System.Text;

namespace IrrigationController.Service
{
    public enum Status
    {
        SUCCESS, NOT_FOUND, OTHER_ERROR
    }

    public class Response<T> where T : class
    {
        public Status Status { get; private set; }
        public string StatusString { get; private set; }
        public T Data { get; private set; }

        public Response(Status status, string statusString, T data) 
        {
            Status = status;
            StatusString = statusString;
            Data = data;
        }
    }

    public class Response
    {
        public static Response<I> Success<I>(I data) where I : class => new Response<I>(Status.SUCCESS, "", data);
        public static Response<I> NotFound<I>(string msg, I data = null) where I : class => new Response<I>(Status.NOT_FOUND, msg, data);
        public static Response<I> Error<I>(string msg, I data = null) where I : class => new Response<I>(Status.OTHER_ERROR, msg, data);
    }
}
