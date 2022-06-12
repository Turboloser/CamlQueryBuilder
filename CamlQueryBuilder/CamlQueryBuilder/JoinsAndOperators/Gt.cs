namespace CamlQueryBuilder.JoinsAndOperators
{
    /// <summary>
    /// Checks, if a Fields Value is greater to the provided Value.
    /// </summary>
    public class Gt : ComparsionOperator
    {
        /// <summary>
        /// Checks, if a Fields Value is greater to the provided Value.
        /// </summary>
        /// <param name="fieldName">The Name of the Field to check.</param>
        /// <param name="value">The Value to check against.</param>
        /// <param name="valueType">An (optional) Value Type to use when performing Checks. Default is Text.</param>
        public Gt(string fieldName, string value, CamlQueryBuilderRoot.ValueType valueType = CamlQueryBuilderRoot.ValueType.Text)
        {
            this.fieldName = fieldName;
            this.value = value;
            this.valueType = valueType;
        }

        /// <summary>
        /// Creates a CamlQuery-String using the current Configuration.
        /// </summary>
        /// <returns>A new CamlQuery-String.</returns>
        public override string ToCamlQueryString()
        {
            return $"<Gt><FieldRef Name='{fieldName}'/><Value Type='{valueType}'>{value}</Value></Gt>";
        }
    }
}
