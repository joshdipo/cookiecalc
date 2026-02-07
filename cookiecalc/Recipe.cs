using System;
using System.Collections.Generic;

namespace cookiecalc.Measurement
{
    /// <summary>
    /// Represents a single recipe ingredient with its quantity.
    /// </summary>
    public class RecipeIngredient
    {
        public Ingredient IngredientType { get; set; }
        public IngredientCategory Category { get; set; }
        public double Amount { get; set; }
        public object Unit { get; set; } = MetricWeightUnit.Gram;

        public RecipeIngredient()
        {
        }

        public RecipeIngredient(Ingredient ingredientType, IngredientCategory category, double amount, object unit)
        {
            IngredientType = ingredientType;
            Category = category;
            Amount = amount;
            Unit = unit;
        }
    }

    /// <summary>
    /// Represents a cookie recipe with ingredients organized by category.
    /// Ensures ingredient quantities within each category remain balanced.
    /// </summary>
    public class Recipe
    {
        public string Name { get; set; } = "New Recipe";
        public List<RecipeIngredient> Ingredients { get; set; } = new();

        /// <summary>
        /// Gets the total amount of ingredients in a specific category.
        /// </summary>
        public double GetCategoryTotal(IngredientCategory category)
        {
            double total = 0;
            foreach (var ingredient in Ingredients)
            {
                if (ingredient.Category == category)
                {
                    var measurement = new Measurement(ingredient.Amount, ingredient.Unit, ingredient.IngredientType);
                    total += measurement.ConvertTo(MetricWeightUnit.Gram).Value;
                }
            }
            return total;
        }

        /// <summary>
        /// Gets all ingredients grouped by category.
        /// </summary>
        public Dictionary<IngredientCategory, List<RecipeIngredient>> GetIngredientsByCategory()
        {
            var grouped = new Dictionary<IngredientCategory, List<RecipeIngredient>>();

            foreach (var category in Enum.GetValues(typeof(IngredientCategory)))
            {
                grouped[(IngredientCategory)category] = new List<RecipeIngredient>();
            }

            foreach (var ingredient in Ingredients)
            {
                grouped[ingredient.Category].Add(ingredient);
            }

            return grouped;
        }

        /// <summary>
        /// Adds an ingredient to the recipe.
        /// </summary>
        public void AddIngredient(RecipeIngredient ingredient)
        {
            Ingredients.Add(ingredient);
        }

        /// <summary>
        /// Removes an ingredient from the recipe.
        /// </summary>
        public void RemoveIngredient(RecipeIngredient ingredient)
        {
            Ingredients.Remove(ingredient);
        }
    }
}
