
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
        private static readonly HttpClient client = new HttpClient();

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
            try
            {
                HttpResponseMessage response = await client.GetAsync(customUrl);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                JsonToList(responseBody);
            }catch(HttpRequestException e)
            {
                throw new Exception("Http response error");
            }
        }
        public void JsonToList(String json)
        {
            var currencyListFromJson = JsonSerializer.Deserialize<Response>(json);
            if (currencyListFromJson != null && currencyListFromJson.rates != null)
            {
                ratesOfCurr = currencyListFromJson.rates;
            }
            else
            {
                throw new Exception("no rates found");
            }


        }
        public float? Average()
        {
            try
            {
                return ratesOfCurr.Select(cur => cur.mid).Average();
            }
            catch (InvalidOperationException i)
            {
                throw new Exception(i.Message);
            }
            }
        public float? CountMin()
        {
            try
            {
                return ratesOfCurr.Select(cur => cur.mid).Min();

            }catch(InvalidOperationException i)
            {
                throw new Exception(i.Message);
            }
            }
        public float? CountMax()
        {
            try
            {
                return ratesOfCurr.Select(cur => cur.mid).Max();
            }catch(InvalidOperationException i)
            {
                throw new InvalidOperationException(i.Message);
            }

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
