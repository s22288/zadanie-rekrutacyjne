
using JsonSerializer = System.Text.Json.JsonSerializer;
namespace currency_c_.CurrencyMain
{
    internal class CurrecncyCalculator
    {
        private String baseUrl;
        private String table;
        private String start;
        private String end;
        private String code;
        private List<Currency> ratesOfCurr;
        private readonly string format = "yyyy-MM-dd";

        public CurrecncyCalculator(String table, String currency)
        {
            this.ratesOfCurr = [];
            this.baseUrl = "http://api.nbp.pl/api/exchangerates/rates";
            this.end = DateTime.Today.ToString(format);
            this.start = DateTime.Today.AddDays(-20).ToString(format);
            this.code = currency;
            this.table = table;

        }

        public async Task ConnectAsync()
        {

            String customUrl = $"{baseUrl}/{table}/{code}/{start}/{end}";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(customUrl);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            JsonToList(responseBody);
        }
        public void JsonToList(String json)
        {
            var currencyListFromJson = JsonSerializer.Deserialize<Response>(json);
            if (currencyListFromJson != null && currencyListFromJson.rates != null)
            {
                ratesOfCurr = currencyListFromJson.rates;
            }


        }
        public float? Average()
        {
            return ratesOfCurr.Select(cur => cur.mid).Average();
        }
        public float? CountMin()
        {
            return ratesOfCurr.Select(cur => cur.mid).Min();

        }
        public float? CountMax()
        {
            return ratesOfCurr.Select(cur => cur.mid).Max();

        }
        public void PrintResults()
        {
            float? avg = Average();
            float? min = CountMin();
            float? max = CountMax();
            Console.WriteLine($"avg {avg} min {min} max {max}");
        }
    }
}
