using Mastercard.Common.DTO;
using Mastercard.Common.DTO.Response;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public MisionPointDTO MisionView { get; set;}
        public string getUrlImage()
        {
            return string.Format("{0}{1}", ConfigurationManager.AppSettings["PasalaGanando.NetCommerce.Image.Url"] ,this.imageUrl.Replace("~", ""));
        }
    }
    }
