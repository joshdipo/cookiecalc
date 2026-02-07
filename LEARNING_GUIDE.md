# CookieCalc Learning Guide for Blazor

## Quick Start: What Just Happened?

Your app is now running at `http://localhost:5002/recipe-builder`. You built a **C# Blazor application** that:

1. **Shows a form** where you pick an ingredient, amount, and unit
2. **Converts between metric/imperial** on the fly
3. **Groups ingredients by category** (Flour, Sugar, Fat, etc.)
4. **Calculates totals** in grams per category

This teaches you about:
- **Web components** (interactive UI)
- **Object-oriented design** (classes, enums, methods)
- **Data conversion** (the hard part you already solved!)

---

## Key Blazor Concepts

### 1. **The `.razor` File = Component + UI + Logic**

Your `RecipeBuilder.razor` file has three sections:

```razor
@page "/recipe-builder"                    ← What URL shows this page
@using cookiecalc.Measurement             ← What code namespaces are available

<!-- HTML Markup -->
<h1>Cookie Recipe Builder</h1>

@code {                                    ← C# code behind the UI
    private Recipe recipe = new();
    private void AddIngredient() { ... }
}
```

**Key insight:** In Blazor, your C# and HTML live in the **same file**. When you change data in C# methods (like `AddIngredient()`), Blazor automatically updates the HTML.

### 2. **Data Binding: `@bind` Connects HTML ↔ C#**

```razor
<input @bind="recipe.Name" />              ← Two-way binding
```

- User types in the textbox
- Blazor detects the change
- Updates `recipe.Name` in C#
- Refreshes the display

You'll see this pattern everywhere:
```razor
<input @bind="newAmount" />                ← Connects to your C# variable
<select @bind="newUnitType">               ← Blazor tracks this too
```

### 3. **Events: `@onclick`, `@oninput` Trigger Your Methods**

```razor
<button @onclick="AddIngredient">Add</button>
```

When the user clicks, Blazor calls your C# method. Here's the flow:

```
User clicks
    ↓
Blazor calls AddIngredient()
    ↓
AddIngredient() modifies recipe.Ingredients
    ↓
Blazor detects the change
    ↓
Blazor re-renders the page (shows new ingredients)
```

### 4. **`@foreach` Repeats HTML for Each Item**

```razor
@foreach (var ingredient in ingredientsInCategory)
{
    <tr>
        <td>@ingredient.IngredientType.GetDisplayName()</td>
    </tr>
}
```

This loops through your list and creates a table row for each ingredient. When you add/remove ingredients, Blazor automatically updates the loop.

---

## How Your Data Flows

### Adding an Ingredient: The Complete Journey

```
User selects "Butter" from dropdown
    ↓
@oninput="OnIngredientSelected" fires
    ↓
OnIngredientSelected() parses "Butter" → Ingredient.Butter enum
    ↓
Sets newIngredient = "Butter"
    ↓
User enters "200" grams and clicks Add
    ↓
@onclick="AddIngredient" fires
    ↓
AddIngredient() creates:
    RecipeIngredient {
        IngredientType: Ingredient.Butter,
        Category: ingredient.GetCategory() → IngredientCategory.Fat,
        Amount: 200,
        Unit: MetricWeightUnit.Gram
    }
    ↓
Adds to recipe.Ingredients list
    ↓
Blazor detects change → re-renders HTML
    ↓
GetIngredientsByCategory() groups by category
    ↓
HTML loop shows "Fat" card with "200 g Butter"
```

### Calculating Totals: Where Your Conversions Shine

The HTML shows:
```razor
Total: @recipe.GetCategoryTotal(category).ToString("F1") g
```

This calls your `Recipe` class:

```csharp
public double GetCategoryTotal(IngredientCategory category)
{
    double total = 0;
    foreach (var ingredient in Ingredients)
    {
        // For each ingredient in this category...
        var measurement = new Measurement(ingredient.Amount, ingredient.Unit, ingredient.IngredientType);
        
        // Convert to grams (your Measurement class does this!)
        total += measurement.ConvertTo(MetricWeightUnit.Gram).Value;
    }
    return total;
}
```

**This is the magic:** Whether you added 1 cup of butter or 200g, your `Measurement.ConvertTo()` normalizes everything to grams.

---

## Your Code Architecture

