using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVMSClientApp.Models
{
    public class PassportId
    {
        public string type { get; set; }
        public string countryCode {  get; set; }
        public string passportNo { get; set; }
        public string surname { get; set; }
        public string givenName { get; set; }
        public string nationality { get; set; }
        public string gender { get; set; }
        public string dob { get; set; }
        public string placeOfBirth { get; set; }
        public string placeOfIssue { get; set; }
        public string doi { get; set; }
        public string doe { get; set; }
    }
}