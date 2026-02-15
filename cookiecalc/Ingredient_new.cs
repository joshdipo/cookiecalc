using System.Reflection;

namespace cookiecalc.second
{
    public class Ingredient
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Density { get; set; }
    }


    public class Measurement
    {
        public Ingredient Ingredient { get; set; }
        public object Unit { get; set; }
        public double Amount; // Always in Grams for simplicity

        public Measurement(Ingredient ingredient, object unit, double amount)
        {
            Ingredient = ingredient;
            Unit = unit;
            SetAmount(amount); // Use SetAmount() method to convert using input unit and set the amount field
        }



        // Units and conversion mappings
        private static readonly Dictionary<string, double> MetricWeight =
            new()
            {
                { "Gram", 1 },
                { "Kilogram", 1000 }
            };
        
        private readonly struct MetricWeight
        {
            // unit dicts?


            // bool functions?


            // conversion factor mappings?
        }

        // Conversion mappings
        private static double GetConversionToGrams(this MetricWeightUnit unit)
        private static readonly Dictionary<MetricWeightUnit, double> MetricWeightToG =
            new()
            {
                { MetricWeightUnit.Gram, 1 },
                { MetricWeightUnit.Kilogram, 1000 }
            };
        private static readonly Dictionary<MetricVolumeUnit, double> MetricVolumeToMl =
            new()
            {
                { MetricVolumeUnit.Milliliter, 1 },
                { MetricVolumeUnit.Liter, 1000 }
            };
        private static readonly Dictionary<ImperialWeightUnit, double> ImperialWeightToG =
            new()
            {
                { ImperialWeightUnit.Ounce, 28.3495 },
                { ImperialWeightUnit.Pound, 453.592 }
            };
        private static readonly Dictionary<ImperialVolumeUnit, double> ImperialVolumeToMl =
            new()
            {
                { ImperialVolumeUnit.Teaspoon, 4.92892 },
                { ImperialVolumeUnit.Tablespoon, 14.7868 },
                { ImperialVolumeUnit.Cup, 236.588 },
                { ImperialVolumeUnit.Pint, 473.176 },
                { ImperialVolumeUnit.Quart, 946.353 },
                { ImperialVolumeUnit.Gallon, 3785.41 }
            };




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
                var conv = MetricWeightToG[(MetricWeightUnit)Unit]; // Get amount of target Unit in Grams
                var convertedAmount = Amount / conv; // Convert from Grams to target Unit by dividing by conversion factor
                return convertedAmount;
            }

            if (IsMetric() && IsVolume())
            {
                var conv = MetricVolumeToMl[(MetricVolumeUnit)Unit]; // Get amount of target Unit in Grams
                var convertedAmount = Amount / conv; // Convert from Grams to target Unit by dividing by conversion factor
                convertedAmount = convertedAmount / double.Parse(Ingredient.Density); // Convert from Grams to target volume Unit by dividing by conversion factor and adjusting for density
                return convertedAmount;
            }

            if (IsImperial() && IsWeight())
            {
                var conv = ImperialWeightToG[(ImperialWeightUnit)Unit]; // Get amount of target Unit in Grams
                var convertedAmount = Amount / conv; // Convert from Grams to target Unit by dividing by conversion factor
                return convertedAmount;
            }

            if (IsImperial() && IsVolume())
            {
                var conv = ImperialVolumeToMl[(ImperialVolumeUnit)Unit]; // Get amount of target Unit in Grams
                var convertedAmount = Amount / conv; // Convert from Grams to target Unit by dividing by conversion factor
                convertedAmount = convertedAmount / double.Parse(Ingredient.Density); // Convert from Grams to target volume Unit by dividing by conversion factor and adjusting for density
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
                var conv = MetricWeightToG[(MetricWeightUnit)Unit];
                var convertedAmount = _amount * conv;
                Amount = convertedAmount;
                // return $"Amount set to: {convertedAmount} {Unit}";
            }

            if (IsMetric() && IsVolume())
            {
                var conv = MetricVolumeToMl[(MetricVolumeUnit)Unit];
                var convertedAmount = _amount * conv;
                convertedAmount = convertedAmount * double.Parse(Ingredient.Density);
                Amount = convertedAmount;
            }

                        if (IsImperial() && IsWeight())
            {
                var conv = ImperialWeightToG[(ImperialWeightUnit)Unit];
                var convertedAmount = _amount * conv;
                Amount = convertedAmount;
            }

            if (IsImperial() && IsVolume())
            {
                var conv = ImperialVolumeToMl[(ImperialVolumeUnit)Unit];
                var convertedAmount = _amount * conv;
                convertedAmount = convertedAmount * double.Parse(Ingredient.Density);
                Amount = convertedAmount;
            }

            throw new InvalidOperationException(
                $"Cannot set amount as {Unit}. Not a valid unit"
            );
        }
    }
}