using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AssessmentSoftware.Models;
using AssessmentSoftware.ViewModel;
using Microsoft.AspNet.Identity;

namespace AssessmentSoftware.Controllers
{
    /// <summary>
    /// Manipulates the users and their roles
    /// Holds functions that allows admins to promote and suspend users
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class ManageUsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ManageUsers
        public ActionResult Index()
        {
            List<UserViewModel> theUsers = new List<UserViewModel>();
            foreach (ApplicationUser aUser in db.Users)
            {
                UserViewModel uViewModel = new UserViewModel();
                uViewModel.Id = aUser.Id;
                uViewModel.UserName = aUser.UserName;
                uViewModel.Email = aUser.Email;
                uViewModel.IsAdmin = aUser.IsAdmin;
                //change
                uViewModel.IsSuspended = aUser.IsSuspended;
                theUsers.Add(uViewModel);
            }
            return View(theUsers);
        }

        // GET: ManageUsers/Details/5
        public ActionResult Details(string id)
        {
            ApplicationUser aUser = db.Users.Single(m => m.Id == id);
            UserViewModel uvm = new UserViewModel();
            uvm.UserName = aUser.UserName;
            uvm.Email = aUser.Email;
            uvm.IsAdmin = aUser.IsAdmin;
            uvm.IsSuspended = aUser.IsSuspended;
            return View(uvm);
        }

        // GET: ManageUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ManageUsers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ManageUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserViewModel uvm = new UserViewModel();
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            else
            {
                uvm.UserName = applicationUser.UserName;
                uvm.Email = applicationUser.Email;
                uvm.IsAdmin = applicationUser.IsAdmin;
                uvm.IsSuspended = applicationUser.IsSuspended;
            }
            return View(uvm);
        }

        // POST: ManageUsers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserViewModel userViewModel)
        {
            //initaliase a local usermanager so that we can check user roles
            var UserManager = new UserManager<ApplicationUser>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<ApplicationUser>(db));
            //get user from db
            ApplicationUser au = db.Users.Find(userViewModel.Id);
            //set values of user to the changed values from the edit form
            au.Id = userViewModel.Id;
            au.UserName = userViewModel.UserName;
            au.Email = userViewModel.Email;
            au.IsAdmin = userViewModel.IsAdmin;
            au.IsSuspended = userViewModel.IsSuspended;
            au.FirstName = userViewModel.FirstName;
            au.LastName = userViewModel.LastName;

            if (ModelState.IsValid)
            {

                //setting users to admin role
                if ((au.IsAdmin) && (!UserManager.IsInRole(au.Id, "Admin")))
                    UserManager.AddToRole(au.Id, "Admin");
                //unsetting user from admin role
                else if ((!au.IsAdmin) && (UserManager.IsInRole(au.Id, "Admin")))
                    UserManager.RemoveFromRoles(au.Id, "Admin");
                //setting user to restricted role
                if ((au.IsSuspended) && (!UserManager.IsInRole(au.Id, "Restricted")))
                    UserManager.AddToRole(au.Id, "Restricted");
                //unsetting user from restricted role
                else if ((!au.IsSuspended) && (UserManager.IsInRole(au.Id, "Restricted")))
                    UserManager.RemoveFromRoles(au.Id, "Restricted");

                db.Entry(au).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(userViewModel);
        }

        public ActionResult Delete(string id)
        {
            UserViewModel uvm = new UserViewModel();
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            else
            {
                uvm.Id = applicationUser.Id;
                uvm.UserName = applicationUser.UserName;
                uvm.Email = applicationUser.Email;
                uvm.IsAdmin = applicationUser.IsAdmin;
                uvm.IsSuspended = applicationUser.IsSuspended;

            }
            return View(uvm);
        }

        // POST: ManageUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);
            db.Users.Remove(applicationUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
