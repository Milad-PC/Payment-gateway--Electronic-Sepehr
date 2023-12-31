using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace SaderatAPI
{
    public class RestAPI
    {
        public TokenResp GetToken(TokenReq src)
        {
            var client = new HttpClient { BaseAddress = new Uri("https://sepehr.shaparak.ir:8081") };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var myContent = new JavaScriptSerializer().Serialize(src);
            var buffer = Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var respnseMessage = client.PostAsync("V1/PeymentApi/GetToken", byteContent);
            var responseJson = respnseMessage.Result;
            string res = responseJson.Content.ReadAsStringAsync().Result;
            return new JavaScriptSerializer().Deserialize<TokenResp>(res);
        }
        public AdviceResp Verify(AdviceReq src)
        {
            var client = new HttpClient { BaseAddress = new Uri("https://sepehr.shaparak.ir:8081") };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var myContent = new JavaScriptSerializer().Serialize(src);
            var buffer = Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var respnseMessage = client.PostAsync("V1/PeymentApi/Advice", byteContent);
            var responseJson = respnseMessage.Result;
            string res = responseJson.Content.ReadAsStringAsync().Result;
            return new JavaScriptSerializer().Deserialize<AdviceResp>(res);
        }
        public AdviceResp RollBack(AdviceReq src)
        {
            var client = new HttpClient { BaseAddress = new Uri("https://sepehr.shaparak.ir:8081") };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var myContent = new JavaScriptSerializer().Serialize(src);
            var buffer = Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var respnseMessage = client.PostAsync("V1/PeymentApi/Rollback", byteContent);
            var responseJson = respnseMessage.Result;
            string res = responseJson.Content.ReadAsStringAsync().Result;
            return new JavaScriptSerializer().Deserialize<AdviceResp>(res);
        }
        public string GetStatus(int status)
        {
            string x = "نامشخص";
            switch (status)
            {
                case 0:
                    x = " تراکنش موفق ";
                    break;
                case -1:
                    x = " تراکنش پیدا نشد ";
                    break;
                case -2:
                    x = " عدم تطابق آی پی یا بسته بودن پورت 8081 ";
                    break;
                case -3:
                    x = " خطای عمومی ";
                    break;
                case -4:
                    x = " امکان انجام درخواست برای این تراکنش وجود ندارد ";
                    break;
                case -5:
                    x = " آی پی نامعتبر است ";
                    break;
                case -6:
                    x = " سرویس برگشت برای پذیرنده فعال نیست ";
                    break;
            }
            return x;
        }
    }
    public class TokenReq
    {
        [Range(minimum: 1000, maximum: long.MaxValue,ErrorMessage = "حداقل مبلغ 1000 ریال است")]
        [Required]
        public long Amount { get; set; }
        [MaxLength(500)]
        [Required]
        public string CallbackUrl { get; set; } = "https://localhost:44395/Home/Verify";
        [Display(Name = "شماره فاکتور")]
        [MaxLength(100)]
        [Required]
        [Key]
        // be ezaye har pazirande bayad yekta bashad
        public string invoiceID { get; set; }
        [Required]
        public long terminalID { get; set; }
        /*
         * JSON Bashad
         * etelaate takmili site harchi
         * nabayad char khas dashte bashe (*'"xp_%!+- ...)
         */
        public string Payload { get; set; } = "";
        public string email { get; set; }
    }
    public class TokenResp
    {
        // if 0 = success
        public int Status { get; set; }
        public string AccessToken { get; set; }
    }
    public class PayRequest
    {
        [Required]
        [MaxLength(3000)]
        public string token { get; set; }
        [Required]
        public long terminalID { get; set; }
        public string nationalCode { get; set; }

    }
    public class PayResponse
    {
        /* 0 yni movafaq
         * -1 yni enseraf
         * -2 yni etmam zaman
         */
        public int respcode { get; set; }
        [Display(Name = "متن نتیجه تراکنش")]
        public string respmsg { get; set; }
        [Display(Name = "مبلغ کسر شده از مشتری")]
        public long amount { get; set; }
        [Display( Name = " شماره فاکتور ")]
        public string invoiceid { get; set; }
        [Display(Name = " اطلاعاتی که پذیرنده ارسال کرد ")]
        public string payload { get; set; }
        [Display(Name = " شماره ترمینال ")]
        public long terminalid { get; set; }
        [Display(Name = " شماره پیگیری ")]
        public long tracenumber { get; set; }
        // bayad negah dari she hatman to db
        [Display(Name = " شماره سند بانکی ")]
        public long rrn { get; set; }
        [Display(Name = " زمان و تاریخ پرداخت ")]
        public string datepaid { get; set; }
        [Display(Name = " رسید دیجیتال ")]
        public string digitalreceipt { get; set; }
        [Display(Name = " نام بانک صادر کننده کارت ")]
        public string issuerbank { get; set; }
        [Display(Name = " شماره کارت ")]
        public string cardnumber { get; set; }
    }

    public class AdviceReq
    {
        [Display(Name = " رسید دیجیتال ")]
        public string digitalreceipt { get; set; }
        public long Tid { get; set; }
    }

    public class AdviceResp : object
    {
        public AdviceResp() : base()
        {

        }
        /* 
         * OK - NOK - Duplicate
         */
        [Display(Name = "وضعیت")]
        public string Status { get; set; }
        /* if NOK : Code Error
         * else : return Amount
         */
        public string ReturnId { get; set; }
        [Display(Name = "متن فارسی وضعیت")]
        public string Message { get; set; }
    }
}