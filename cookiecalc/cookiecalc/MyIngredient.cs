using System.Text.RegularExpressions;

namespace cookiecalc.MyMCookie
{
    /// <summary>
    /// Class defining an Ingredient
    /// </summary>
    public class Ingredient
    {
        public string Name { get; set; }
        public string Category { get; set; }
        /// <summary>
        /// Density is defined as grams per millilitre
        /// </summary>
        public double Density { get; set; }

        public Ingredient(string name, string category, double density)
        {
            Name = name;
            Category = category;
            Density = density;
        }
    }


    /// <summary>
    /// Class defining the dictionary of all ingredients
    /// </summary>
    public static class Ingredients
    {
        public static Dictionary<string, Ingredient> IngredientList { get; } = new();


        // Methods

        private static string nameToKey(string name)
        {
            return name.Replace(" ", "").ToLower();
        }

        /// <summary>
        /// Adds an ingredient to the dictionary 
        /// </summary>
        public static void AddIngrdient(string name, string category, double density)
        {
            // Prevent invalid inputs
            if (IngredientList.ContainsKey(nameToKey(name)))
            {
                throw new ArgumentException("Ingredient already exists");
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name cannot be empty");
            }
            if (string.IsNullOrEmpty(category))
            {
                throw new ArgumentException("Category cannot be empty");
            }
            if (density <= 0.0)
            {
                throw new ArgumentException("Density must be greater than 0");
            }
            
            IngredientList.Add(
                nameToKey(name),
                new Ingredient(name, category, density)
            );
        }

        public static void SetIngredient(string name, string category, double density)
        {
            // Prevent invalid inputs
            if (string.IsNullOrEmpty(name) || !Regex.IsMatch(name, @"^\w+(\s\w+)*$"))
            {
                throw new ArgumentException("Name is invalid");
            }
            if (string.IsNullOrEmpty(category) || !Regex.IsMatch(category, @"^\w+(\s\w+)*$"))
            {
                throw new ArgumentException("Category is invalid");
            }
            if (density <= 0.0 || !Regex.IsMatch(density.ToString(), @"^\d{1,2}(\.\d{1,3}){0,1}$"))
            {
                throw new ArgumentException("Density is invalid");
            }

            // Try and add the ingredient as new
            bool newIngredient = IngredientList.TryAdd(
                nameToKey(name),
                new Ingredient(name, category, density)
            );

            // If the ingredient was not added, update the existing ingredient entry
            if (!newIngredient)
            {
                IngredientList[nameToKey(name)].Name = name;
                IngredientList[nameToKey(name)].Category = category;
                IngredientList[nameToKey(name)].Density = density;
            }
        }

        /// <summary>
        /// Removes an ingredient from the dictionary if it exists
        /// </summary>
        public static void RemoveIngredient(string name)
        {
            IngredientList.Remove(nameToKey(name));
        }


        // public List<string> GetAllIngredients()
        // {
            
        // }
    }
}