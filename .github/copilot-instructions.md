# CookieCalc AI Coding Agent Instructions

## Project Overview
CookieCalc is a **Blazor WebAssembly application** (C#, `.NET 10.0`) for building and managing cookie recipes. The core value is automatic ingredient categorization and cross-unit measurement conversions to help bakers balance recipe proportions.

## Architecture & Data Flow

### Domain Model (cookiecalc/ namespace)
- **Ingredient** (enum): 21 baking ingredients with embedded densities (g/ml) for volume-to-weight conversion
- **IngredientCategory** (enum): 6 categories (Flour, Sugar, Fat, Liquid, Leavening, Other)
- **Measurement**: Converts between metric (ml/g) and imperial (cups/oz) units via static dictionaries
- **Recipe**: Aggregate that holds RecipeIngredient list; provides `GetCategoryTotal()` and `GetIngredientsByCategory()`
- **RecipeIngredient**: DTO storing ingredient type, amount, unit, and category

**Key insight**: All conversions normalize to grams as the universal unit via Measurement.ToMetric(). Categories ensure recipe balance.

### UI Layer
- **RecipeBuilder.razor** (`/recipe-builder`): Single interactive page with:
  - Recipe name input
  - Ingredient dropdown → amount → unit selector → Add button
  - Ingredients rendered in collapsible category cards (each showing category total in grams)
  - Remove buttons per ingredient

### Interactivity
- Blazor **InteractiveServerComponents** (not WASM) - stateful on server
- Two-way binding via `@bind="recipe.Name"` and `@bind="newAmount"`
- Events: `@oninput="OnIngredientSelected"`, `@onclick="AddIngredient"`

## Essential Patterns & Conventions

### Extension Methods for Enums
Use C# pattern matching on Ingredient/IngredientCategory enums:
```csharp
// IngredientExtensions.cs - GetDensity(), GetCategory(), GetDisplayName()
public static double GetDensity(this Ingredient ing) => ing switch { ... }
```
**Why**: Enums are immutable, so methods attach behavior. Follow this for any new ingredient properties.

### Unit Conversion Workflow
1. Input: Ingredient + Amount + Unit (e.g., "2 cups")
2. Measurement.ConvertVolumeToMl() + ingredient.GetDensity() → grams
3. Measurement.ToMetric() ensures metric output
4. Recipe.GetCategoryTotal() sums grams per category

**Don't create new Measurement types** — extend the static dictionaries if adding units.

### Razor Component Patterns
- `@foreach` over Enum.GetValues(typeof(Ingredient)) to populate dropdowns
- `@bind` for two-way binding to backing variables (newAmount, newUnitType)
- Use `class="form-control" / "btn btn-primary"` — Bootstrap 5 classes (in wwwroot/lib/bootstrap/)

## Developer Workflows

### Build & Run
```bash
dotnet build
dotnet run
# Launches http://localhost:5000 (dev) or https://localhost:5001 (HTTPS)
```
Configuration: launchSettings.json in Properties/. IIS Express profile included.

### Adding a New Ingredient
1. Add to `Ingredient` enum (cookiecalc/Ingredient.cs)
2. Add density to `GetDensity()` switch statement
3. Add category mapping to `GetCategory()` switch statement
4. Add display name to `GetDisplayName()` if custom formatting needed

### Debugging Conversions
- Measurement.ConvertVolumeToMl() and ConvertWeightToG() are private helpers
- Test via Measurement.ToMetric() to verify outputs normalize to grams
- Check Recipe.GetCategoryTotal() which calls `ingredient.GetDensity()` internally

## Key Files Reference
- [cookiecalc/Ingredient.cs](cookiecalc/Ingredient.cs) — ingredient enum + extensions
- [cookiecalc/Measurement.cs](cookiecalc/Measurement.cs) — unit conversions
- [cookiecalc/Recipe.cs](cookiecalc/Recipe.cs) — recipe aggregate
- [Components/Pages/RecipeBuilder.razor](Components/Pages/RecipeBuilder.razor) — UI component
- PROJECT_ARCHITECTURE.md — class diagram
- RECIPE_BUILDER_ARCHITECTURE.md — feature spec

## .NET / Blazor Notes
- **Nullable reference types enabled** — avoid null checks unless IngredientCategory is explicitly nullable
- **Implicit usings** — no need to fully qualify System.* types
- **Route prefix**: /recipe-builder (NOT /recipepecipe-builder or case variations)
- Error page: Components/Pages/Error.razor
- Navigation: NavMenu.razor (add links here)

## Future Extension Points
- Ingredient bounds per category (min/max grams)
- Save/load recipes to database
- Recipe scaling by total weight ratio
- Export as PDF/JSON
