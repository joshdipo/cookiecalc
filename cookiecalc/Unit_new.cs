using Microsoft.AspNetCore.StaticAssets;

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
}
