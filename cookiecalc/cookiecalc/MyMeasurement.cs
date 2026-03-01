using System.Globalization;

namespace cookiecalc.MyMCookie
{
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

        /// <summary>
        /// Gets the quantity of an measurement in the defined units
        /// </summary>
        /// <returns>double</returns>
        /// <exception cref="InvalidOperationException"></exception>
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

        /// <summary>
        /// Sets the Amount for the measurement as grams, converting from an quantity in the defined units
        /// </summary>
        /// <param name="_amount"></param>
        /// <exception cref="InvalidOperationException"></exception>
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

        /// <summary>
        /// Gets the amount for the measurement formatted as a string with the units appended
        /// </summary>
        /// <returns>string</returns>
        public string GetDisplayAmount()
        {
            double amount = GetAmount();
            if (amount <= 1)
            {
                return $"{amount.ToString("N", CultureInfo.InvariantCulture)} {Unit.ToString()}";
            } else
            {
                return $"{amount.ToString("N", CultureInfo.InvariantCulture)} {Unit.ToString()}s";
            }
        }
    }
}