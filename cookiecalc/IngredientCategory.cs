namespace cookiecalc.Measurement
{
    /// <summary>
    /// Categories for grouping ingredients in a recipe.
    /// Used to ensure ingredient quantities stay balanced within each category.
    /// </summary>
    public enum IngredientCategory
    {
        Flour,
        Sugar,
        Fat,
        Liquid,
        Leavening,
        Other
    }

    /// <summary>
    /// Extension methods for ingredient categories.
    /// </summary>
    public static class IngredientCategoryExtensions
    {
        /// <summary>
        /// Gets a friendly display name for the category.
        /// </summary>
        public static string GetDisplayName(this IngredientCategory category)
        {
            return category switch
            {
                IngredientCategory.Flour => "Flour",
                IngredientCategory.Sugar => "Sugar",
                IngredientCategory.Fat => "Fat",
                IngredientCategory.Liquid => "Liquid",
                IngredientCategory.Leavening => "Leavening",
                IngredientCategory.Other => "Other",
                _ => "Unknown"
            };
        }
    }
}
