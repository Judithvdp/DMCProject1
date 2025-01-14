﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DMCProject1.DAL;
using DMCProject1.Models;
using System.Dynamic;
using DMCProject1.Helpers;

namespace DMCProject1.Controllers
{
    public class UserColorController : Controller
    {
        private DmcContext db = new DmcContext();

        // GET: UserColors
        public ActionResult Index()
        {
            List<ColorCollection> colorCollections = new List<ColorCollection>();

            List<UserColor> userColors = new List<UserColor>();
            userColors = db.UserColors.ToList();
            foreach (UserColor item in userColors)
            {
                ColorCollection color = new ColorCollection();
                color.ColorId = item.ColorId;
                color.DmcId = item.DmcId;
                color.UserId = item.UserId;
                color.Amount = item.Amount;

                DmcColor dmcColor = db.DmcColors.Where(d => d.DmcId == item.DmcId).FirstOrDefault();
                if (dmcColor != null)
                {
                    color.HexaDecimal = dmcColor.HexaDecimal;
                }

                colorCollections.Add(color);
            }
            
            return View(colorCollections);
        }

        // GET: UserColors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserColor userColor = db.UserColors.Find(id);
            if (userColor == null)
            {
                return HttpNotFound();
            }
            ColorCollection color = new ColorCollection();
            color.ColorId = userColor.ColorId;
            color.DmcId = userColor.DmcId;
            color.Amount = userColor.Amount;
            color.UserId = userColor.UserId;

            DmcColor dmcColor = db.DmcColors.Where(e => e.DmcId == userColor.DmcId).FirstOrDefault();
            if (dmcColor != null)
            {
                color.HexaDecimal = dmcColor.HexaDecimal;
            }
            return View(color);
        }

        // GET: UserColors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserColors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ColorId,DmcID,UserID,Amount")] UserColor userColor)
        {
            if (ModelState.IsValid)
            {
                //TODO: UserId is set to 1
                userColor.UserId = 1;
                DmcColor dmcColor = db.DmcColors.Where(e => e.DmcId == userColor.DmcId).FirstOrDefault();
                if (dmcColor == null)
                {
                    //TODO: make proper error page 
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else db.UserColors.Add(userColor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userColor);
        }

        // GET: UserColors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserColor userColor = db.UserColors.Find(id);
            if (userColor == null)
            {
                return HttpNotFound();
            }
            return View(userColor);
        }

        // POST: UserColors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ColorId,DmcID,UserID,Amount")] UserColor userColor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userColor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userColor);
        }

        // GET: UserColors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserColor userColor = db.UserColors.Find(id);
            if (userColor == null)
            {
                return HttpNotFound();
            }
            return View(userColor);
        }

        // POST: UserColors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserColor userColor = db.UserColors.Find(id);
            db.UserColors.Remove(userColor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
