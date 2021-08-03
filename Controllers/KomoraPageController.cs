using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TraceabilityWebApi.Controllers
{
    public class KomoraPageController : Controller
    {
        // GET: /KomoraPage
        public string Index()
        {
            return "This is my default action...";
        }

        // 
        // GET: /KomoraPage/Welcome/ 

        public string Welcome()
        {
            return "This is the Welcome action method...";
        }
    }
}