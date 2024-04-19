using System;
using System.Text.RegularExpressions;

namespace InputValidation;

public class Validator : IValidator
{
  public bool IsValidEmail(string email)
  {
    // TODO - replace the below exception with your own implementation
    
    // is it a bit pretentious to solve this using regex? probably, but it's fun

    //we first need to build our regular expression. This is done in steps with comments for the benefit of the reader
    string reg = "";

    //an email starts with a series of alphanumeric characters, but underscores, dashes and periods are also allowed
    reg += "^[\\w-\\.]+";

    //then we need to have @ at some point after those characters
    reg += "@";

    //now we want the domain name. This includes an address followed by a tld. We'll handle the first part first
    //this consistent of the same characters as the first part in any order once again
    reg += "[\\w-\\.]+";

    //lastly we want to check for the tld. This starts with a period, followed by more alphanumeric characters
    //I will here limit that to 2-4 characters as I've never personally seen one shorter or longer,
    //however you could extend that or remove the requirement all together and just require at least 1 character
    reg += "\\.[\\w]{2,4}";

    //lastly we just make sure that we have reached the end of the string and there are no more hiding characters that would invalidate the email
    reg += "$";

    Regex regex = new Regex(reg, RegexOptions.IgnoreCase);
    return regex.IsMatch(email);

  }
}
