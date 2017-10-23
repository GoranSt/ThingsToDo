using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using ThingsToDo.App.Controllers;

namespace ThingsToDo.App.Helpers
{
    public abstract class BaseHelper : BaseController
    {
        public int getLoggedUserID()
        {
            return Thread.CurrentPrincipal.Identity.GetUserId<int>();
        }

        public string getLoggedUserName()
        {
            return Thread.CurrentPrincipal.Identity.GetUserName();
        }

        public string getName()
        {
            return Thread.CurrentPrincipal.Identity.Name;
        }
    }
}

