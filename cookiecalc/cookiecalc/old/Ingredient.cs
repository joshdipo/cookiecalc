namespace cookiecalc.Measurement
{
    /// <summary>
    /// Common baking ingredients with their densities (grams per milliliter).
    /// </summary>
    public enum Ingredient
    {
        // Flours
        AllPurposeFlour = 0,      // 0.530 g/ml
        CakeFlour = 1,            // 0.510 g/ml
        WholeWheatFlour = 2,      // 0.520 g/ml
        
        // Sugars
        GranulatedSugar = 3,      // 0.800 g/ml
        CasterSugar = 4,          // 0.800 g/ml
        BrownSugar = 5,           // 0.850 g/ml
        PowderedSugar = 6,        // 0.560 g/ml
        
        // Fats
        Butter = 7,               // 0.911 g/ml
        Oil = 8,                  // 0.920 g/ml
        Shortening = 9,           // 0.885 g/ml
        
        // Liquids
        Water = 10,               // 1.000 g/ml
        Milk = 11,                // 1.032 g/ml
        Eggs = 12,                // 1.050 g/ml
        Honey = 13,               // 1.420 g/ml
        MapleSyrup = 14,          // 1.380 g/ml
        
        // Leavening
        BakingPowder = 15,        // 1.600 g/ml
        BakingSoda = 16,          // 2.200 g/ml
        Yeast = 17,               // 1.050 g/ml
        
        // Other
        Salt = 18,                // 2.160 g/ml
        VanillaExtract = 19,      // 0.880 g/ml
        CocoaPowder = 20          // 0.600 g/ml
    }

    /// <summary>
    /// Extension methods to get ingredient properties.
    /// </summary>
    public static class IngredientExtensions
    {
        /// <summary>
        /// Gets the density of an ingredient in grams per milliliter.
        /// </summary>
        public static double GetDensity(this Ingredient ingredient)
        {
            return ingredient switch
            {
                // Flours
                Ingredient.AllPurposeFlour => 0.530,
                Ingredient.CakeFlour => 0.510,
                Ingredient.WholeWheatFlour => 0.520,
                
                // Sugars
                Ingredient.GranulatedSugar => 0.800,
                Ingredient.CasterSugar => 0.800,
                Ingredient.BrownSugar => 0.850,
                Ingredient.PowderedSugar => 0.560,
                
                // Fats
                Ingredient.Butter => 0.911,
                Ingredient.Oil => 0.920,
                Ingredient.Shortening => 0.885,
                
                // Liquids
                Ingredient.Water => 1.000,
                Ingredient.Milk => 1.032,
                Ingredient.Eggs => 1.050,
                Ingredient.Honey => 1.420,
                Ingredient.MapleSyrup => 1.380,
                
                // Leavening
                Ingredient.BakingPowder => 1.600,
                Ingredient.BakingSoda => 2.200,
                Ingredient.Yeast => 1.050,
                
                // Other
                Ingredient.Salt => 2.160,
                Ingredient.VanillaExtract => 0.880,
                Ingredient.CocoaPowder => 0.600,
                
                _ => throw new ArgumentException($"Unknown ingredient: {ingredient}")
            };
        }

        /// <summary>
        /// Gets the category of an ingredient.
        /// </summary>
        public static IngredientCategory GetCategory(this Ingredient ingredient)
        {
            return ingredient switch
            {
                // Flours
                Ingredient.AllPurposeFlour => IngredientCategory.Flour,
                Ingredient.CakeFlour => IngredientCategory.Flour,
                Ingredient.WholeWheatFlour => IngredientCategory.Flour,
                
                // Sugars
                Ingredient.GranulatedSugar => IngredientCategory.Sugar,
                Ingredient.CasterSugar => IngredientCategory.Sugar,
                Ingredient.BrownSugar => IngredientCategory.Sugar,
                Ingredient.PowderedSugar => IngredientCategory.Sugar,
                
                // Fats
                Ingredient.Butter => IngredientCategory.Fat,
                Ingredient.Oil => IngredientCategory.Fat,
                Ingredient.Shortening => IngredientCategory.Fat,
                
                // Liquids
                Ingredient.Water => IngredientCategory.Liquid,
                Ingredient.Milk => IngredientCategory.Liquid,
                Ingredient.Eggs => IngredientCategory.Liquid,
                Ingredient.Honey => IngredientCategory.Liquid,
                Ingredient.MapleSyrup => IngredientCategory.Liquid,
                
                // Leavening
                Ingredient.BakingPowder => IngredientCategory.Leavening,
                Ingredient.BakingSoda => IngredientCategory.Leavening,
                Ingredient.Yeast => IngredientCategory.Leavening,
                
                // Other
                Ingredient.Salt => IngredientCategory.Other,
                Ingredient.VanillaExtract => IngredientCategory.Other,
                Ingredient.CocoaPowder => IngredientCategory.Other,
                
                _ => IngredientCategory.Other
            };
        }

        /// <summary>
        /// Gets the display name of an ingredient.
        /// </summary>
        public static string GetDisplayName(this Ingredient ingredient)
        {
            return ingredient.ToString()
                .InsertSpacesBetweenWords()
                .ToLower();
        }

        private static string InsertSpacesBetweenWords(this string str)
        {
            var result = "";
            foreach (var c in str)
            {
                if (char.IsUpper(c) && result.Length > 0)
                {
                    result += " ";
                }
                result += c;
            }
            return result;
        }
    }
}
