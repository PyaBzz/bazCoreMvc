﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Baz.Core;

namespace myCoreMvc.Domain
{
    public class Thing : IThing, IClonable
    {
        public Guid Id { get; set; }
    }
}
