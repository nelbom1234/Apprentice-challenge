using System;
using System.Collections;
using System.Net.Http;
using System.Text.RegularExpressions;

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

    //the result is a list but as a string. This is largely unhelpful, however each date in the result is contained in curly brackets,
    //so we can use regular expressions to make a list that divides each date together with their other info, which we can then work with

    //below is a bit of a beast of a regex, made using a regex generator to check more throughly for the contents of each result before I figured out a simpler solution
    //It has been left in for your amusement.
    //string pattern = "\\{\"date\":\"[0-9]{4}-[0-9]{2}-[0-9]{2}\",\"name\":\"[^\"]*\",\"nationalHoliday\":[A-Za-z]+\\}";

    //the better regex simply checks that there are curly braces around the expression and none inside of it

    string pattern = "\\{[^}]*\\}";
    ICollection<string> intermediaryList = new List<string>();

    try
    {
      foreach (Match match in Regex.Matches(result, pattern, RegexOptions.None, TimeSpan.FromSeconds(1))) {
        intermediaryList.Add(match.Value);
      }
    }
    catch(RegexMatchTimeoutException) {}

    //split each date into its parts, date, name and nationalHoliday. Checking whether it is a national holiday, 
    //and if so converts the date to ints with which we can make the DateTime and add it to the list
    //we have to split it at the commas instead of just substringing the entire thing because the name can be variable length, so we want to avoid that
    //substringing like this is not the prettiest solution, but it works for our purposes
    foreach (string date in intermediaryList) {
      string[] info = date.Split(',');
      if (info[2].Substring(18, 4) == "true") {
        int year; int.TryParse(info[0].Substring(9, 4), out year);
        int month; int.TryParse(info[0].Substring(14, 2), out month);
        int day; int.TryParse(info[0].Substring(17, 2), out day);

        retList.Add(new DateTime(year, month, day));
      }

    }
    return retList;

  }
}
