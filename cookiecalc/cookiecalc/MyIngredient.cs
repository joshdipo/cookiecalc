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
            IngredientList.Add(
                nameToKey(name),
                new Ingredient(name, category, density)
            );
        }

        public static void SetIngredient(string name, string category, double density)
        {
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