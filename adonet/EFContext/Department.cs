﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adonet.EFContext
{
    public class Department
    {
        public Guid Id { get; set; }
        public String Name { get; set; } = null!;
        public DateTime? DeleteDt { get; set; }
    }
}