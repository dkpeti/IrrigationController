using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace IrrigationController.Class
{
    public class Zona :RealmObject
    {
        [PrimaryKey]
        public long ZonaId
        { get; set; }
        public string Nev
        { get; set; }
    }
}
