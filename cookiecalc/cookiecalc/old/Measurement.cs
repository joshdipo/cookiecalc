using System;
using System.Collections.Generic;

namespace cookiecalc.Measurement
{
    /// <summary>
    /// Represents a measurement value with its unit.
    /// Supports conversions between metric/imperial units and between volume/weight.
    /// </summary>
    public class Measurement
    {
        private static readonly Dictionary<MetricVolumeUnit, double> MetricVolumeToMl =
            new()
            {
                { MetricVolumeUnit.Milliliter, 1 },
                { MetricVolumeUnit.Liter, 1000 }
            };

        private static readonly Dictionary<ImperialVolumeUnit, double> ImperialVolumeToMl =
            new()
            {
                { ImperialVolumeUnit.Teaspoon, 4.92892 },
                { ImperialVolumeUnit.Tablespoon, 14.7868 },
                { ImperialVolumeUnit.FluidOunce, 29.5735 },
                { ImperialVolumeUnit.Cup, 236.588 },
                { ImperialVolumeUnit.Pint, 473.176 },
                { ImperialVolumeUnit.Quart, 946.353 },
                { ImperialVolumeUnit.Gallon, 3785.41 }
            };

        private static readonly Dictionary<MetricWeightUnit, double> MetricWeightToG =
            new()
            {
                { MetricWeightUnit.Gram, 1 },
                { MetricWeightUnit.Kilogram, 1000 }
            };

        private static readonly Dictionary<ImperialWeightUnit, double> ImperialWeightToG =
            new()
            {
                { ImperialWeightUnit.Ounce, 28.3495 },
                { ImperialWeightUnit.Pound, 453.592 }
            };

        public double Value { get; set; }
        public object Unit { get; set; }
        public Ingredient? Ingredient { get; set; }

        /// <summary>
        /// Initializes a new measurement with value and unit.
        /// </summary>
        public Measurement(double value, object unit, Ingredient? ingredient = null)
        {
            Value = value;
            Unit = unit;
            Ingredient = ingredient;
        }

        /// <summary>
        /// Check if this measurement is a volume.
        /// </summary>
        public bool IsVolume()
        {
            return Unit is MetricVolumeUnit or ImperialVolumeUnit;
        }

        /// <summary>
        /// Check if this measurement is a weight.
        /// </summary>
        public bool IsWeight()
        {
            return Unit is MetricWeightUnit or ImperialWeightUnit;
        }

        /// <summary>
        /// Check if this measurement uses metric units.
        /// </summary>
        public bool IsMetric()
        {
            return Unit is MetricVolumeUnit or MetricWeightUnit;
        }

        /// <summary>
        /// Check if this measurement uses imperial units.
        /// </summary>
        public bool IsImperial()
        {
            return Unit is ImperialVolumeUnit or ImperialWeightUnit;
        }

        /// <summary>
        /// Convert this measurement to a different unit.
        /// </summary>
        /// <param name="targetUnit">The target unit to convert to</param>
        /// <returns>A new Measurement with the converted value and unit</returns>
        public Measurement ConvertTo(object targetUnit)
        {
            // Volume to Volume conversion
            if (IsVolume() && targetUnit is MetricVolumeUnit or ImperialVolumeUnit)
            {
                var baseML = ConvertVolumeToML(Value, Unit);
                var targetValue = baseML / ConvertVolumeUnitToML(targetUnit);
                return new Measurement(targetValue, targetUnit, Ingredient);
            }

            // Weight to Weight conversion
            if (IsWeight() && targetUnit is MetricWeightUnit or ImperialWeightUnit)
            {
                var baseG = ConvertWeightToG(Value, Unit);
                var targetValue = baseG / ConvertWeightUnitToG(targetUnit);
                return new Measurement(targetValue, targetUnit, Ingredient);
            }

            // Volume to Weight conversion
            if (IsVolume() && targetUnit is MetricWeightUnit or ImperialWeightUnit)
            {
                if (Ingredient is null)
                {
                    throw new InvalidOperationException(
                        "Cannot convert volume to weight without knowing the ingredient. " +
                        "Specify an ingredient when creating the Measurement."
                    );
                }
                return VolumeToWeight(targetUnit);
            }

            // Weight to Volume conversion
            if (IsWeight() && targetUnit is MetricVolumeUnit or ImperialVolumeUnit)
            {
                if (Ingredient is null)
                {
                    throw new InvalidOperationException(
                        "Cannot convert weight to volume without knowing the ingredient. " +
                        "Specify an ingredient when creating the Measurement."
                    );
                }
                return WeightToVolume(targetUnit);
            }

            throw new InvalidOperationException(
                $"Cannot convert {Unit} to {targetUnit}. " +
                "Ensure both units are compatible or specify an ingredient."
            );
        }

