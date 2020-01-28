using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Random_PassCode.Models;

// Add this line in Models to use session
using Microsoft.AspNetCore.Http;


namespace Random_PassCode.Controllers
{
    public class HomeController : Controller
    {


        private string SessionPasscode
        {
            get { return HttpContext.Session.GetString("passcode"); }
            set { HttpContext.Session.SetString("passcode", value); }
        }

        private int? SessionCount
        {
            get { return HttpContext.Session.GetInt32("count"); }
            set { HttpContext.Session.SetInt32("count", (int)value); }
        }

        private int? SessionHappiness
        {
            get { return HttpContext.Session.GetInt32("happiness"); }
            set { HttpContext.Session.SetInt32("happiness", (int)value); }
        }

        private int? SessionFullness
        {
            get { return HttpContext.Session.GetInt32("fullness"); }
            set { HttpContext.Session.SetInt32("fullness", (int)value); }
        }

        private int? SessionEnergy
        {
            get { return HttpContext.Session.GetInt32("energy"); }
            set { HttpContext.Session.SetInt32("energy", (int)value); }
        }

        private int? SessionMeals
        {
            get { return HttpContext.Session.GetInt32("meals"); }
            set { HttpContext.Session.SetInt32("meals", (int)value); }
        }

        private int? PictureID
        {
            get { return HttpContext.Session.GetInt32("pictureID"); }
            set { HttpContext.Session.SetInt32("pictureID", (int)value); }
        }

        private int? OddsID
        {
            get { return HttpContext.Session.GetInt32("oddsID"); }
            set { HttpContext.Session.SetInt32("oddsID", (int)value); }
        }


        public string GeneratePasscode()
        {
            string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            string result = "";
            Random rand = new Random();
            for (var i = 1; i <= 15; i++)
                result += Characters[rand.Next(Characters.Length)];
            return result;
        }
        public IActionResult Index()
        {
            // *Inside controller methods*
            // To store a string in session we use ".SetString"
            // The first string passed is the key and the second is the value we want to retrieve later
            HttpContext.Session.SetString("UserName", "Samantha");
            // To retrieve a string from session we use ".GetString"
            string LocalVariable = HttpContext.Session.GetString("UserName");

            // To store an int in session we use ".SetInt32"
            HttpContext.Session.SetInt32("UserAge", 28);
            // To retrieve an int from session we use ".GetInt32"
            int? IntVariable = HttpContext.Session.GetInt32("UserAge");
            // in your Controller
            ViewBag.Count = HttpContext.Session.GetInt32("count");
            if (ViewBag.Count >= 0)
            {
                ViewBag.Count += 1;
            }

            {

                if (SessionHappiness == null)
                    SessionHappiness = 20;
                if (SessionFullness == null)
                    SessionFullness = 20;
                if (SessionEnergy == null)
                    SessionEnergy = 50;
                if (SessionMeals == null)
                    SessionMeals = 3;
                if (SessionPasscode == null)
                    SessionPasscode = "Generate a passcode";
                if (SessionCount == null)
                    SessionCount = 0;
                if (PictureID == null)
                    PictureID = 0;

                ViewBag.Happiness = SessionHappiness;
                ViewBag.Fullness = SessionFullness;
                ViewBag.Energy = SessionEnergy;
                ViewBag.Meals = SessionMeals;
                ViewBag.PictureID = PictureID;

                ViewBag.Passcode = SessionPasscode;
                ViewBag.Count = SessionCount;
                return View();
            }
        }

        [HttpPost("generate")]
        public IActionResult Generate()
        {
            SessionCount++;
            SessionPasscode = GeneratePasscode();
            return RedirectToAction("Index");
        }

        [HttpPost("reset")]
        public IActionResult Reset()
        {
            HttpContext.Session.Clear();
            // SessionCount = 0;
            // SessionPasscode = "Generate a Passcode";
            return RedirectToAction("Index");
        }

        [HttpPost("feed")]
        public IActionResult Feed()
        {
            Random rand = new Random();
            OddsID = rand.Next(1,4);
            if(OddsID == 1)
            {
                SessionMeals--;
                PictureID = 5;
                return RedirectToAction("Index");
            }
            else
            {
                if (SessionMeals == 0)
                {
                    SessionMeals = SessionMeals;
                    return RedirectToAction("Index");
                }
                else
                {
                    SessionMeals--;
                    PictureID = 1;
                    SessionFullness += rand.Next(5, 10);
                    return RedirectToAction("Index");
                }
            }
        }

        [HttpPost("play")]
        public IActionResult Play()
        {
            Random rand = new Random();
            OddsID = rand.Next(1,4);
            if(OddsID == 1)
            {
                PictureID = 5;
                SessionEnergy -= 5;
                return RedirectToAction("Index");
            }
            else
            {
                if (SessionEnergy < 1)
                {
                    SessionEnergy = SessionEnergy;
                    return RedirectToAction("Index");
                }
                else
                {
                    SessionEnergy -= 5;
                    PictureID = 2;
                    SessionHappiness += rand.Next(5, 10);
                    return RedirectToAction("Index");
                }
            }
        }
        

        [HttpPost("work")]
        public IActionResult Work()
        {
            if (SessionEnergy < 1)
            {
                SessionEnergy = SessionEnergy;
                return RedirectToAction("Index");
            }
            else
            {
                SessionEnergy -= 5;
                PictureID = 3;
                Random rand = new Random();
                SessionMeals += rand.Next(1, 3);
                return RedirectToAction("Index");
            }
        }

        [HttpPost("sleep")]
        public IActionResult Sleep()
        {
            if (SessionFullness < 1 || SessionHappiness < 1)
            {
                SessionFullness = SessionFullness;
                SessionHappiness = SessionHappiness;
                return RedirectToAction("Index");
            }
            else
            {
                PictureID = 4;
                SessionFullness -= 5;
                SessionHappiness -= 5;
                SessionEnergy += 15;
                return RedirectToAction("Index");
            }
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
