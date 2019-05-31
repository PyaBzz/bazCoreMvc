﻿using System;
using System.Linq;
using myCoreMvc.Domain;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PyaFramework.Core;
using myCoreMvc.App;
using myCoreMvc.App.Providing;
using PyaFramework.CoreMvc;

namespace myCoreMvc.UI.Controllers
{
    [Area("WorkPlans")]
    public class WorkPlanEnterController : BaseController
    {
        private readonly IWorkPlanBiz WorkPlanBiz;

        public WorkPlanEnterController(IWorkPlanBiz workPlanBiz)
            => WorkPlanBiz = workPlanBiz;

        public IActionResult Index(Guid id)
        {
            var inputModel = new EnterModel();
            if (id != Guid.Empty)
            {
                var workPlan = WorkPlanBiz.Get(id);
                if (workPlan != null) inputModel.CopySimilarPropertiesFrom(workPlan);
            }
            return View("WorkPlanEnter", inputModel);
        }

        [HttpPost]
        public IActionResult Index(EnterModel inputModel)
        {
            if (ModelState.IsValid)
            {
                WorkPlan workPlan;
                TransactionResult transactionResult;
                if (inputModel.Id == Guid.Empty)
                {
                    workPlan = new WorkPlan();
                    workPlan.CopySimilarPropertiesFrom(inputModel);  // Prevents malicious over-posting
                    transactionResult = WorkPlanBiz.Of(workPlan).Add();
                }
                else
                {
                    workPlan = WorkPlanBiz.Get(inputModel.Id);
                    workPlan.CopySimilarPropertiesFrom(inputModel);  // Prevents malicious over-posting
                    transactionResult = WorkPlanBiz.Of(workPlan).Update();
                }
                var resultMessage = "";
                switch (transactionResult)
                {
                    case TransactionResult.Updated: resultMessage = "Item updated"; break;
                    case TransactionResult.Added: resultMessage = "New item added"; break;
                    default: resultMessage = transactionResult.ToString(); break;
                }
                return RedirectToAction(nameof(WorkPlanListController.Index), ShortName.Of<WorkPlanListController>(), new { message = resultMessage });  // Prevents re-submission by refresh
            }
            else
            {
                inputModel.Message = "Invalid values for: "
                    + ModelState.Where(p => p.Value.ValidationState == ModelValidationState.Invalid).Select(p => p.Key).ToString(", ");
                return View("WorkPlanEnter", inputModel);
            }
        }

        public class EnterModel : IClonable
        {
            public Guid Id { get; set; }

            [Display(Name = "Plan name")]
            [Required(ErrorMessage = "{0} is mandatory.")]
            [StringLength(16, MinimumLength = 3, ErrorMessage = "{0} should be between {2} and {1} characters in length.")]
            [RegularExpression("^[A-Z][a-zA-Z0-9]*", ErrorMessage = "{0} must start with a capital letter and may only contain alphanumeric characters.")]
            public string Name { get; set; }

            public string Message = "";
        }
    }
}
