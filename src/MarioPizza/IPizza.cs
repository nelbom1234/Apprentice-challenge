using System.Collections.Generic;

namespace MarioPizza;

public interface IPizza
{
  string Name { get; }

  ICollection<string> Ingredients { get; }
}
