using System;

namespace Assets.Scripts.Enums
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Get the next Enum value in an Enum, or the first value if this enum is currently set to the last value in the Enum.
        /// </summary>
        /// <typeparam name="T">The Enum to cycle through.</typeparam>
        /// <param name="entry">The instance of the Enum.</param>
        /// <returns>The next Enum value, or the first in the Enum if <paramref name="entry"/> refers to the last entry in the Enum.</returns>
        public static T CircularNext<T>(this T entry) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

            T[] AllPossibleValues = (T[])Enum.GetValues(entry.GetType());
            int j = Array.IndexOf<T>(AllPossibleValues, entry) + 1;
            return (AllPossibleValues.Length == j) ? AllPossibleValues[0] : AllPossibleValues[j];
        }

        public static T[] AllPossibleValues<T>() where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

            T[] AllPossibleValues = (T[])Enum.GetValues(typeof(T));
            return AllPossibleValues;
        }
    }
}
