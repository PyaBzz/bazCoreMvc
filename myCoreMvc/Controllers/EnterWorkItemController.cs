﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using myCoreMvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace myCoreMvc
{
    public class EnterWorkItemController : Controller
    {
        public IActionResult Index(Guid id)
        {
            var item = DataProvider.Get<WorkItem>(wi => wi.Id == id);
            var inputModel = new EnterModel();
            if (item != null)
            {
                inputModel.Id = item.Id;
                inputModel.Reference = item.Reference;
                inputModel.Name = item.Name;
                inputModel.Priority = item.Priority;
            }
            return View("~/Views/ListOfWorkItems/EnterWorkItem.cshtml", inputModel);  // TODO: Use "asp-" tag helpers instead of tags attributes.
                                                                                      // TODO: See if you can minimise duplicate markup in the view.
        }

        [HttpPost]
        public IActionResult Index(EnterModel inputModel)
        {
            if (ModelState.IsValid)
            {
                var workItem = new WorkItem();
                workItem.CopyCommonPropertiesFrom(inputModel);  // We use this simple way to prevent malicious over-posting
                var result = "";
                switch (DataProvider.Save(workItem))
                {
                    case DataProvider.TransactionResult.Updated: result = "Item updated"; break;
                    case DataProvider.TransactionResult.Added: result = "New item added"; break;
                    default: result = "New item added"; break;
                }
                return RedirectToAction("Index", "ListOfWorkItems", new { message = result });  // Prevents form re-submission by refresh
            }
            else
            {
                var message = "Invalid: ";
                message += ModelState.Where(p => p.Value.ValidationState == ModelValidationState.Invalid).Select(p => p.Key).ToString(", ");
                message += "<br>" + " ______ Valid: ";
                message += ModelState.Where(p => p.Value.ValidationState == ModelValidationState.Valid).Select(p => p.Key).ToString(", ");
                inputModel.Message = message;  // TODO: Give specific binding error to the user
                return View("~/Views/ListOfWorkItems/EnterWorkItem.cshtml", inputModel);
            }
        }

        public class EnterModel : IClonable
        {
            public Guid Id { get; set; }
            public String Reference { get; set; }
            public int Priority { get; set; }
            [Display(Name = "Item name"), Required]
            public string Name { get; set; }
            public IEnumerable<int> PriorityChoices
            {
                get { return WorkItem.PriorityChoices; }
            }
            public string Message = "";
        }
    }
}
