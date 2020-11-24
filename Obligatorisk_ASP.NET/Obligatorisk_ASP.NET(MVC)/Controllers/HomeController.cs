using Obligatorisk_ASP.NET_MVC_.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Obligatorisk_ASP.NET_MVC_.Controllers
{
    public class HomeController : Controller
    {
        public static VærktøjsudlejningEntities _db = new VærktøjsudlejningEntities();


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session.Remove("user");
            return Redirect("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "Email,Brugernavn,Password")] Kunde kunde)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<Kunde> kundeFound = null;


                //Finde kunden i databasen pba. brugnavn og password
                if (kunde.Brugernavn != null || kunde.Password != null)
                {
                    try
                    {
                        kundeFound =
                            from k in _db.KundeSet
                            where k.Brugernavn == kunde.Brugernavn && k.Password == kunde.Password
                            select k;
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        TempData["LoginError"] = "Error";
                    }
                }

                if (kundeFound != null)
                {
                    Kunde loginKunde = null;
                    try
                    {
                        loginKunde = _db.KundeSet.First(x => x.Brugernavn.Equals(kunde.Brugernavn)); // TODO: er det overhovedet nødvendig eller kan jeg bare bruge kundeSet fra metodesignaturen
                    }
                    catch(Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                        TempData["LoginError"] = "Error"; 
                        return View();
                    }
                    Session["user"] = loginKunde.Navn;
                    return RedirectToAction("Index", "Booking");
                }

            }
            return View();
        }


        public ActionResult OpretBruger()
        {
            return View();
        }


        // OBS: Sikkerheds-issue: Hvis man kender en anden brugers email og brugernavn kan man gå ind og ændre password for denne bruger.
        //      TODO: udsend en confirmation-email til brugeren om at forny password        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OpretBruger([Bind(Include = "Email,Brugernavn,Password")] Kunde kunde, string BekræftPasswordInput)
        {
            //hvis emailen kan findes og ikke har brugernavn eller password opdateres kunden med brugernavn og password
            if (ModelState.IsValid)
            {
                Kunde nyKunde = null;

                //Finde kunden i databasen pba. email
                if(kunde.Email != null)
                {
                    try
                    {
                        nyKunde = _db.KundeSet.First(x => x.Email.Equals(kunde.Email));
                    }catch(Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                    }
                }

                //Hvis kunden ikke kan findes i db
                if (nyKunde == null)
                {
                    TempData["buttonval"] = "Not found";
                    return View();
                }
                else
                {
                    //Hvis brugernavn eller password ikke er udfyldt
                    if(kunde.Brugernavn == null || kunde.Password == null)
                    {
                        TempData["buttonval"] = "Not filled";
                        return View();
                    }

                    // Tjekke om brugernavnet allerede eksisterer

                    Kunde brugerAllreadyExists = null; 

                    try
                    {
                        brugerAllreadyExists = _db.KundeSet.First(x => x.Brugernavn.Equals(kunde.Brugernavn));
                    }
                    catch(Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                    }

                    if (brugerAllreadyExists != null)
                    {
                        TempData["buttonval"] = "Brugeren eksisterer allerede";
                        return View();
                    }

                    nyKunde.Brugernavn = kunde.Brugernavn;


                    // Tjekke at password og confirm password stemmer overens

                    if (!PasswordHashed(kunde.Password).Equals(PasswordHashed(BekræftPasswordInput)))
                    {
                        TempData["buttonval"] = "Password stemmer ikke overens";
                        return View();
                    }

                    nyKunde.Password = PasswordHashed(kunde.Password);

                    _db.SaveChanges();

                    TempData["buttonval"] = "Found";
                }

            }
            return View();
        }

        private string PasswordHashed(string password)
        {
            var hashedPasswordBytes = Encoding.ASCII.GetBytes(password);
            var sha1 = new SHA1CryptoServiceProvider();
            var sha1data = sha1.ComputeHash(hashedPasswordBytes);
            var hashedPassword = ASCIIEncoding.UTF8.GetString(sha1data);
            return hashedPassword;
        }


    }
}