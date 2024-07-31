
using JsonSerializer = System.Text.Json.JsonSerializer;
namespace currency_c_.CurrencyMain
{
    internal class CurrecncyCalculator
    {
        private String baseUrl;
        private String table;
        private String start;
        private String end;
        private String currency;
        List<Currency> ratesOfCurr;
       
        public CurrecncyCalculator(String table, String currency)
        {
            String format = "yyyy-M-d";
            this.baseUrl = "http://api.nbp.pl/api/exchangerates/rates";
            this.end = DateTime.Today.ToString("yyyy-MM-dd");
            this.start = DateTime.Today.AddDays(-20).ToString("yyyy-MM-dd");
            this.currency = currency;
            this.table = table;

        }

        public async Task connectAsync()
        {

            String customUrl = $"{baseUrl}/{table}/{currency}/{start}/{end}";

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(customUrl);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            JsonToList(responseBody);
        }
        public void JsonToList(String json)
        {
            var currencyListFromJson = JsonSerializer.Deserialize<Response>(json);
            if (currencyListFromJson.rates !=null)
            {
                List<Currency> rates = currencyListFromJson.rates;
                ratesOfCurr = rates;
            }

           
        }
        public float? average()
        {
          return ratesOfCurr.Select(cur => cur.mid).Average();
        }
        public float? countMin()
        {
            return  ratesOfCurr.Select(cur => cur.mid).Min();
           
        }
        public float? countMax()
        {
        return  ratesOfCurr.Select(cur => cur.mid).Max();
  
        }
        public void printResults()
        {
            float? avg = average();
            float? min = countMin();
            float? max = countMax();
            Console.WriteLine($"avg {avg} min {min} max {max}");
        }
    }
}
