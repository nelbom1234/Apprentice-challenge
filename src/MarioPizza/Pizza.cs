using System.Collections.Generic;

namespace MarioPizza;

public class Pizza : IPizza
{
  public string Name { get; set; }

  public ICollection<string> Ingredients { get; set; }
}
