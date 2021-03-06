namespace CamlQueryBuilder.JoinsAndOperators
{
    /// <summary>
    /// Checks, if a Fields Value is excluded in a Lookup-Field with multiple Values.
    /// </summary>
    public class NotIncludes : ComparsionOperator
    {
        /// <summary>
        /// Checks, if a Fields Value is excluded in a Lookup-Field with multiple Values.
        /// </summary>
        /// <param name="fieldName">The Name of the Field to check.</param>
        /// <param name="value">The Value to check against.</param>
        /// <param name="valueType">An (optional) Value Type to use when performing Checks. Default is Text.</param>
        public NotIncludes(string fieldName, string value, CamlQueryBuilderRoot.ValueType valueType = CamlQueryBuilderRoot.ValueType.Text)
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
            return $"<NotIncludes><FieldRef Name='{fieldName}'/><Value Type='{valueType}'>{value}</Value></NotIncludes>";
        }
    }
}
