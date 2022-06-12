using CamlQueryBuilder.Interfaces;

namespace CamlQueryBuilder.JoinsAndOperators
{
    /// <summary>
    /// An abstract Class, which provides basic Fields for most CamlQuery Comparsion Operators.
    /// </summary>
    public abstract class ComparsionOperator : ICamlQueryElement
    {
        /// <summary>
        /// The Name of the Field to check.
        /// </summary>
        protected string fieldName;

        /// <summary>
        /// The Value to check against.
        /// </summary>
        protected string value;

        /// <summary>
        /// The Value-Type to use. Default is Text.
        /// </summary>
        protected CamlQueryBuilderRoot.ValueType valueType = CamlQueryBuilderRoot.ValueType.Text;

        /// <summary>
        /// Creates a CamlQuery-String using the current Configuration.
        /// </summary>
        /// <returns>A new CamlQuery-String.</returns>
        public abstract string ToCamlQueryString();
    }
}