```
RecipeBuilder.razor (the UI)
    │
    ├─ Displays: form, ingredient list, summary
    └─ Calls: AddIngredient(), RemoveIngredient()
            ↓
        Recipe (the brain)
            │
            ├─ Holds: List<RecipeIngredient>
            ├─ Provides: GetCategoryTotal(), GetIngredientsByCategory()
            └─ Uses: Measurement class
                    │
                    └─ Holds: Ingredient enum, Unit (grams/cups/etc)
                    └─ Provides: ConvertTo() (your conversion logic!)
```

**Key pattern:** Your domain classes (`Recipe`, `Measurement`, `Ingredient`) are **data + logic**. They know how to:
- Group ingredients by category
- Convert units
- Calculate totals

Blazor just **displays** that data and **calls methods** when users interact.

---

## Understanding Enums (Important for Baking!)

Your `Ingredient` enum is a list of 21 constants:

```csharp
public enum Ingredient
{
    AllPurposeFlour = 0,
    CakeFlour = 1,
    Butter = 7,
    // ... etc
}
```

The `IngredientExtensions` class adds **methods to enums**:

```csharp
public static double GetDensity(this Ingredient ingredient)
{
    return ingredient switch
    {
        Ingredient.AllPurposeFlour => 0.530,  // g/ml
        Ingredient.Butter => 0.911,           // g/ml
        _ => throw new ArgumentException(...)
    };
}
```

**Pattern matching** (`switch` expression) is a C# feature. It says:
- "If ingredient is Butter, return 0.911"
- "Otherwise, throw an error"

This is how you convert 1 cup of butter → grams automatically!

---

## Blazor vs Traditional Web Dev

### Traditional (ASP.NET):
```
Browser sends form
    → Server processes
    → Server sends new HTML page
    → Browser shows it
(Page refresh, slow)
```

### Blazor InteractiveServerComponents (what you're using):
```
User clicks button in browser
    → Blazor detects event
    → C# method runs on server
    → Server calculates changes
    → Only changed HTML is sent
    → Browser updates smoothly
(No page refresh, fast!)
```

You're using **InteractiveServerComponents** because your `RecipeBuilder` is interactive. The state (recipe data) lives on the **server**, and the browser just displays it.

---

## Next Steps: Learning While Building

### Exercise 1: Add a Display Conversion Feature
**Goal:** Add a button that shows "Convert to Imperial" 

**What you'll learn:**
- How to create new C# methods
- How to call `Measurement.ToImperial()` on your data
- How to trigger display changes with buttons

**Steps:**
1. Add a button: `<button @onclick="ToggleImperial">Show Imperial</button>`
2. Add a field: `private bool showImperial = false;`
3. Add a method:
   ```csharp
   private void ToggleImperial()
   {
       showImperial = !showImperial;
   }
   ```
4. Update ingredient display:
   ```razor
   @if (showImperial)
   {
       <!-- Show in imperial -->
   }
   else
   {
       <!-- Show in metric -->
   }
   ```

This teaches you **conditional rendering** and **event handlers**.

### Exercise 2: Add a "Recipe Scaler" Slider
**Goal:** Multiply all ingredients by 2x, 0.5x, etc.

**What you'll learn:**
- How to transform your data with methods
- How Blazor re-renders everything automatically
- How sliders work with `@onchange`

---

## Debugging Tips

### "It's not updating!"
Check:
1. Did you call a method that modifies data?
2. Is the method missing `StateHasChanged()`? (Usually not needed, but sometimes Blazor needs a nudge)
3. Try `dotnet build` to catch compile errors

### "The numbers are wrong!"
Check:
1. Print values with `@ingredient.Amount` in the HTML to debug
2. Add breakpoints in your C# methods (F5 to debug)
3. Check your `Measurement.ConvertTo()` logic

### "Where do I see errors?"
- Browser console (F12)
- Terminal running `dotnet run`
- VS Code Problems panel

---

## Key Takeaways

1. **Blazor components** are `.razor` files with HTML + C# together
2. **`@bind`** connects HTML input to C# variables (two-way)
3. **Events** (`@onclick`, `@oninput`) call your C# methods
4. **Loops** (`@foreach`) automatically update when data changes
5. **Your domain classes** (Recipe, Measurement) do the heavy lifting
6. **Blazor re-renders** automatically when data changes

You've already built the hard part (conversions)! Blazor is just showing it in a nice UI.

---

## Resources for Deeper Learning

- Microsoft Blazor Docs: https://learn.microsoft.com/en-us/aspnet/core/blazor/
- C# Pattern Matching: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/patterns
- Understanding Component Lifecycle: https://learn.microsoft.com/en-us/aspnet/core/blazor/components/lifecycle
