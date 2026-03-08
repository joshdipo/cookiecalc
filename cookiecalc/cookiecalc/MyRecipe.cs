namespace cookiecalc.MyMCookie
{
    /// <summary>
    /// Class defining a Recipe
    /// </summary>
    public class Recipe
    {
        // Fields
        public string Name { get; set; }
        public string Category { get; set; }
        public List<Measurement> Ingredients { get; set; } = new();


        // Constructors
        public Recipe(string name)
        {
            Name = name;
            Category = "";
            Ingredients = new List<Measurement>();
        }


        // Methods

        // public void AddIngredient()
        // {
            
        // }

        // public void RemoveIngredient()
        // {
            
        // }

        /// <summary>
        /// Gets a list of the distinct categories used by ingredients in the recipe
        /// </summary>
        public List<string> GetIngredientCategories()
        {
            List<string> categories = new List<string>();
            foreach (Measurement ingredient in Ingredients)
            {
                // Add each ingredient category if it is not already in the list
                if (!categories.Contains(ingredient.Ingredient.Category) && !String.IsNullOrEmpty(ingredient.Ingredient.Category))
                {
                    categories.Add(ingredient.Ingredient.Category);
                }
            }
            return categories;
        }

        /// <summary>
        /// Gets the total weight of all ingredients in the recipe
        /// </summary>
        /// <returns>double (as grams)</returns>
        private double getTotalWeight()
        {
            double total = 0;
            foreach (Measurement ingredient in Ingredients)
            {
                total += ingredient.Amount;
            }
            return total;
        }

        /// <summary>
        /// Gets the total weight of ingredients in a specific ingredient category
        /// </summary>
        private double getCategoryTotalWeight(string category)
        {
            /// not actually sure what we want to return here
            /// ingredients are measured in different units, and weight and volume dont read the same
            /// could just suck it up and return everything in grams?
            /// or return all weights in grams and all volumes in millilitres?
            /// or could return the percentage of the total quantity (of weight) for the category?
            
            double total = 0;
            foreach (Measurement ingredient in Ingredients)
            {
                if (ingredient.Ingredient.Category == category)
                {
                    total += ingredient.Amount;
                }
            }
            return total;
        }

        /// <summary>
        /// Gets the percentage of the total ingredient weight for a given category of ingredient
        /// </summary>
        public double GetCategoryPercentageOfTotal(string category)
        {
            double recipeTotal = getTotalWeight();
            double categoryTotal = getCategoryTotalWeight(category);
            double percentage = (100 / recipeTotal) * categoryTotal;
            return 0.0;
        }
    }
}