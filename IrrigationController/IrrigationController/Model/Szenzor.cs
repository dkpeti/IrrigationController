using System;
using System.Collections.Generic;
using System.Text;

namespace IrrigationController.Model
{
    public enum SzenzorTipus
    {
        Homerseklet, Talajnedvesseg
    }

    public class Szenzor
    {
        public int Id { get; set; }
        public string Nev { get; set; }
        public SzenzorTipus Tipus { get; set; }
        public string Megjegyzes { get; set; }
        public int ZonaId { get; set; }
        public int PiId { get; set; }
    }
}
