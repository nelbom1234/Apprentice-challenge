using System.Collections.Generic;

namespace MarioPizza;

public class Menu : IMenu
{
  public ICollection<IPizza> AllPizzas { get; set; }
  
  public ICollection<string> FindPizza(IList<string> mustHaveIngredients, IList<string> wontHaveIngredients)
  {
    //keep track of which pizzas are still valid as we go through each ingredient

    ICollection<IPizza> validPizzas = new List<IPizza>(AllPizzas);

    //iterate through ingredients we want, if they are not included in a pizza, we remove it from validPizzas
    foreach(string ingredient in mustHaveIngredients)
    {
      //Create a copy of validPizzas for the iteration since we will be modifying it mid-loop which will throw an exception if we do so directly on the list being iterated on
      foreach(IPizza pizza in validPizzas.ToList())
      {
        if (!pizza.Ingredients.Contains(ingredient))
        {
          validPizzas.Remove(pizza);
        }
      }
    }

    //iterate through ingredients we do not want, if they are included in a pizza, we remove it from validPizzas
    foreach(string ingredient in wontHaveIngredients)
    {
      foreach(IPizza pizza in validPizzas.ToList())
      {
        if (pizza.Ingredients.Contains(ingredient))
        {
          validPizzas.Remove(pizza);
        }
      }
    }

    //extract the names of the valid pizzas, so they can be returned
    ICollection<string> validPizzaNames= new List<string> {};
    foreach(IPizza pizza in validPizzas)
    {
      validPizzaNames.Add(pizza.Name);
    }
    return new List<string>(validPizzaNames);
  }
}
