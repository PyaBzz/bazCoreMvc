﻿using Microsoft.AspNetCore.Mvc;
using myCoreMvc.App;
using myCoreMvc.App.Providing;
using Baz.CoreMvc;
using System;

namespace myCoreMvc.UI.Controllers
{
    [Area("Users")]
    public class UserDetailsController : BaseController
    {
        private readonly IUserBiz UserBiz;

        public UserDetailsController(IUserBiz userBiz)
            => UserBiz = userBiz;

        public IActionResult Index(Guid id)
        {
            var viewModel = UserBiz.Get(id);
            return View("UserDetails", viewModel);
        }

        public IActionResult Delete(Guid id)
        {
            var result = "";
            var user = UserBiz.Get(id);
            switch (UserBiz.Of(user).Delete())
            {
                case TransactionResult.NotFound: result = "Found no User with the provided Id."; break;
                case TransactionResult.Deleted: result = "Item deleted."; break;
                default: result = "Found no User with the provided Id."; break;
            }
            return RedirectToAction(nameof(UserListController.Index), Short<UserListController>.Name, new { area = "Users", message = result });  // Prevents re-submission by refresh
        }
    }
}