        /// <summary>
        /// Convert this measurement to metric equivalent.
        /// </summary>
        public Measurement ToMetric()
        {
            if (IsVolume())
            {
                if (IsMetric())
                {
                    return new Measurement(Value, Unit, Ingredient);
                }
                // Imperial volume to metric
                return ConvertTo(MetricVolumeUnit.Milliliter);
            }
            else if (IsWeight())
            {
                if (IsMetric())
                {
                    return new Measurement(Value, Unit, Ingredient);
                }
                // Imperial weight to metric
                return ConvertTo(MetricWeightUnit.Gram);
            }

            throw new InvalidOperationException("Measurement is neither volume nor weight.");
        }

        /// <summary>
        /// Convert this measurement to imperial equivalent.
        /// </summary>
        public Measurement ToImperial()
        {
            if (IsVolume())
            {
                if (IsImperial())
                {
                    return new Measurement(Value, Unit, Ingredient);
                }
                // Metric volume to imperial
                return ConvertTo(ImperialVolumeUnit.Cup);
            }
            else if (IsWeight())
            {
                if (IsImperial())
                {
                    return new Measurement(Value, Unit, Ingredient);
                }
                // Metric weight to imperial
                return ConvertTo(ImperialWeightUnit.Ounce);
            }

            throw new InvalidOperationException("Measurement is neither volume nor weight.");
        }

        /// <summary>
        /// Returns a formatted string representation of this measurement.
        /// </summary>
        public override string ToString()
        {
            var unitStr = GetUnitString(Unit);
            var ingredientStr = Ingredient.HasValue 
                ? $" ({Ingredient.Value.GetDisplayName()})" 
                : "";
            return $"{Value:F2} {unitStr}{ingredientStr}";
        }

        // Private helper methods

        private Measurement VolumeToWeight(object targetUnit)
        {
            var density = Ingredient!.Value.GetDensity();
            var volumeML = ConvertVolumeToML(Value, Unit);
            var weightG = volumeML * density;
            var targetValue = weightG / ConvertWeightUnitToG(targetUnit);
            return new Measurement(targetValue, targetUnit, Ingredient);
        }

        private Measurement WeightToVolume(object targetUnit)
        {
            var density = Ingredient!.Value.GetDensity();
            var weightG = ConvertWeightToG(Value, Unit);
            var volumeML = weightG / density;
            var targetValue = volumeML / ConvertVolumeUnitToML(targetUnit);
            return new Measurement(targetValue, targetUnit, Ingredient);
        }

        private double ConvertVolumeToML(double value, object unit)
        {
            return value * ConvertVolumeUnitToML(unit);
        }

        private double ConvertVolumeUnitToML(object unit)
        {
            return unit switch
            {
                MetricVolumeUnit metricUnit => MetricVolumeToMl[metricUnit],
                ImperialVolumeUnit imperialUnit => ImperialVolumeToMl[imperialUnit],
                _ => throw new ArgumentException($"Unknown volume unit: {unit}")
            };
        }

        private double ConvertWeightToG(double value, object unit)
        {
            return value * ConvertWeightUnitToG(unit);
        }

        private double ConvertWeightUnitToG(object unit)
        {
            return unit switch
            {
                MetricWeightUnit metricUnit => MetricWeightToG[metricUnit],
                ImperialWeightUnit imperialUnit => ImperialWeightToG[imperialUnit],
                _ => throw new ArgumentException($"Unknown weight unit: {unit}")
            };
        }

        private string GetUnitString(object unit)
        {
            return unit switch
            {
                MetricVolumeUnit.Milliliter => "ml",
                MetricVolumeUnit.Liter => "l",
                ImperialVolumeUnit.Teaspoon => "tsp",
                ImperialVolumeUnit.Tablespoon => "tbsp",
                ImperialVolumeUnit.FluidOunce => "fl oz",
                ImperialVolumeUnit.Cup => "cup",
                ImperialVolumeUnit.Pint => "pt",
                ImperialVolumeUnit.Quart => "qt",
                ImperialVolumeUnit.Gallon => "gal",
                MetricWeightUnit.Gram => "g",
                MetricWeightUnit.Kilogram => "kg",
                ImperialWeightUnit.Ounce => "oz",
                ImperialWeightUnit.Pound => "lb",
                _ => unit.ToString() ?? "unknown"
            };
        }
    }
}
