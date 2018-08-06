using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ErrandApp.Models;
using ErrandApp.Util;
using DevOne.Security.Cryptography.BCrypt;
using System.IO;
using ErrandApp.UI.DB.PRODUCTION;

namespace ErrandApp.API.Controllers
{
    public class GateWaySvc : Controller
    {
        private readonly ErrandAppEntities db = new ErrandAppEntities();

        #region ERRAND APPPLICATION API CALLS
  

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        // GET: API
        public JsonResult CreateErrandee(CreateErrandee errandee)
        {
            try
            {
                var checkErrandee = (from x in db.Registerations
                                     where x.Email == errandee.Email
                                     select x).Count();

                if (checkErrandee > 0)
                {
                    return Json(new { ResponseCode = "03", Message = "Errandee Already Exist" }, JsonRequestBehavior.AllowGet);
                }


                bool isEmailValid = Validator.isValidEmail(errandee.Email);

                if (isEmailValid == false)
                {
                    return Json(new { ResponseCode = "02", Message = "Email Invalid" });
                }

                string salt = BCryptHelper.GenerateSalt(6);

                var hashedPassword = BCryptHelper.HashPassword(errandee.Password, salt);

                Registeration reg = new Registeration()
                {
                    UserName = errandee.UserName,

                    Email = errandee.Email,

                    Password = hashedPassword,
                    FullName = errandee.FullName,

                    Location = errandee.Location,

                    PhoneNo = errandee.PhoneNo,
                    Gender = errandee.Gender
                };

                db.Registerations.Add(reg);

                db.SaveChanges();

                //Create directories for new users
                DirectoryInfo AccountDirectoryForUsers = Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\" + "AccountDirectory" + "\\" + errandee.Email);

                AccountDirectoryForUsers.CreateSubdirectory("ProfileImage");

                AccountDirectoryForUsers.CreateSubdirectory("Documents");

                return Json(new { ResponseCode = "00", Message = "Errandee Created Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //throw;
                return Json(new { ResponseCode = "05", Message = "Invalid HTTP POST Parameters" }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult UpdateErrandee(CreateErrandee errandee)
        {
            try
            {
                var checkErrande = (from x in db.Registerations
                                    where x.Email == errandee.Email
                                    select x).Count();

                if (checkErrande.Equals(0))
                {
                    return Json(new { ResponseCode = "03", Message = "Errandee Not Found" }, JsonRequestBehavior.AllowGet);
                }

                var id = db.Registerations.Where(x => x.Email == errandee.Email).Select(x => x.RegisterationId).FirstOrDefault();

                var details = db.Registerations.Find(id);

                details.FullName = errandee.FullName;

                details.PhoneNo = errandee.PhoneNo;

                details.Location = errandee.Location;

                details.UserName = errandee.UserName;

                details.Email = errandee.Email;

                string salt = BCryptHelper.GenerateSalt(6);

                var hashedPassword = BCryptHelper.HashPassword(errandee.Password, salt);

                details.Password = hashedPassword;

                db.Entry(details).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();

                return Json(new { ResponseCode = "00", Message = "Update Complete" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                // throw;

                return Json(new { ResponseCode = "05", Message = "Invalid HTTP POST Parameters" }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult DeleteErrandee(string Email)
        {
            try
            {
                var checkErrande = (from x in db.Registerations
                                    where x.Email == Email
                                    select x).Count();
                if (checkErrande > 0)
                {
                    return Json(new { ResponseCode = "03", Message = "Errandee Not Found" }, JsonRequestBehavior.AllowGet);
                }

                var id = db.Registerations.Where(x => x.Email == Email).Select(x => x.RegisterationId).FirstOrDefault();

                Registeration reg = db.Registerations.Find(Convert.ToInt32(id));

                db.Registerations.Remove(reg);

                db.Entry(reg).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();

                return Json(new { ResponseCode = "00", Message = "Errandee Deleted" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //throw;
                return Json(new { ResponseCode = "05", Message = "Invalid HTTP GET Parameter" }, JsonRequestBehavior.AllowGet);
            }
        }
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult ErandeeLoginAuthentication(Login login)
        {
            try
            {
                bool isEmail = Validator.isValidEmail(login.Identity);

                if (isEmail == true)
                {
                    var checkMailExist = (from x in db.Registerations
                                          where x.Email == login.Identity
                                          select x).Count();

                    if (checkMailExist.Equals(0))
                    {
                        return Json(new { ResponseCode = "03", Message = "Email Does Not Exist", data = string.Empty }, JsonRequestBehavior.AllowGet);
                    }

                    var details = db.Registerations.Where(x => x.Email == login.Identity).Select(x => x).FirstOrDefault();

                    bool isPasswordValid = BCryptHelper.CheckPassword(login.Password, details.Password.Trim());

                    switch (isPasswordValid)
                    {
                        case true:

                            var retVal = (from x in db.Registerations
                                          where x.Email == login.Identity
                                          select new
                                          {
                                              FullName = x.FullName.Trim(),
                                              UserName = x.UserName.Trim(),
                                              Location = x.Location.Trim(),
                                              Email = x.Email.Trim(),
                                              ErrandeeID = x.RegisterationId,
                                              Gender = x.Gender.Trim()
                                          }).FirstOrDefault();


                            return Json(new { ResponseCode = "00", Message = "Password Authentication Valid", data = retVal }, JsonRequestBehavior.AllowGet);

                        case false:
                            return Json(new { ResponseCode = "02", Message = "Password Authentication Invalid", data = string.Empty }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var checkPhoneExist = (from x in db.Registerations
                                           where x.PhoneNo == login.Identity
                                           select x).Count();

                    if (checkPhoneExist.Equals(0))
                    {
                        return Json(new { ResponseCode = "03", Message = "PhoneNo Does Not Exist", data = string.Empty }, JsonRequestBehavior.AllowGet);
                    }

                    var details = db.Registerations.Where(x => x.PhoneNo == login.Identity).Select(x => x).FirstOrDefault();

                    bool isPasswordValid = BCryptHelper.CheckPassword(login.Password, details.Password.Trim());

                    switch (isPasswordValid)
                    {
                        case true:
                            return Json(new { ResponseCode = "00", Message = "Password Authentication Valid", data = string.Empty }, JsonRequestBehavior.AllowGet);

                        case false:
                            return Json(new { ResponseCode = "02", Message = "Password Authentication Invalid", data = string.Empty }, JsonRequestBehavior.AllowGet);
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                //throw;
                return Json(new { ResponseCode = "05", Message = "Invalid HTTP POST Parameters:"+ex.Message, data = string.Empty }, JsonRequestBehavior.AllowGet);
            }
        }
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetErrandeeDetails(string Identity)
        {
            try
            {
                var details = (from x in db.Registerations
                               where x.Email == Identity
                               select new
                               {
                                   ResponseCode = "00",
                                   Message = "Request Successful",
                                   FullName = x.FullName.Trim(),
                                   UserName = x.UserName.Trim(),
                                   Location = x.Location.Trim(),
                                   Email = x.Email.Trim()
                               }).FirstOrDefault();

                return Json(details, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult UpdatePassword(string Identity, string NewPassword)
        {
            try
            {
                bool isMailValid = Validator.isValidEmail(Identity);

                if (isMailValid == false)
                {
                    return Json(new { ResponseCode = "02", Message = "Email is Not Valid" }, JsonRequestBehavior.AllowGet);
                }

                var id = db.Registerations.Where(x => x.Email == Identity).Select(x => x.RegisterationId).FirstOrDefault();

                var details = db.Registerations.Find(id);

                string salt = BCryptHelper.GenerateSalt(6);

                var hashedPassword = BCryptHelper.HashPassword(NewPassword, salt);

                details.Password = hashedPassword;

                db.Entry(details).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();

                return Json(new { ResponseCode = "00", Message = "Password Successfully Updated" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { ResponseCode = "05", Message = "Invalid HTTP POST Parameters" }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult UploadProfileImage(HttpPostedFileBase file, string Identity)
        {
            try
            {

                foreach (string fileName in Request.Files)

                {
                    file = Request.Files[fileName];

                    var id = db.Registerations.Where(x => x.Email == Identity).Select(x => x.RegisterationId).FirstOrDefault();

                    //Check if file exist
                    var isFileExisting = (from x in db.ErrandeeProfileImages
                                          where x.ErrandeeID == id.ToString()
                                          select x).Count();

                    if (isFileExisting > 0)
                    {
                        //If already exist, overide
                        string path = Path.Combine(Server.MapPath("~/AccountDirectory/" + Identity + "/ProfileImage"), Path.GetFileName(file.FileName));

                        file.SaveAs(path);


                        var profileimageid = (from x in db.ErrandeeProfileImages
                                              where x.ErrandeeID == id.ToString()
                                              select x.ProfileImageId).FirstOrDefault();

                        var profile = db.ErrandeeProfileImages.Find(profileimageid);

                        profile.ProfileImageName = file.FileName;

                        db.Entry(profile).State = System.Data.Entity.EntityState.Modified;

                        db.SaveChanges();

                        return Json(new { ResponseCode = "00", Message = "Profile Image Saved" }, JsonRequestBehavior.AllowGet);
                    }

                    else if (isFileExisting.Equals(0))
                    {
                        ErrandeeProfileImage profileimg = new ErrandeeProfileImage()
                        {
                            ErrandeeID = id.ToString(),

                            ProfileImageName = file.FileName,

                            Date = DateTime.UtcNow.ToString("yyyy-MM-dd-HH:mm:ss")
                        };

                        //Save logo file to directory
                        string path = Path.Combine(Server.MapPath("~/AccountDirectory/" + Identity + "/ProfileImage"), Path.GetFileName(file.FileName));

                        file.SaveAs(path);

                        return Json(new { ResponseCode = "00", Message = "Profile Image Saved" }, JsonRequestBehavior.AllowGet);
                    }

                }

                return null;
            }
            catch (Exception)
            {
                //throw;

                return Json(new { ResponseCode = "05", Message = "Invalid HTTP POST" }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult CreateErrander(CreateErrander errander)
        {
            try
            {
                var isEmailValid = Validator.isValidEmail(errander.Email);

                if (isEmailValid == false)
                {
                    return Json(new { ResponseCode = "02", Message = "Invalid Email" }, JsonRequestBehavior.AllowGet);
                }

                var checkErrander = (from x in db.ErranderRegisterations
                                     where x.Email == errander.Email
                                     select x).Count();
                if (checkErrander > 0)
                {
                    return Json(new { ResponseCode = "03", Message = "Errander Already Exist" }, JsonRequestBehavior.AllowGet);
                }

                string salt = BCryptHelper.GenerateSalt(6);

                var hashedPassword = BCryptHelper.HashPassword(errander.Password, salt);

                ErranderRegisteration reg = new ErranderRegisteration()
                {
                    FullName = errander.FullName,

                    Email = errander.Email,

                    Password = hashedPassword,

                    Gender = errander.Gender,

                    PhoneNo = errander.PhoneNo,

                    Address = errander.Address,

                    AddedOn = DateTime.UtcNow.ToString("yyyy-MM-dd-HH:mm:ss"),
                };

                db.ErranderRegisterations.Add(reg);

                db.SaveChanges();

                return Json(new { ResponseCode = "00", Message = "Errander Created Successfully" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                //throw;
                return Json(new { ResponseCode = "05", Message = "Invalid HTTP POST Parameters" }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult ErranderLoginAuthentication(Login login)
        {
            try
            {

                var retVal = (from x in db.ErranderRegisterations
                              where x.Email == login.Identity
                              select new
                              {
                                  FullName = x.FullName.Trim(),
                                  Location = x.Address.Trim(),
                                  ErranderID = x.ErranderId,
                                  Email = x.Email.Trim(),
                                  Gender = x.Gender.Trim(),
                              }).FirstOrDefault();

                bool isEmail = Validator.isValidEmail(login.Identity);

                if (isEmail == true)
                {
                    var checkMailExist = (from x in db.ErranderRegisterations
                                          where x.Email == login.Identity
                                          select x).Count();

                    if (checkMailExist.Equals(0))
                    {
                        return Json(new { ResponseCode = "03", Message = "Email Does Not Exist" }, JsonRequestBehavior.AllowGet);
                    }

                    var details = db.ErranderRegisterations.Where(x => x.Email == login.Identity).Select(x => x).FirstOrDefault();

                    bool isPasswordValid = BCryptHelper.CheckPassword(login.Password, details.Password.Trim());

                    switch (isPasswordValid)
                    {
                        case true:

                            return Json(new { ResponseCode = "00", Message = "Password Authentication Valid", data = retVal }, JsonRequestBehavior.AllowGet);

                        case false:
                            return Json(new { ResponseCode = "02", Message = "Password Authentication Invalid", data = string.Empty }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var checkPhoneExist = (from x in db.ErranderRegisterations
                                           where x.PhoneNo == login.Identity
                                           select x).Count();

                    if (checkPhoneExist.Equals(0))
                    {
                        return Json(new { ResponseCode = "03", Message = "PhoneNo Does Not Exist", data = string.Empty }, JsonRequestBehavior.AllowGet);
                    }

                    var details = db.ErranderRegisterations.Where(x => x.PhoneNo == login.Identity).Select(x => x).FirstOrDefault();

                    bool isPasswordValid = BCryptHelper.CheckPassword(login.Password, details.Password.Trim());

                    switch (isPasswordValid)
                    {
                        case true:
                            return Json(new { ResponseCode = "00", Message = "Password Authentication Valid", data = retVal }, JsonRequestBehavior.AllowGet);

                        case false:
                            return Json(new { ResponseCode = "02", Message = "Password Authentication Invalid", data = string.Empty }, JsonRequestBehavior.AllowGet);
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                //throw;
                return Json(new { ResponseCode = "05", Message = "Invalid HTTP POST Parameter:: "+ex.Message, data = string.Empty }, JsonRequestBehavior.AllowGet);

            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult UpdateErranderPassword(string Identity, string NewPassword)
        {
            try
            {
                bool isMailValid = Validator.isValidEmail(Identity);

                if (isMailValid == false)
                {
                    return Json(new { ResponseCode = "02", Message = "Email is Not Valid" }, JsonRequestBehavior.AllowGet);
                }

                var id = db.ErranderRegisterations.Where(x => x.Email == Identity).Select(x => x.ErranderId).FirstOrDefault();

                var details = db.ErranderRegisterations.Find(id);

                string salt = BCryptHelper.GenerateSalt(6);

                var hashedPassword = BCryptHelper.HashPassword(NewPassword, salt);

                details.Password = hashedPassword;

                db.Entry(details).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();

                return Json(new { ResponseCode = "00", Message = "Password Successfully Updated" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //throw;
                return Json(new { ResponseCode = "05", Message = "Invalid HTTP POST Parameters" }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetErranderDetails(string Identity)
        {
            try
            {
                var details = (from x in db.ErranderRegisterations
                               where x.Email == Identity
                               select new
                               {
                                   ResponseCode = "00",
                                   Message = "Request Successful",
                                   FullName = x.FullName.Trim(),
                                   Location = x.Address.Trim(),
                                   Email = x.Email.Trim(),
                                   Gender = x.Gender.Trim(),
                               }).FirstOrDefault();

                return Json(details, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                //throw;
                return Json(new { ResponseCode = "05", Message = "Invalid HTTP GET Parameter:: "+ex.Message, FullName = string.Empty, Location = string.Empty, Email = string.Empty, Gender = string.Empty }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult CreateErrandType(string ErrandName, string Category, string Cost)
        {
            try
            {
                ErrandType type = new ErrandType()
                {
                    ErrandName = ErrandName,

                    Category = Category,

                    Cost = Cost,

                    AddedOn = DateTime.UtcNow.ToString("yyyy-MM-dd-HH:mm:ss"),
                };

                db.ErrandTypes.Add(type);

                db.SaveChanges();

                return Json(new { ResponseCode = "00", Message = "Errand Type Created" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                return Json(new { ResponseCode = "05", Message = "Invalid HTTP POST Parameter:: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult DeleteErrandType(string ErrandID)
        {
            try
            {
                ErrandType type = db.ErrandTypes.Find(Convert.ToInt32(ErrandID));

                db.ErrandTypes.Remove(type);

                db.Entry(type).State = System.Data.Entity.EntityState.Deleted;

                db.SaveChanges();

                return Json(new { ResponseCode = "00", Message = "Errand Type Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //throw;
                return Json(new { ResponseCode = "05", Message = "Invalid HTTP GET Parameter" }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetAllErrandTypes()
        {
            try
            {
                var types = db.ErrandTypes.Select(x => new
                {
                    ErrandName = x.ErrandName,
                    Category = x.Category,
                    Cost = x.Cost,
                    ErrandID = x.ErrandTypeId,
                    DateCreated = x.AddedOn
                }).ToList();

                return Json(types, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult UpdateErrandType(string ErrandID, string ErrandName, string Cost, string Category)
        {
            try
            {
                var checkErrand = db.ErrandTypes.Where(x => x.ErrandTypeId.ToString() == ErrandID).Count();

                if (checkErrand.Equals(0))
                {
                    return Json(new { ResponseCode = "03", Message = "Errand Type Not Found" }, JsonRequestBehavior.AllowGet);
                }


                ErrandType type = db.ErrandTypes.Find(Convert.ToInt32(ErrandID));

                type.ErrandName = ErrandName;

                type.Category = Category;

                type.Cost = Cost;

                db.Entry(type).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();


                return Json(new { ResponseCode = "00", Message = "Errand Type Update Successfully" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                //throw;
                return Json(new { ResponseCode = "05", Message = "Invalid HTTP GET Parameter" }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult StartErrand(string ErrandID, string ErranderID, string ErrandeeID, string PickUpLocation, string DropOfLocation)
        {
            try
            {

                Request req = new Request()
                {
                    ErrandeeID = ErrandeeID,

                    ErranderID = ErranderID,

                    PickUpLocation = PickUpLocation,

                    DropOffLocation = DropOfLocation,

                    ErrandID = ErrandID,

                    Status = "Start",

                    Date = DateTime.UtcNow.ToString("yyyy-MM-dd-HH:mm:ss"),
                    IsComplete = false
                };

                db.Requests.Add(req);

                db.SaveChanges();

                ErrandeeAvailability availability = new ErrandeeAvailability()
                {
                    ErrandeeID = ErrandeeID.ToString(),

                    isAvailable = false,
                    
                };

                db.ErrandeeAvailabilities.Add(availability);

                db.SaveChanges();

                return Json(new { ResponseCode = "00", Message = "Errand Activated" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //throw;
                return Json(new { ResponseCode = "05", Message = "Invalid HTTP GET POST Parameter" }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult StopErrand(string ErrandID, string ErrandeeID)
        {
            try
            {

                var ReqID = db.Requests.Where(x => x.ErrandeeID == ErrandeeID && x.Status.Equals("Start")).Select(x => x.RequestId).FirstOrDefault();

                Request req = db.Requests.Find(Convert.ToInt32(ReqID));

                req.IsComplete = true;

                req.Status = "Stopped";

                db.Requests.Add(req);
                db.Entry(req).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();

                var availabilityid = db.ErrandeeAvailabilities.Where(x => x.ErrandeeID == ErrandeeID).Select(x => x.AvailabilityID).FirstOrDefault();

                ErrandeeAvailability availability = db.ErrandeeAvailabilities.Find(availabilityid);

                availability.isAvailable = true;

                db.Entry(availability).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();

                return Json(new { ResponseCode = "00", Message = "Errand Deactivated Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                // throw;
                return Json(new { ResponseCode = "05", Message = "Invalid HTTP POST Parameter" }, JsonRequestBehavior.AllowGet);

            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult ErrandeeAvailability()
        {
            try
            {
                var availability = (from x in db.ErrandeeAvailabilities
                                    join y in db.Registerations on x.ErrandeeID equals y.RegisterationId.ToString()
                                    where x.isAvailable == true
                                    select new
                                    {
                                        ErrandeeID = x.ErrandeeID,
                                        isAvailable = x.isAvailable,
                                        Location = y.Location.Trim(),
                                        FullName = y.FullName.Trim(),
                                        UserName = y.UserName.Trim(),
                                        PhoneNo = y.PhoneNo.Trim(),
                                        Email = y.Email.Trim(),
                                    }).ToList();

                return Json(availability, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult SendErrandeeLocation(string ErrandeeEmail, string Lat, string Long)
        {
            try
            {
                var checkErrandeeExistence = db.ErrandeeLocations.Where(x => x.ErrandeeEmail == ErrandeeEmail).Count();

                if (checkErrandeeExistence > 0)
                {
                    //Update previous location

                    var locationID = (from x in db.ErrandeeLocations
                                      where x.ErrandeeEmail == ErrandeeEmail
                                      select x.LocationId).FirstOrDefault();

                    ErrandeeLocation errandeelocation = db.ErrandeeLocations.Find(locationID);

                    errandeelocation.Lat = Lat;

                    errandeelocation.Long = Long;

                    db.Entry(errandeelocation).State = System.Data.Entity.EntityState.Modified;

                    db.SaveChanges();

                    return Json(new { ResponseCode = "00", Message = "Errandee Location Saved" }, JsonRequestBehavior.AllowGet);
                }
                //Insert new location

                ErrandeeLocation location = new ErrandeeLocation()
                {
                    ErrandeeEmail = ErrandeeEmail,

                    Lat = Lat,

                    Long = Long,
                };

                db.ErrandeeLocations.Add(location);

                db.Entry(location).State = System.Data.Entity.EntityState.Added;

                db.SaveChanges();

                return Json(new { ResponseCode = "00", Message = "Errandee Location Saved" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                return Json(new { ResponseCode = "05", Message = "Invalid HTTP POST Parameter:: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult UpdateErrandeeLocation(string ErrandeeEmail, string Lat, string Long)
        {
            try
            {
                var checkErrandeeExistence = db.ErrandeeLocations.Where(x => x.ErrandeeEmail == ErrandeeEmail).Count();

                if (checkErrandeeExistence > 0)
                {
                    var locationID = (from x in db.ErrandeeLocations
                                      where x.ErrandeeEmail == ErrandeeEmail
                                      select x.LocationId).FirstOrDefault();

                    ErrandeeLocation errandeelocation = db.ErrandeeLocations.Find(locationID);

                    errandeelocation.Lat = Lat;

                    errandeelocation.Long = Long;

                    db.Entry(errandeelocation).State = System.Data.Entity.EntityState.Modified;

                    db.SaveChanges();
                }
                return Json(new { ResponseCode = "00", Message = "Errandee Location Updated" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                return Json(new { ResponseCode = "05", Message = "Invalid HTTP POST Parameter:: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetAllErrandeeLocations()
        {
            try
            {
                var errandeeLocations = (from x in db.ErrandeeLocations
                                         join y in db.Registerations on x.ErrandeeEmail equals y.Email
                                         select new
                                         {
                                             FullName = y.FullName.Trim(),
                                             Latitude = x.Lat.Trim(),
                                             Longitude = x.Long.Trim()
                                         }).ToList();

                return Json(new { errandeeLocations }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult SendErranderLocation(string ErranderEmail, string Lat, string Long)
        {
            try
            {
                var checkErranderExistence = db.ErranderLocations.Where(x => x.ErranderEmail == ErranderEmail).Count();

                if (checkErranderExistence > 0)
                {
                    //Update previous location

                    var locationID = (from x in db.ErranderLocations
                                      where x.ErranderEmail == ErranderEmail
                                      select x.LocationId).FirstOrDefault();

                    ErranderLocation erranderlocation = db.ErranderLocations.Find(locationID);

                    erranderlocation.Lat = Lat;

                    erranderlocation.Long = Long;

                    db.Entry(erranderlocation).State = System.Data.Entity.EntityState.Modified;

                    db.SaveChanges();

                    return Json(new { ResponseCode = "00", Message = "Errandee Location Saved" }, JsonRequestBehavior.AllowGet);
                }
                //Insert new location

                ErranderLocation location = new ErranderLocation()
                {
                    ErranderEmail = ErranderEmail,

                    Lat = Lat,

                    Long = Long,
                };

                db.ErranderLocations.Add(location);

                db.Entry(location).State = System.Data.Entity.EntityState.Added;

                db.SaveChanges();

                return Json(new { ResponseCode = "00", Message = "Errandee Location Saved" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                return Json(new { ResponseCode = "05", Message = "Invalid HTTP POST Parameter:: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult UpdateErranderLocation(string ErranderEmail, string Lat, string Long)
        {
            try
            {
                var checkErranderExistence = db.ErranderLocations.Where(x => x.ErranderEmail == ErranderEmail).Count();

                if (checkErranderExistence > 0)
                {
                    var locationID = (from x in db.ErranderLocations
                                      where x.ErranderEmail == ErranderEmail
                                      select x.LocationId).FirstOrDefault();

                    ErranderLocation erranderlocation = db.ErranderLocations.Find(locationID);

                    erranderlocation.Lat = Lat;

                    erranderlocation.Long = Long;

                    db.Entry(erranderlocation).State = System.Data.Entity.EntityState.Modified;

                    db.SaveChanges();
                }
                return Json(new { ResponseCode = "00", Message = "Errandee Location Updated" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //throw;
                return Json(new { ResponseCode = "05", Message = "Invalid HTTP POST Parameter:: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetAllErranderLocations()
        {
            try
            {
                var erranderLocations = (from x in db.ErranderLocations
                                         join y in db.ErranderRegisterations on x.ErranderEmail equals y.Email
                                         select new
                                         {
                                             FullName = y.FullName.Trim(),
                                             Latitude = x.Lat.Trim(),
                                             Longitude = x.Long.Trim()
                                         }).ToList();

                return Json(new { erranderLocations }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }

        //Start Testing From Here
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult CreateErrandeePaymentDetails(string AccountName, long AccountNo, string BankName, string ErrandeeID)
        {
            try
            {

                AccountDetail details = new AccountDetail()
                {
                    AccountName = AccountName,
                    AccountNo = AccountNo.ToString(),
                    BankName = BankName,
                    ErrandeeID = ErrandeeID
                };
                db.AccountDetails.Add(details);
                db.Entry(details).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();

                return Json(new { ResponseCode = "00", Message = "Details Added" }, JsonRequestBehavior.AllowGet);

            } catch (Exception ex)
            {
                return Json(new { ResponseCode = "02", Message = "Invalid HTTP Post Parameter:: " + ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetErrandeePaymentDetails(string ErrandeeID)
        {
            try
            {
                var retVal = db.AccountDetails.Where(x => x.ErrandeeID == ErrandeeID).Select(x => new {
                    AccountName = x.AccountName.Trim(),
                    AccountNo = x.AccountNo.Trim(),
                    BankName = x.BankName.Trim()
                }).ToList();

                return Json(new { ResponseCode = "00", Message = "Request Seccessful", data = retVal }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ResponseCode = "02", Message = "Invalid HTTP Post Parameter:: " + ex.Message, data = string.Empty }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult UpdateErrandeePaymentDetails(string AccountName, long AccountNo, string BankName, string ErrandeeID)
        {
            try
            {
                AccountDetail details = db.AccountDetails.Where(x => x.ErrandeeID == ErrandeeID).Select(x => x).FirstOrDefault();

                details.AccountName = AccountName;
                details.AccountNo = AccountNo.ToString();
                details.BankName = BankName;

                db.Entry(details).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return Json(new { ResponseCode = "00", Message = "Request Successful" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Response = "02", Message = "Invalid HTTP Post Parameter:: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult PostErrandeeEarnings(decimal Amount, string ErrandeeID)
        {
            try
            {
                Earning earn = new Earning()
                {
                    Amount = Amount,
                    ErrandeeID = ErrandeeID,
                    DateEarned = DateTime.Parse(DateTime.Now.ToString()),
                    IsLiquidated = false,
                };
                db.Earnings.Add(earn);
                db.Entry(earn).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();

                return Json(new { ResponseCode = "00", Message = "Request Successful" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ResponseCode = "02", Message = "Invalid HTTP Post Parameter:: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetErrandeeEarnings(string ErrandeeID)
        {
            try
            {
                var totalAmount = db.Earnings.Where(x => x.ErrandeeID == ErrandeeID && !x.IsLiquidated == true).Select(x => x).ToList();

                var earningBreakDown = db.Earnings.Where(x => x.ErrandeeID == ErrandeeID && !x.IsLiquidated == true).Select(x => new
                {
                    DateEarned = x.DateEarned,
                    Amount = x.Amount,
                    IsLiquidated = x.IsLiquidated
                }).OrderByDescending(x=>x.DateEarned).ToList();

                return Json(new { ResponseCode = "00", Message = "Request Successful", TotalEarning = totalAmount.Sum(y => y.Amount), data = earningBreakDown }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ResponseCode = "02", Message = "Invalid HTTP Parameter:: " + ex.Message, TotalEarning = string.Empty , data = string.Empty }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult RateErrandee(string ErrandeeID, string Review, int Rating, string ErranderID)
        {
            try
            {
                ErrandeeRating rating = new ErrandeeRating()
                {
                    ErrandeeID = ErrandeeID,
                    ErranderID = ErranderID,
                    RateDate = DateTime.Parse(DateTime.Now.ToString()),
                    Review = Review,
                    Rating = Rating
                };
                db.ErrandeeRatings.Add(rating);
                db.Entry(rating).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();

                return Json(new {ResponseCode = "00", Message = "Request Successful" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { ResponseCode = "02", Message = "Invalid HTTO Post Parameter:: "+ ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetErrandeeRating(string ErrandeeID)
        {
            try
            {
                var rating = (from x in db.ErrandeeRatings
                              where x.ErrandeeID == ErrandeeID
                              select new
                              {
                                  Review = x.Review.Trim(),
                                  Rate = x.Rating,
                                  Date = x.RateDate
                              }).ToList();

                return Json(new {ResponseCode = "00", Message = "Request Successful", StarRating = Math.Round(rating.Sum(x => x.Rate) / Convert.ToDouble(rating.Count)), data = rating }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new {ResponseCode = "02", Message = "Request Failed:: "+ex.Message, StartRating = string.Empty, data = string.Empty }, JsonRequestBehavior.AllowGet);
            }

        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult SetErranderPaymentMethod(string ErranderID, string PayMethod)
        {
            try
            {
                PaymentMethod payment = new PaymentMethod()
                {
                     ErranderID = ErranderID,
                     IsCard = PayMethod == "Card" ? true:false,
                     IsCash = PayMethod == "Cash" ? true:false
                };
                db.PaymentMethods.Add(payment);
                db.Entry(payment).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();

                return Json(new {ResponseCode = "00", Message = "Request Successful" }, JsonRequestBehavior.AllowGet);

            }
            catch(Exception ex)
            {
                return Json(new {ResponseCode = "02", Message = "Invalid HTTP Post Parameter:: "+ex.Message,  }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetErranderPaymentMethod(string ErranderID)
        {
            try
            {
                var retVal = db.PaymentMethods.Where(x => x.ErranderID == ErranderID).Select(x => x).FirstOrDefault();

                return Json(new {ResponseCode = "00", Message = "Request Successful", data = retVal }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new {ResponseCode = "02", Message = "Invalid HTTP Post Parameter::"+ex.Message, data = string.Empty }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult UpdateErranderPaymentMethod(string ErranderID, string PayMethod)
        {
            try
            {
                PaymentMethod method = db.PaymentMethods.Where(x => x.ErranderID == ErranderID).Select(x => x).FirstOrDefault();

                method.IsCard = PayMethod == "Card" ? true : false;
                method.IsCash = PayMethod == "Cash" ? true : false;

                db.Entry(method).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                return Json(new {ResponseCode = "00", Message = "Request Successful" }, JsonRequestBehavior.AllowGet);

            }
            catch(Exception ex)
            {
                return Json(new {ResponseCode = "02", Message = "Invalid HTTP Post Parameter:: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult SetErranderCardDetails(string ErranderID, string CardNo, string Cvv, string Exp)
        {
            try
            {
                if (CardNo.Length < 16)
                    return Json(new {ResponseCode = "03", Message = "Invalid PAN Lenght::" +CardNo.Length }, JsonRequestBehavior.AllowGet);

                CardDetail card = new CardDetail()
                {
                    ErranderID = ErranderID,
                    MaskedCardNo = new string (CardNo.Take(5).ToArray()) + "******",
                    CardType = CardNo.StartsWith("4") == true ? "VC" : "MC",
                    CVV = Cvv,
                    Exp = Exp,
                    CardNo = CardNo
                };
                db.CardDetails.Add(card);
                db.Entry(card).State = System.Data.Entity.EntityState.Added;                
                db.SaveChanges();

                return Json(new {ResponseCode = "00", Message = "Request Successful" }, JsonRequestBehavior.AllowGet);

            }catch(Exception ex)
            {
                return Json(new {ResponseCode = "02", Message = "Invalid HTTP Post Parameter:: "+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetErranderCardDetails(string ErranderID)
        {
            try
            {
                var retVal = db.CardDetails.Where(x => x.ErranderID == ErranderID).Select(x => new
                {
                    CardType = x.CardType.Trim(),
                    MaskedCardNo = x.MaskedCardNo.Trim(),
                }).ToList();

                return Json(new {ResponseCode = "00", Message= "Request Successful", data = retVal }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new {ResponseCode = "02", Message = "Invalid HTTP Post Parameter:: "+ex.Message, data = string.Empty }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetErranderHistory(string ErranderID)
        {
            try
            {
                var retVal = (from x in db.Requests where x.IsComplete == true && x.Status.Equals("Stopped", StringComparison.OrdinalIgnoreCase) && x.ErranderID == ErranderID
                                    join y in db.ErrandTypes on x.ErrandID equals y.ErrandTypeId.ToString()
                                   select new {
                                       Date = x.Date,
                                       PickUpLocation = x.PickUpLocation.Trim(),
                                       DropOffLocation = x.DropOffLocation.Trim(),
                                       IsComplete = x.IsComplete,
                                       Status = x.Status.Trim(),
                                       ErrandName = y.ErrandName.Trim(),
                                       Catetory = y.Category.Trim(),
                                       Cost = y.Cost
                                   }).ToList();

                return Json(new {ResponseCode = "00", Message = "Request Successful", data = retVal }, JsonRequestBehavior.AllowGet);
                                    
            }
            catch(Exception ex)
            {
                return Json(new {ResponseCode = "02", Message = "Invalid HTTP Post Parameter:: "+ex.Message, data = string.Empty }, JsonRequestBehavior.AllowGet);
            }
        }
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult GetErrandeeHistory(string ErrandeeID)
        {
            try
            {
                var retVal = (from x in db.Requests
                              where x.IsComplete == true && x.Status.Equals("Stopped", StringComparison.OrdinalIgnoreCase) && x.ErranderID == ErrandeeID
                              join y in db.ErrandTypes on x.ErrandID equals y.ErrandTypeId.ToString()
                              select new
                              {
                                  Date = x.Date,
                                  PickUpLocation = x.PickUpLocation.Trim(),
                                  DropOffLocation = x.DropOffLocation.Trim(),
                                  IsComplete = x.IsComplete,
                                  Status = x.Status.Trim(),
                                  ErrandName = y.ErrandName.Trim(),
                                  Catetory = y.Category.Trim(),
                                  Cost = y.Cost.Trim()
                              }).ToList();

                return Json(new { ResponseCode = "00", Message = "Request Successful", data = retVal }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { ResponseCode = "02", Message = "Invalid HTTP Post Parameter:: " + ex.Message, data = string.Empty }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public JsonResult LiquidateErrandeeEarnings(string ErrandeeID, decimal Amount)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                return Json(new { ResponseCode = "02", Message = "Invalid HTTP Post Parameter:: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        //[HttpPost]
        //public JsonResult GetClosestErrandee()
        //{
        //    try
        //    {

        //    }
        //    catch(Exception)
        //    {
        //        //throw;
        //        return Json(new { ResponseCode = "05", Message = "Invalid HTTP POST Parameter" }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        #endregion
    }
}     