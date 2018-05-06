﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PooyasFramework;

namespace myCoreMvc.Models
{
    public class WorkItem : Thing, IClonable
    { //TODO: Organise these into sections for Properties, Fields and Methods.
        public String Reference { get; set; }
        public int Priority { get; set; }
        [Display(Name = "Item name"), Required]
        public string Name { get; set; }
        public WorkPlan WorkPlan { get; set; }
        public override string ToString() => Name;

        public static IEnumerable<int> PriorityChoices { get; } = new List<int> { 1, 2, 3, 4 };
    }
}
