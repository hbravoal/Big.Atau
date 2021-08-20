using System;

namespace Itau.Common.DTO.Response
{
    public class ReCaptchaResponse
    {
        public bool success { get; set; }
        public DateTime challenge_ts { get; set; }
        public string hostname { get; set; }
    }
}