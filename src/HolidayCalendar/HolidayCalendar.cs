using System;
using System.Collections;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;

namespace HolidayCalendar;
public class HolidayCalendar : IHolidayCalendar
{
  public bool IsHoliday(DateTime date)
  {
    // TODO - replace the below exception with your own implementation

    //split the date into its parts as strings. Pad with 0s to fit the dats scheme of the API
    string day = date.Day.ToString().PadLeft(2, '0');
    string month = date.Month.ToString().PadLeft(2, '0');
    string year = date.Year.ToString().PadLeft(4, '0');

    //format url with given dates
    string url = $"https://api.sallinggroup.com/v1/holidays/is-holiday?date={year}-{month}-{day}";
    Task<string> response = webRequest(url);

    string result = response.Result;

    if (result == "true")
    {
      return true;
    }
    else return false;

  }

  public async Task<string> webRequest(string url)
  {

    HttpClient client = new HttpClient();

    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

    request.Headers.Add("accept", "application/json");
    //api authorization token
    request.Headers.Add("Authorization", "Bearer ce6d0fcf-9a9d-4124-8fc5-d6e9c8744b9b");

    HttpResponseMessage response = await client.SendAsync(request);
    response.EnsureSuccessStatusCode();
    string responseBody = await response.Content.ReadAsStringAsync();
    return responseBody;

  }

  public ICollection<DateTime> GetHolidays(DateTime startDate, DateTime endDate)
  {
    string startDay = startDate.Day.ToString().PadLeft(2, '0');
    string startMonth = startDate.Month.ToString().PadLeft(2, '0');
    string startYear = startDate.Year.ToString().PadLeft(4, '0');

    string endDay = endDate.Day.ToString().PadLeft(2, '0');
    string endMonth = endDate.Month.ToString().PadLeft(2, '0');
    string endYear = endDate.Year.ToString().PadLeft(4, '0');

    ICollection<DateTime> retList = new List<DateTime>();

    //format url with given dates
    string url = $"https://api.sallinggroup.com/v1/holidays?startDate={startYear}-{startMonth}-{startDay}&endDate={endYear}-{endMonth}-{endDay}&translation=en-us";
    Task<string> response = webRequest(url);

    string result = response.Result;


    //convert the json date to a JsonElement and then turn it into an array we can iterate on
    JsonElement data = JsonSerializer.Deserialize<JsonElement>(result);
    var dataArray = data.EnumerateArray();

    //Iterate on the array, checking the last value in each element if it is a holiday and if it is,
    //we convert the date in the first value to DateTime and add it to the return list
    foreach (var obj in dataArray)
    {
        var objProperties = obj.EnumerateObject();
        
        bool isHoliday = bool.Parse(objProperties.Last().Value.ToString());
        if (isHoliday)
        {
            //string holidayDate = DateTime.ParseExact(objProperties.First().Value.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime date = DateTime.ParseExact(objProperties.First().Value.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            retList.Add(date);
        }
    }

    return retList;

  }
}
