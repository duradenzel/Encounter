﻿using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{

    public IActionResult Index() { return View(); }

}
