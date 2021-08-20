using Mastercard.Common.DTO.Response;
using System.Collections.Generic;

namespace Mastercard.UI.Business.ViewModels
{
    public class AwardViewModel
    {
        public List<AwardDTO> Awards { get; set; }
        public string message { get; set; }
    }
}