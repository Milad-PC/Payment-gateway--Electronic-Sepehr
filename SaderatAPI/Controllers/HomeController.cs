using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SaderatAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Buy(int mablaq,string terminal)
        {
            TokenReq req = new TokenReq();

            req.Amount = mablaq;
            req.invoiceID = Guid.NewGuid().ToString();
            req.terminalID = Convert.ToInt64(terminal);

            TokenResp rsp = new RestAPI().GetToken(req);

            ViewBag.Terminal = terminal;
            return View(rsp);

            /*
             * Hame Ina Bayad To DataBase Zakhire She
             * Ke Verify Check kne Bargasht vagheEye ya Cake
             */
        }
        [HttpPost]
        public ActionResult Verify(PayResponse irsv)
        {
            /*
             * Check kn Hame Chi Doroste Ya Na
             * Az DataBase Khodet Check Kn
             * Age Doros Bod Advice
             * Va Tamam Ettelaate Bargashti
             * Bayad to DataBase Save She
             * Makhsosan rrn, digitalreceipt, tracenumber
             */
            return View(irsv);
        }
        [HttpPost]
        public ActionResult Advice(AdviceReq req)
        {
            AdviceResp rsp = new RestAPI().Verify(req);
            return View(rsp);
        }
        [HttpPost]
        public ActionResult RollBack(AdviceReq req)
        {
            AdviceResp rsp = new RestAPI().RollBack(req);
            return View(rsp);
        }
    }
}