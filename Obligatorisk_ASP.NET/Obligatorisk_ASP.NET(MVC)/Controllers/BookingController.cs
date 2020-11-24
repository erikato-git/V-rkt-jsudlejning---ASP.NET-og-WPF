using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Obligatorisk_ASP.NET_MVC_.Models;

namespace Obligatorisk_ASP.NET_MVC_.Controllers
{
    public class BookingController : Controller
    {
        private VærktøjsudlejningEntities _db = new VærktøjsudlejningEntities();

        // GET: BookingSets
        public ActionResult Index()
        {
            Kunde kunde = null;
            List<Booking> bookinger = new List<Booking>();

            if (Session["user"] != null)
            {
                string tmp = Session["user"] as String;

                try
                {
                    kunde = _db.KundeSet.First(x => x.Navn.Equals(tmp));
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }

                if (kunde != null)
                {

                    bookinger = _db.BookingSet.Where(u => u.KundeKundenummerID == kunde.KundenummerID).ToList();

                }
            }
            return View(bookinger);
        }

        // GET: BookingSets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = _db.BookingSet.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        public ActionResult Søg(string DateFra, string DateTil)
        {
            if(DateTil.CompareTo(DateFra) < 0)
            {
                TempData["SearchError"] = "Ugyldig";
                return Redirect("Index");
            }

            TempData["SearchData"] = DateFra + ";" + DateTil;
            return RedirectToAction("Index", "Værktøj");
        }


        // GET: BookingSets/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BookingSets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DateTime AfhentningstidspunktInput, int AntalDøgnInput)
        {
            //Find kundeId
            string sessionName = Session["user"] as String;
            Kunde kunde = null;
            Booking newBooking = new Booking();

            newBooking.Afhentningstidspunkt = AfhentningstidspunktInput.Date;
            newBooking.AntalDøgn = AntalDøgnInput;
            newBooking.Status = "reserveret";

            try
            {
                kunde = _db.KundeSet.First(x => x.Navn.Equals(sessionName));
            }
            catch (Exception e)
            {
                TempData["CreateFejl"] = "Kunde ikke fundet";
                System.Diagnostics.Debug.WriteLine(e);
            }
            
            if(kunde != null)
            {
                newBooking.KundeKundenummerID = kunde.KundenummerID;
                kunde.Booking.Add(newBooking);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }


        // GET: BookingSets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = _db.BookingSet.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: BookingSets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = _db.BookingSet.Find(id);
            _db.BookingSet.Remove(booking);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
