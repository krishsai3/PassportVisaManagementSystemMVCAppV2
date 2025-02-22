using Newtonsoft.Json;
using PVMSClientApp.Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI.WebControls;

namespace PVMSClientApp.Models.BL
{
    public class DbValidation
    {
        public List<State> validateState()
        {
            List<State> states = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44386/api/");
                var responseTalk = client.GetAsync("Validation/GetStates");
                responseTalk.Wait();
                var result = responseTalk.Result;
                var readData = result.Content.ReadAsStringAsync().Result;
                if(result.IsSuccessStatusCode)
                {
                    states = JsonConvert.DeserializeObject<List<State>>(readData);
                }
                else
                {

                }
            }
            return states;
        }
        public List<city> validateCity(string sId)
        {

            List<city> cities = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44386/api/");
                var responseTalk = client.GetAsync("Validation/GetCities?sId="+sId);
                responseTalk.Wait();
                var result = responseTalk.Result;
                var readData = result.Content.ReadAsStringAsync().Result;
                if (result.IsSuccessStatusCode)
                {
                    cities = JsonConvert.DeserializeObject<List<city>>(readData);
                }
                else
                {

                }
            }
            return cities;
        }
        
        public List<Visa_form> getvisa()
        {
            List<Visa_form> visaForms = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44386/api/");
                var responseTalk = client.GetAsync("Validation/GetVisaForms");
                responseTalk.Wait();
                var result = responseTalk.Result;
                var readData = result.Content.ReadAsStringAsync().Result;
                if (result.IsSuccessStatusCode)
                {
                    visaForms = JsonConvert.DeserializeObject<List<Visa_form>>(readData);
                }
                else
                {

                }
            }
            return visaForms;
        }
        public List<Visa_type> getvisaType1()
        {
            List<Visa_type> visaTypes = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44386/api/");
                var responseTalk = client.GetAsync("Validation/GetVisaTypes");
                responseTalk.Wait();
                var result = responseTalk.Result;
                var readData = result.Content.ReadAsStringAsync().Result;
                if (result.IsSuccessStatusCode)
                {
                    visaTypes = JsonConvert.DeserializeObject<List<Visa_type>>(readData);
                }
                else
                {

                }
            }
            return visaTypes;
        }
    }
}