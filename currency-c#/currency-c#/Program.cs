// See https://aka.ms/new-console-template for more information
using currency_c_.CurrencyMain;

CurrecncyCalculator currency = new CurrecncyCalculator("A","EUR");

await currency.ConnectAsync();
currency.PrintResults();