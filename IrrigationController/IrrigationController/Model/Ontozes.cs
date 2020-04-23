using System;
using System.Collections.Generic;
using System.Text;

namespace IrrigationController.Model
{
    public enum OntozesUtasitas
    {
        KEZDES, VEGE
    }

    public class Ontozes
    {
        public OntozesUtasitas Utasitas { get; set; }
        public int? Hossz { get; set; }
    }
}
