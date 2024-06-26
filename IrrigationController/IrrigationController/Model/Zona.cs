﻿using System;
using System.Collections.Generic;
using System.Text;

namespace IrrigationController.Model
{
    public class Zona
    {
        public int Id { get; set; }
        public string Nev { get; set; }
        public int PiId { get; set; }
        public int[] SzenzorLista { get; set; }
        public DateTime? UtolsoOntozesKezdese { get; set; }
        public int? UtolsoOntozesHossza { get; set; }
    }
}
