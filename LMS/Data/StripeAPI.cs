using LMS.Data.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Dynamic;
using System.Threading.Tasks;

namespace LMS.Data
{
    public class StripeAPI
    {
        public const string BASE_URL = "https://api.stripe.com/v1/";
        public const string API_KEY = "sk_test_51IJm7eJRUfZcVjHeA4HzQx2TWQhwjfeXKxxu9LG4T8oz4KcbxujZ4VFSufMg86ZvpWe4bewN9x29mbB8c6dTx4an00k9M0b10X";
        private readonly RestClient Client;

        #region Helper Classes 
        public class Token
        {
            public string id { get; set; }
            public bool used { get; set; }
        }
        public class Source
        {
            public string id { get; set; }
            public string @object { get; set; }
            public object address_city { get; set; }
            public object address_country { get; set; }
            public object address_line1 { get; set; }
            public object address_line1_check { get; set; }
            public object address_line2 { get; set; }
            public object address_state { get; set; }
            public object address_zip { get; set; }
            public object address_zip_check { get; set; }
            public string brand { get; set; }
            public string country { get; set; }
            public object customer { get; set; }
            public object cvc_check { get; set; }
            public object dynamic_last4 { get; set; }
            public int exp_month { get; set; }
            public int exp_year { get; set; }
            public string fingerprint { get; set; }
            public string funding { get; set; }
            public string last4 { get; set; }
            public object name { get; set; }
            public object tokenization_method { get; set; }
        }

        public class Charge
        {
            public string id { get; set; }
            public int amount { get; set; }
            public int amount_captured { get; set; }

            public string balance_transaction { get; set; }
            public string calculated_statement_descriptor { get; set; }
            public int created { get; set; }
            public string currency { get; set; }
            public object failure_code { get; set; }
            public object failure_message { get; set; }
            public bool paid { get; set; }
            public string receipt_url { get; set; }
            public bool refunded { get; set; }
            public Source source { get; set; }
            public string status { get; set; }
        }
        #endregion

        public StripeAPI()
        {
            Client = new RestClient($"{BASE_URL}");
            Client.Authenticator = new HttpBasicAuthenticator(API_KEY, string.Empty);
        }

        /// <summary>
        /// Creates a one-time use card token from the passed Payment.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public async Task<Token> GenToken(Payment p)
        {
            var token = new Token();
            var card = new
            {
                number = p.CardNumber,
                exp_month = p.ExpDate.Month < 10 ? int.Parse(0.ToString() + p.ExpDate.Month.ToString()) : p.ExpDate.Month,
                exp_year = p.ExpDate.Year,
                cvc = p.CVC.ToString()
            };

            try
            {
                var request = new RestRequest("tokens", Method.POST);
                request.AddHeader("Authorization", API_KEY);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("card[number]", card.number);
                request.AddParameter("card[exp_month]", card.exp_month);
                request.AddParameter("card[exp_year]", card.exp_year);
                request.AddParameter("card[cvc]", card.cvc);
                IRestResponse response = await Client.ExecuteAsync(request);

                dynamic dtoken = JsonConvert.DeserializeObject<ExpandoObject>(response.Content, new ExpandoObjectConverter());
                token = new Token()
                {
                    id = dtoken.id,
                    used = dtoken.used
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return token;
        }

        /// <summary>
        /// Charges a card with the passed payment.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public async Task<Charge> ChargeCard(Payment p)
        {
            var token = await GenToken(p);
            var charge = new Charge();

            try
            {
                var request = new RestRequest("charges", Method.POST);
                request.AddHeader("Authorization", API_KEY);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("amount", p.AttemptAmount.ToString().Replace(".", ""));
                request.AddParameter("currency", "usd");
                request.AddParameter("source", token.id);

                IRestResponse response = await Client.ExecuteAsync(request);
                charge = JsonConvert.DeserializeObject<Charge>(response.Content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return charge;
        }
    }
}
