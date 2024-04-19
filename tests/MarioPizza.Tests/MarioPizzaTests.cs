using NUnit.Framework;
using System.Collections.Generic;

namespace MarioPizza.Tests;

public class Tests
{
  private readonly IMenu fixture = new Menu
  {
    AllPizzas = new List<IPizza>{
        new Pizza{Name = "Margherita", Ingredients = new List<string>{"Tomato", "Cheese"} },
        new Pizza{Name = "Vesuvio", Ingredients = new List<string>{"Tomato", "Cheese", "Ham"} },
        new Pizza{Name = "Hawaii", Ingredients = new List<string>{"Tomato", "Cheese", "Ham", "Pineapple"} },
        new Pizza{Name = "Pepperoni", Ingredients = new List<string>{"Tomato", "Cheese", "Pepperoni"} },
        new Pizza{Name = "Calzone", Ingredients = new List<string>{"Tomato", "Cheese", "Ham"} },
    }
  };

  [Test]
  public void GIVEN_MustHaveHam_WontHavePineapple_WHEN_FindPizza_THEN_return_vesuvio_calzone()
  {
    // Arrange
    IList<string> mustHaveIngredients = new List<string> { "Ham" };
    IList<string> wontHaveIngredients = new List<string> { "Pineapple" };

    // Act
    var result = fixture.FindPizza(mustHaveIngredients, wontHaveIngredients);

    // Assert
    Assert.IsTrue(result.Contains("Vesuvio"));
    Assert.IsTrue(result.Contains("Calzone"));
    Assert.AreEqual(2, result.Count);
  }
}
