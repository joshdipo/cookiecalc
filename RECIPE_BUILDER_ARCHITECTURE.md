## Recipe Builder Architecture

### Overview
The Recipe Builder page allows users to create and manage cookie recipes with ingredients organized by category. Categories ensure ingredient quantities stay balanced across different ingredient types.

### Key Components

#### Domain Models

**IngredientCategory.cs** - Categorizes ingredients
- `Flour` - Flour-based ingredients
- `Sugar` - All sugar types
- `Fat` - Butter, oils, shortening
- `Liquid` - Water, milk, eggs, syrups
- `Leavening` - Baking powder, baking soda, yeast
- `Other` - Salt, vanilla extract, cocoa powder

**Recipe.cs** - Core recipe model
- `Recipe` class - Represents a complete cookie recipe
  - `Name` - Recipe name
  - `Ingredients` - List of recipe ingredients
  - `GetCategoryTotal(category)` - Get total weight of ingredients in a category (in grams)
  - `GetIngredientsByCategory()` - Get ingredients grouped by category
  - `AddIngredient(ingredient)` - Add ingredient to recipe
  - `RemoveIngredient(ingredient)` - Remove ingredient from recipe

- `RecipeIngredient` class - Represents a single ingredient in a recipe
  - `IngredientType` - Which ingredient (enum)
  - `Category` - Which category it belongs to
  - `Amount` - How much (numeric value)
  - `Unit` - Unit of measurement (ml, g, cup, oz, etc.)

#### Updated Core Files

**Ingredient.cs** - Enhanced with category mapping
- New `GetCategory()` extension method that returns the `IngredientCategory` for each ingredient
- Automatically categorizes all 21 baking ingredients

### UI Components

**RecipeBuilder.razor** (`/recipe-builder` route)
- Recipe name input field
- Add ingredient section with:
  - Ingredient dropdown (all 21 ingredients)
  - Amount input field
  - Unit selector (metric/imperial, volume/weight)
  - Add button
- Ingredients organized by category with:
  - Category name and total weight display
  - Table of ingredients with amounts, units, and remove buttons
- Summary sidebar showing:
  - Category totals in grams
  - Overall recipe weight

### Navigation
Added "Recipe Builder" link to NavMenu with book icon pointing to `/recipe-builder`

### Usage Flow
1. User navigates to Recipe Builder
2. Enters recipe name (optional)
3. Selects an ingredient from dropdown
4. Enters amount and unit
5. Clicks "Add" button
6. Ingredient appears in appropriate category section
7. Summary updates with category and total weights
8. Can remove ingredients individually
9. View all ingredients organized by category with real-time calculations

### Key Features
- **Automatic Categorization**: Each ingredient is automatically placed in correct category
- **Unit Support**: Works with metric/imperial units and volume/weight conversions
- **Weight Calculations**: All totals displayed in grams for consistency
- **Category Totals**: Easily see how much of each category is in the recipe
- **Simple Architecture**: Clean separation of concerns with reusable domain models

### Future Enhancement Points
- Add ingredient bounds/constraints per category
- Save/load recipes
- Scale recipes by total weight or category ratios
- Recipe templates/presets
- Ingredient substitution suggestions
