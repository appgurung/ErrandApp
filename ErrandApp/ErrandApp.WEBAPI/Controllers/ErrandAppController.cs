using ErrandApp.API.Controllers;
using ErrandApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ErrandApp.WEBAPI.Controllers
{
    /// <summary>
    /// RESTFUL API SERVICES FOR MOBILE CONSUMPTION V 1.0 
    /// </summary>
    [RoutePrefix("api/v1/gateway")]
    
    public class ErrandAppController : ApiController
    {
        GateWaySvc _svc = new GateWaySvc();

        /// <summary>
        /// Use this to create errandee or errand runners
        /// </summary>
        /// <param name="errandee"></param>
        /// <returns>Response code 00 means errandee was created"</returns>
        [HttpPost]
        [Route("createErrandee")]
        public IHttpActionResult createErrandee(CreateErrandee errandee)
        {
            object resp = _svc.CreateErrandee(errandee);

            return Ok(resp);
        }

        /// <summary>
        /// Use this to update errandee or errand runners
        /// </summary>
        /// <param name="errandee"></param>
        /// <returns>Response code 00 means update was successful</returns>
        [HttpPost]
        [Route("updateErrandee")]
        public IHttpActionResult updateErrandee(CreateErrandee errandee)
        {
            object resp = _svc.UpdateErrandee(errandee);

            return Ok(resp);
        }

        /// <summary>
        /// Use this to delete errandee or errand runners
        /// </summary>
        /// <param name="Email"></param>
        /// <returns>Response code 00 means successful</returns>
        [HttpGet]
        [Route("deleteErrandee")]
        public IHttpActionResult deleteErrandee(string Email)
        {
            object resp = _svc.DeleteErrandee(Email);

            return Ok(resp);
        }

        /// <summary>
        /// Use this to authenticate errandee login process. Identity can be either Email or PhoneNo. After login, data of errandee will be returned
        /// </summary>
        /// <param name="login"></param>
        /// <returns>Response code 00 means successful, 02 means invalid HTTP Post/Get Parameters.</returns>
        [HttpPost]
        [Route("erandeeLoginAuthentication")]
        public IHttpActionResult erandeeLoginAuthentication(Login login)
        {
            object resp = _svc.ErranderLoginAuthentication(login);

            return Ok(resp);
        }


        /// <summary>
        /// Use this to update password. Identity can be either errandee Email or PhoneNo
        /// </summary>
        /// <param name="Identity"></param>
        /// <param name="NewPassword"></param>
        /// <returns>Response code 00 means password update request was successful</returns>
        [HttpPost]
        [Route("updatePassword")]
        public IHttpActionResult updatePassword(string Identity, string NewPassword)
        {
            object resp = _svc.UpdatePassword(Identity, NewPassword);

            return Ok(resp);
        }

        /// <summary>
        /// Use this to upload image
        /// </summary>
        /// <param name="file"></param>
        /// <param name="Identity"></param>
        /// <returns>Response code 00 means image was uploaded</returns>
        [HttpPost]
        [Route("uploadProfileImage")]
        public IHttpActionResult uploadProfileImage(HttpPostedFileBase file, string Identity)
        {
            object resp = _svc.UploadProfileImage(file, Identity);

            return Ok(resp);
        }


        /// <summary>
        /// Use this to create errander/ errander owners
        /// </summary>
        /// <param name="errander"></param>
        /// <returns>Response code 00 meeans errander was created successful</returns>
        [HttpPost]
        [Route("createErrander")]
        public IHttpActionResult createErrander(CreateErrander errander)
        {
            object resp = _svc.CreateErrander(errander);

            return Ok(resp);
        }

        /// <summary>
        /// Use this to authenticate errander for login process. errander details will be returned if cussessfuly authenticated
        /// </summary>
        /// <param name="login"></param>
        /// <returns>Response code 00 means errander login was successful</returns>
        [HttpPost]
        [Route("erranderLoginAuthentication")]
        public IHttpActionResult erranderLoginAuthentication(Login login)
        {
            object resp = _svc.ErranderLoginAuthentication(login);

            return Ok(resp);
        }
        
        /// <summary>
        /// Use this to update errander password
        /// </summary>
        /// <param name="Identity"></param>
        /// <param name="NewPassword"></param>
        /// <returns>Response code 00 means errnader password was updated successfuly</returns>
        [HttpPost]
        [Route("updateErranderPassword")]
        public IHttpActionResult updateErranderPassword(string Identity, string NewPassword)
        {
            object resp = _svc.UpdateErranderPassword(Identity, NewPassword);

            return Ok(resp);
        }


        /// <summary>
        /// Use this to create errand types.
        /// </summary>
        /// <param name="ErrandName"></param>
        /// <param name="Category"></param>
        /// <param name="Cost"></param>
        /// <returns>Response code 00 means errand type created successful</returns>
        [HttpPost]
        [Route("createErrandType")]
        public IHttpActionResult createErrandType(string ErrandName, string Category, string Cost)
        {
            object resp = _svc.CreateErrandType(ErrandName, Category, Cost);

            return Ok(resp);
        }

        /// <summary>
        /// Use this to delete errand type
        /// </summary>
        /// <param name="ErrandID"></param>
        /// <returns>Response code 00 means errand type deleted successfuly</returns>
        [HttpGet]
        [Route("deleteErrandType")]
        public IHttpActionResult deleteErrandType(string ErrandID)
        {
            object resp = _svc.DeleteErrandType(ErrandID);

            return Ok(resp);
        }

        /// <summary>
        /// Use this to get all errand types created
        /// </summary>
        /// <returns>Response code 00 means all errand types successfuly retrievd</returns>
        [HttpGet]
        [Route("getAllErrandTypes")]
        public IHttpActionResult getAllErrandTypes()
        {
            object resp = _svc.GetAllErrandTypes();

            return Ok(resp);
        }

        /// <summary>
        /// Use this to uodate errand types
        /// </summary>
        /// <param name="ErrandID"></param>
        /// <param name="ErrandName"></param>
        /// <param name="Cost"></param>
        /// <param name="Category"></param>
        /// <returns>Response code 00 means errand type updated successfuly</returns>
        [HttpPost]
        [Route("updateErrandType")]
        public IHttpActionResult updateErrandType(string ErrandID, string ErrandName, string Cost, string Category)
        {
            object resp = _svc.UpdateErrandType(ErrandID, ErrandName, Cost, Category);

            return Ok(resp);
        }

        /// <summary>
        /// Use this to start an errand. At this point the errandee is connected to an errander, and the errandee is no longer available for another errand while still running an errand
        /// </summary>
        /// <param name="ErrandID"></param>
        /// <param name="ErranderID"></param>
        /// <param name="ErrandeeID"></param>
        /// <param name="PickUpLocation"></param>
        /// <param name="DropOfLocation"></param>
        /// <returns>Response code 00 means errand started</returns>
        [HttpPost]
        [Route("startErrand")]
        public IHttpActionResult startErrand(string ErrandID, string ErranderID, string ErrandeeID, string PickUpLocation, string DropOfLocation)
        {
            object resp = _svc.StartErrand(ErrandID, ErranderID, ErrandeeID, PickUpLocation, DropOfLocation);

            return Ok(resp);
        }

        /// <summary>
        /// Use this to stop an errand. When errand is stoped the errandee can be available for a new errand
        /// </summary>
        /// <param name="ErrandID"></param>
        /// <param name="ErrandeeID"></param>
        /// <returns>Response code 00 means errand stoped</returns>
        [HttpPost]
        [Route("stopErrand")]
        public IHttpActionResult stopErrand(string ErrandID, string ErrandeeID)
        {
            object resp = _svc.StopErrand(ErrandID, ErrandeeID);

            return Ok(resp);
        }

        /// <summary>
        /// Use this to get all available errandees that can run an errand.
        /// </summary>
        /// <returns>Response code 00 means available errandees retrieved successfuly</returns>
        [HttpGet]
        [Route("errandeeAvailability")]
        public IHttpActionResult errandeeAvailability()
        {
            object resp = _svc.ErrandeeAvailability();

            return Ok(resp);
        }

        /// <summary>
        /// Use this to send errandee location
        /// </summary>
        /// <param name="ErrandeeEmail"></param>
        /// <param name="Lat"></param>
        /// <param name="Long"></param>
        /// <returns>Response code 00 means errandee location sent</returns>
        [HttpPost]
        [Route("sendErrandeeLocation")]
        public IHttpActionResult sendErrandeeLocation(string ErrandeeEmail, string Lat, string Long)
        {
            object resp = _svc.SendErrandeeLocation(ErrandeeEmail, Lat, Long);

            return Ok(resp);
        }


        /// <summary>
        /// Use this to update errandee location
        /// </summary>
        /// <param name="ErrandeeEmail"></param>
        /// <param name="Lat"></param>
        /// <param name="Long"></param>
        /// <returns>Response 00 means errandee location updated</returns>
        [HttpPost]
        [Route("updateErrandeeLocation")]
        public IHttpActionResult updateErrandeeLocation(string ErrandeeEmail, string Lat, string Long)
        {
            object resp = _svc.UpdateErrandeeLocation(ErrandeeEmail, Lat, Long);

            return Ok(resp);
        }

        /// <summary>
        /// Use this to get all errandee locations
        /// </summary>
        /// <returns>Response code 00 means all errandee locations retrived successfuly</returns>
        [HttpGet]
        [Route("getAllErrandeeLocations")]
        public IHttpActionResult getAllErrandeeLocations()
        {
            object resp = _svc.GetAllErrandeeLocations();

            return Ok(resp);
        }

        /// <summary>
        /// Use this to send errander location
        /// </summary>
        /// <param name="ErranderEmail"></param>
        /// <param name="Lat"></param>
        /// <param name="Long"></param>
        /// <returns>Response code 00 means errander location sent successfuly</returns>
        [HttpPost]
        [Route("sendErranderLocation")]
        public IHttpActionResult sendErranderLocation(string ErranderEmail, string Lat, string Long)
        {
            object resp = _svc.SendErranderLocation(ErranderEmail, Lat, Long);

            return Ok(resp);
        }

        /// <summary>
        /// Use this to update errander location
        /// </summary>
        /// <param name="ErranderEmail"></param>
        /// <param name="Lat"></param>
        /// <param name="Long"></param>
        /// <returns>Response code 00 means errander location updated successfuly</returns>
        [HttpPost]
        [Route("updateErranderLocation")]
        public IHttpActionResult updateErranderLocation(string ErranderEmail, string Lat, string Long)
        {
            object resp = _svc.UpdateErranderLocation(ErranderEmail, Lat, Long);

            return Ok(resp);
        }


        /// <summary>
        /// Use this to get all errander locations
        /// </summary>
        /// <returns>Reponse code 00 means errander locations retieved successfuly</returns>
        [HttpGet]
        [Route("getAllErranderLocations")]
        public IHttpActionResult getAllErranderLocations()
        {
            object resp = _svc.GetAllErranderLocations();

            return Ok(resp);
        }


        /// <summary>
        /// Use this to create errander payment details
        /// </summary>
        /// <param name="AccountName"></param>
        /// <param name="AccountNo"></param>
        /// <param name="BankName"></param>
        /// <param name="ErrandeeID"></param>
        /// <returns>Response code 00 means errander payment details created successfuly</returns>
        [HttpPost]
        [Route("createErrandeePaymentDetails")]
        public IHttpActionResult createErrandeePaymentDetails(string AccountName, long AccountNo, string BankName, string ErrandeeID)
        {
            object resp = _svc.CreateErrandeePaymentDetails(AccountName, AccountNo, BankName, ErrandeeID);

            return Ok(resp);
        }

        /// <summary>
        /// Use this to get errande payment details
        /// </summary>
        /// <param name="ErrandeeID"></param>
        /// <returns>Response code 00 means errander payment details retrieved</returns>
        [HttpGet]
        [Route("getErrandeePaymentDetails")]
        public IHttpActionResult getErrandeePaymentDetails(string ErrandeeID)
        {
            object resp = _svc.GetErrandeePaymentDetails(ErrandeeID);

            return Ok(resp);
        }

        /// <summary>
        /// Use this to update errandee payment details
        /// </summary>
        /// <param name="AccountName"></param>
        /// <param name="AccountNo"></param>
        /// <param name="BankName"></param>
        /// <param name="ErrandeeID"></param>
        /// <returns>Response code 00 means errandee payment details updated</returns>
        [HttpPost]
        [Route("updateErrandeePaymentDetails")]
        public IHttpActionResult updateErrandeePaymentDetails(string AccountName, long AccountNo, string BankName, string ErrandeeID)
        {
            object resp = _svc.UpdateErrandeePaymentDetails(AccountName, AccountNo, BankName, ErrandeeID);

            return Ok(resp);
        }

        /// <summary>
        /// Use this to post errandee earnings when an errand is completed successfuly.
        /// </summary>
        /// <param name="Amount"></param>
        /// <param name="ErrandeeID"></param>
        /// <returns>Response code 00 means errandee earning created successful</returns>
        [HttpPost]
        [Route("postErrandeeEarnings")]
        public IHttpActionResult postErrandeeEarnings(decimal Amount, string ErrandeeID)
        {
            object resp = _svc.PostErrandeeEarnings(Amount, ErrandeeID);

            return Ok(resp);
        }

        /// <summary>
        /// Use this to get errandee earnings.
        /// </summary>
        /// <param name="ErrandeeID"></param>
        /// <returns>Response code 00 means errandee earnings retrieved successfuly</returns>
        [HttpGet]
        [Route("getErrandeeEarnings")]
        public IHttpActionResult getErrandeeEarnings(string ErrandeeID)
        {
            object resp = _svc.GetErrandeeEarnings(ErrandeeID);

            return Ok(resp);
        }

        /// <summary>
        /// Use this to rate an errandee. Rating 1 means 1 STAR, rating 2 means 1 STARS, and rating 5 means 5 STARS
        /// </summary>
        /// <param name="ErrandeeID"></param>
        /// <param name="Review"></param>
        /// <param name="Rating"></param>
        /// <param name="ErranderID"></param>
        /// <returns>Response code 00 means rating successfuly posted/created</returns>
        [HttpPost]
        [Route("rateErrandee")]
        public IHttpActionResult rateErrandee(string ErrandeeID, string Review, int Rating, string ErranderID)
        {
            object resp = _svc.RateErrandee(ErrandeeID, Review, Rating, ErranderID);

            return Ok(resp);
        }

        /// <summary>
        /// Use this to get errandee rating. 'StarRating' is the average rating, while 'data' displays an array of ratings
        /// </summary>
        /// <param name="ErrandeeID"></param>
        /// <returns>Response code 00 means rating successfuly retrieved.</returns>
        [HttpGet]
        [Route("getErrandeeRating")]
        public IHttpActionResult getErrandeeRating(string ErrandeeID)
        {
            object resp = _svc.GetErrandeeRating(ErrandeeID);

            return Ok(resp);
        }

        /// <summary>
        /// Use this to set errander payment method
        /// </summary>
        /// <param name="ErranderID"></param>
        /// <param name="PayMethod"></param>
        /// <returns>Response code 00 means payment method set successfuly.</returns>
        [HttpPost]
        [Route("setErranderPaymentMethod")]
        public IHttpActionResult setErranderPaymentMethod(string ErranderID, string PayMethod)
        {
            object resp = _svc.SetErranderPaymentMethod(ErranderID, PayMethod);

            return Ok(resp);
        }

        /// <summary>
        /// Use this to update payment method
        /// </summary>
        /// <param name="ErranderID"></param>
        /// <param name="PayMethod"></param>
        /// <returns>Response code 00 means payment method update</returns>
        [HttpPost]
        [Route("updateErranderPaymentMethod")]
        public IHttpActionResult updateErranderPaymentMethod(string ErranderID, string PayMethod)
        {
            object resp = _svc.UpdateErranderPaymentMethod(ErranderID, PayMethod);

            return Ok(resp);
        }

        /// <summary>
        /// Use this to set / accept or store errnader card details or information
        /// </summary>
        /// <param name="ErranderID"></param>
        /// <param name="CardNo"></param>
        /// <param name="Cvv"></param>
        /// <param name="Exp"></param>
        /// <returns>Response code 00 means errander card details stored successfuly.</returns>
        [HttpPost]
        [Route("setErranderCardDetails")]
        public IHttpActionResult setErranderCardDetails(string ErranderID, string CardNo, string Cvv, string Exp)
        {
            object resp = _svc.SetErranderCardDetails(ErranderID, CardNo, Cvv, Exp);

            return Ok(resp);
        }

        /// <summary>
        /// Use this to get errander card details
        /// </summary>
        /// <param name="ErranderID"></param>
        /// <returns>Reponse code 00 means errander card details successfuly retrieved</returns>
        [HttpGet]
        [Route("getErranderCardDetails")]
        public IHttpActionResult getErranderCardDetails(string ErranderID)
        {
            object resp = _svc.GetErranderCardDetails(ErranderID);

            return Ok(resp);
        }

        /// <summary>
        /// Use this to get errander errand history
        /// </summary>
        /// <param name="ErranderID"></param>
        /// <returns>Response 00 means history succcesfuly retrieved</returns>
        [HttpGet]
        [Route("getErranderHistory")]
        public IHttpActionResult getErranderHistory(string ErranderID)
        {
            object resp = _svc.GetErranderHistory(ErranderID);

            return Ok(resp);
        }

        /// <summary>
        /// Use this to get errandee errand history
        /// </summary>
        /// <param name="ErrandeeID"></param>
        /// <returns>Response 00 means history successfuly retrieved</returns>
        [HttpGet]
        [Route("getErrandeeHistory")]
        public IHttpActionResult getErrandeeHistory(string ErrandeeID)
        {
            object resp = _svc.GetErrandeeHistory(ErrandeeID);

            return Ok(resp);
        }

        /// <summary>
        /// Use this to get errander payment method
        /// </summary>
        /// <param name="ErranderID"></param>
        /// <returns>Response 00 means payment method successful retrieved</returns>
        [HttpGet]
        [Route("getErranderPaymentMethod")]
        public IHttpActionResult getErranderPaymentMethod(string ErranderID)
        {
            object resp = _svc.GetErranderPaymentMethod(ErranderID);

            return Ok(resp);
        }
    }
}