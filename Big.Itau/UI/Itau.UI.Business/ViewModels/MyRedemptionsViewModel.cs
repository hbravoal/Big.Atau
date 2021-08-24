using System;
using System.Configuration;

namespace Mastercard.UI.Business.ViewModels
{
    public class MyRedemptionsViewModel
    {
        public DateTime Date { get; set; }
        public string productName { get; set; }
        public string imageUrl { get; set; }
        public string productCode { get; set; }
        public string linkPdfRedemption { get; set; }
        public int type { get; set; }

        public string getUrlImage()
        {
            return string.Format("{0}{1}", ConfigurationManager.AppSettings["PasalaGanando.NetCommerce.Image.Url"], this.imageUrl.Replace("~", ""));
        }
    }
}