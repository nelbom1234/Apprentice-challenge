using System.Collections.Generic;

namespace MarioPizza;

public interface IMenu
{
  ICollection<IPizza> AllPizzas { get; }

  ICollection<string> FindPizza(IList<string> mustHaveIngredients, IList<string> wontHaveIngredients);
}
