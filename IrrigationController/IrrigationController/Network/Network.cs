﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace IrrigationController.Network
{
    class Network
    {
        private static readonly String ServerUrl = "https://192.168.1.106:45455/api";
        public static String PiGetAllUrl() => $"{ServerUrl}/pi";
        public static String PiGetOneUrl(int id) => $"{ServerUrl}/pi/{id}";
        public static String PiCreateUrl() => $"{ServerUrl}/pi";
        public static String PiEditUrl(int id) => $"{ServerUrl}/pi/{id}";
        public static String PiDeleteUrl(int id) => $"{ServerUrl}/pi/{id}";

        public static String ZonaGetAllUrl() => $"{ServerUrl}/zona";
        public static String ZonaGetOneUrl(int id) => $"{ServerUrl}/zona/{id}";
        public static String ZonaCreateUrl() => $"{ServerUrl}/zona";
        public static String ZonaEditUrl(int id) => $"{ServerUrl}/zona/{id}";
        public static String ZonaDeleteUrl(int id) => $"{ServerUrl}/zona/{id}";
    }
}
