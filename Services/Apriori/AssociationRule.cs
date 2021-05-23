using Accord.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{

    /// <summary>
    ///   Association rule.
    /// </summary>
    /// 
    /// <typeparam name="T">The item type.</typeparam>
    /// 
    [Serializable]
    public class AssociationRule<T>
    {
        /// <summary>
        ///   Gets or sets the set of items that triggers the 
        ///   activation of this association rule.
        /// </summary>
        /// 
        public SortedSet<T> X { get; set; }

        /// <summary>
        ///   Gets or sets the set of items that are also likely to
        ///   be included in the original list given that <see cref="X">
        ///   the input items</see> are present.
        /// </summary>
        /// 
        public SortedSet<T> Y { get; set; }

        /// <summary>
        ///   Gets or sets the number of cases that support this rule
        ///   (the number of times this association has been seen in the
        ///   training set).
        /// </summary>
        /// 
        public double Support { get; set; }

        /// <summary>
        ///   Gets or sets the confidence of this rule (as a percentage).
        /// </summary>
        /// 
        public double Confidence { get; set; }


        /// <summary>
        ///   Determines whether this rule can be applied to a given input.
        /// </summary>
        /// <param name="input">The set of elements (being bought, or processed).</param>
        /// 
        /// <returns>True, if this rule can be applied to the given set of inputs; false othersie.</returns>
        /// 
        public bool Matches(SortedSet<T> input)
        {
            return X.IsSubsetOf(input);
        }

        /// <summary>
        ///   Determines whether this rule can be applied to a given input.
        /// </summary>
        /// <param name="input">The set of elements (being bought, or processed).</param>
        /// 
        /// <returns>True, if this rule can be applied to the given set of inputs; false othersie.</returns>
        /// 
        public bool Matches(T[] input)
        {
            return X.IsSubsetOf(input);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(X.ToArray().ToString(OctaveArrayFormatProvider.InvariantCulture));
            sb.Append(" -> ");
            sb.Append(Y.ToArray().ToString(OctaveArrayFormatProvider.InvariantCulture));
            sb.AppendFormat("; support: {0}, confidence: {1}", Support, Confidence);
            return sb.ToString();
        }

    }
}

