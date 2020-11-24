using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Obligatorisk_ASP.NET_MVC_.Models;

namespace Obligatorisk_ASP.NET_MVC_.Controllers
{
    public class VærktøjController : Controller
    {
        private VærktøjsudlejningEntities _db = new VærktøjsudlejningEntities();

        // GET: VærktøjSet
        public ActionResult Index()
        {
            string searchData = (string)TempData["SearchData"];

            if (searchData == null)
            {
                return RedirectToAction("Index", "Home");
            }
            string[] data = searchData.Split(';');
            DateTime fra = new DateTime();
            DateTime til = new DateTime();

            try
            {
                fra = DateTime.Parse(data[0]);
                til = DateTime.Parse(data[1]);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return View(_db.VærktøjSet.ToList());
            }

            IEnumerable<Booking> bookinger = _db.BookingSet;


            // Selekterer på værktøj som er afleveret og ligger indenfor det angivede tidsrum med datoerne
            List<Værktøj> værktøjSelected = (
                from b in bookinger
                where b.Status.Equals("afleveret")
                where fra <= b.Afhentningstidspunkt && til >= b.Afhentningstidspunkt
                from v in b.Værktøj
                select v).ToList();

            // Find alle værktøjer der ikke har en booking. Find al værktøj i bookingerne og fjerne det fra listen med al værktøj i
            List<Værktøj> allVærktøj = _db.VærktøjSet.ToList();
            foreach (var v in from b in bookinger
                              from v in b.Værktøj
                              where b.Status.Equals("udlejet") || b.Status.Equals("reserveret")
                              select v)
                    {
                        allVærktøj.Remove(v);
                    }
            
            værktøjSelected.AddRange(from v in allVærktøj
                                     select v);

            return View(værktøjSelected);
        }

        public string CreateVærktøjer()
        {
            Værktøj v1 = new Værktøj() { Navn = "Boremaskine", Beskrivelse = "Falke akku boremaskine 18V inkl. 2,0 Ah Li-ion batteri", Depositum = 100.0, DøgnPris = 40.0 };
            Værktøj v2 = new Værktøj() { Navn = "Borsæt", Beskrivelse = "Irwin HSS Pro Cobalt borsæt 15 stk.", Depositum = 20.0, DøgnPris = 5.0 };
            Værktøj v3 = new Værktøj() { Navn = "Stiksav", Beskrivelse = "Falke stiksav med pendul 750W", Depositum = 100.0, DøgnPris = 30.0 };
            Værktøj v4 = new Værktøj() { Navn = "Rystepudser", Beskrivelse = "Falke rystepudser 240W", Depositum = 75.0, DøgnPris = 20.0 };
            Værktøj v5 = new Værktøj() { Navn = "Rundsav", Beskrivelse = "Falke akku rundsav 165 mm", Depositum = 400.0, DøgnPris = 80.0 };
            Værktøj v6 = new Værktøj() { Navn = "Savklinge", Beskrivelse = "Savklinge 125 x 10 mm - 24T", Depositum = 30.0, DøgnPris = 10.0 };
            Værktøj v7 = new Værktøj() { Navn = "Varmepistol", Beskrivelse = "Falke varmepistol 2000W", Depositum = 150.0, DøgnPris = 30.0 };
            Værktøj v8 = new Værktøj() { Navn = "Betonblander", Beskrivelse = "Betonblander 125 liter", Depositum = 700.0, DøgnPris = 100.0 };
            Værktøj v9 = new Værktøj() { Navn = "Højtryksrenser", Beskrivelse = "Nilfisk højtryksrenser C 125.7-6 X-TRA", Depositum = 400.0, DøgnPris = 75.0 };
            Værktøj v10 = new Værktøj() { Navn = "Kompressor", Beskrivelse = "Kompressor 2,5 hk 24 liter oliefri", Depositum = 800.0, DøgnPris = 150.0 };

            _db.VærktøjSet.Add(v1);
            _db.VærktøjSet.Add(v2);
            _db.VærktøjSet.Add(v3);
            _db.VærktøjSet.Add(v4);
            _db.VærktøjSet.Add(v5);
            _db.VærktøjSet.Add(v6);
            _db.VærktøjSet.Add(v7);
            _db.VærktøjSet.Add(v8);
            _db.VærktøjSet.Add(v9);
            _db.VærktøjSet.Add(v10);

            _db.SaveChanges();

            return "Værktøj er gemt i databasen";
        }


        public ActionResult Reserver(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Vis reserverknap hvis værktøj har status "afleveret"
            IEnumerable<Booking> bookinger = _db.BookingSet;
            List<Værktøj> værktøjSelected = new List<Værktøj>();
            Værktøj værktøj = _db.VærktøjSet.Find(id);

            if (værktøj == null)
            {
                return HttpNotFound();
            }
            return View(værktøj);
        }

        // POST: Værktøj/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reserver(Værktøj værktøj)
        {
            return RedirectToAction("Create", "Booking", værktøj);
        }

    }
}
