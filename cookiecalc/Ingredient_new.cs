using System.Reflection;

namespace cookiecalc.second
{
    /// <summary>
    /// Metric volume measurements for baking.
    /// </summary>
    public enum MetricVolumeUnit
    {
        Milliliter,  // ml
        Liter        // l
    }

    /// <summary>
    /// Imperial volume measurements for baking.
    /// </summary>
    public enum ImperialVolumeUnit
    {
        Teaspoon,    // tsp
        Tablespoon,  // tbsp
        Cup,         // cup
        Pint,        // pt
        Quart,       // qt
        Gallon       // gal
    }

    /// <summary>
    /// Metric weight measurements for baking.
    /// </summary>
    public enum MetricWeightUnit
    {
        Gram,      // g
        Kilogram   // kg
    }

    /// <summary>
    /// Imperial weight measurements for baking.
    /// </summary>
    public enum ImperialWeightUnit
    {
        Ounce,     // oz
        Pound      // lb
    }


    /// <summary>
    /// UnitExtension class containing methods to easily access conversion factors
    /// </summary>
    public static class UnitExtensions
    {
        public static double GetConversionToGrams(this MetricWeightUnit unit) => unit switch
        {
            MetricWeightUnit.Gram => 1,
            MetricWeightUnit.Kilogram => 1000,
            _ => throw new InvalidOperationException()
        };

        public static double GetConversionToMl(this MetricVolumeUnit unit) => unit switch
        {
            MetricVolumeUnit.Milliliter => 1,
            MetricVolumeUnit.Liter => 1000,
            _ => throw new InvalidOperationException()
        };

        public static double GetConversionToGrams(this ImperialWeightUnit unit) => unit switch
        {
            ImperialWeightUnit.Ounce => 28.3495,
            ImperialWeightUnit.Pound => 453.592,
            _ => throw new InvalidOperationException()
        };

        public static double GetConversionToMl(this ImperialVolumeUnit unit) => unit switch
        {
            ImperialVolumeUnit.Teaspoon => 4.92892,
            ImperialVolumeUnit.Tablespoon => 14.7868,
            ImperialVolumeUnit.Cup => 236.588,
            ImperialVolumeUnit.Pint => 473.176,
            ImperialVolumeUnit.Quart => 946.353,
            ImperialVolumeUnit.Gallon => 3785.41,
            _ => throw new InvalidOperationException()
        };
    }


    /// <summary>
    /// Class defining an Ingredient
    /// </summary>
    public class Ingredient
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public double Density { get; set; }

        public Ingredient(string name, string category, double density)
        {
            Name = name;
            Category = category;
            Density = density;
        }
    }


    /// <summary>
    /// Class defining a Measurement
    /// </summary>
    public class Measurement
    {
        public Ingredient Ingredient { get; set; }
        public object Unit { get; set; }
        public double Amount; // Always in grams for simplicity

        public Measurement(Ingredient ingredient, object unit, double amount)
        {
            Ingredient = ingredient;
            Unit = unit;
            SetAmount(amount); // Use SetAmount() method to convert using input unit and set the amount field
        }

        // Methods
        public bool IsVolume()
        {
            return Unit is MetricVolumeUnit or ImperialVolumeUnit;
        }
        public bool IsWeight()
        {
            return Unit is MetricWeightUnit or ImperialWeightUnit;
        }
        public bool IsMetric()
        {
            return Unit is MetricVolumeUnit or MetricWeightUnit;
        }
        public bool IsImperial()
        {
            return Unit is ImperialVolumeUnit or ImperialWeightUnit;
        }

        public double GetAmount()
        {
            if (IsMetric() && IsWeight())
            {
                var conv = ((MetricWeightUnit)Unit).GetConversionToGrams(); // Uses unit extension method to get amount of target unit in grams
                var convertedAmount = Amount / conv; // Convert from grams to target unit by dividing by conversion factor
                return convertedAmount;
            }

            if (IsMetric() && IsVolume())
            {
                var conv = ((MetricVolumeUnit)Unit).GetConversionToMl(); // Get amount of target unit in millilitres
                var convertedAmount = Amount / conv; // Convert from millitres to target unit by dividing by conversion factor (at this point this is the amount in grams as volume assuming the density of water; 1g = 1ml)
                convertedAmount = convertedAmount / Ingredient.Density; // Convert from water to target ingredient by adjusting for density
                return convertedAmount;
            }

            if (IsImperial() && IsWeight())
            {
                var conv = ((ImperialWeightUnit)Unit).GetConversionToGrams(); // Get amount of target unit in grams
                var convertedAmount = Amount / conv; // Convert from grams to target unit by dividing by conversion factor
                return convertedAmount;
            }

            if (IsImperial() && IsVolume())
            {
                var conv = ((ImperialVolumeUnit)Unit).GetConversionToMl(); // Get amount of target unit in millilitres
                var convertedAmount = Amount / conv; // Convert from millitres to target unit by dividing by conversion factor (at this point this is the amount in grams as volume assuming the density of water; 1g = 1ml)
                convertedAmount = convertedAmount / Ingredient.Density; // Convert from water to target ingredient by adjusting for density
                return convertedAmount;
            }

            throw new InvalidOperationException(
                $"Cannot display amount as {Unit}. Not a valid unit"
            );
        }
        public void SetAmount(double _amount)
        {
            if (IsMetric() && IsWeight())
            {
                var conv = ((MetricWeightUnit)Unit).GetConversionToGrams();
                var convertedAmount = _amount * conv;
                Amount = convertedAmount;
                // return $"Amount set to: {convertedAmount} {Unit}";
            }

            if (IsMetric() && IsVolume())
            {
                var conv = ((MetricVolumeUnit)Unit).GetConversionToMl();
                var convertedAmount = _amount * conv;
                convertedAmount = convertedAmount * Ingredient.Density;
                Amount = convertedAmount;
            }

            if (IsImperial() && IsWeight())
            {
                var conv = ((ImperialWeightUnit)Unit).GetConversionToGrams();
                var convertedAmount = _amount * conv;
                Amount = convertedAmount;
            }

            if (IsImperial() && IsVolume())
            {
                var conv = ((ImperialVolumeUnit)Unit).GetConversionToMl();
                var convertedAmount = _amount * conv;
                convertedAmount = convertedAmount * Ingredient.Density;
                Amount = convertedAmount;
            }

            throw new InvalidOperationException(
                $"Cannot set amount as {Unit}. Not a valid unit"
            );
        }
    }
}