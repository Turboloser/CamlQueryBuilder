namespace CamlQueryBuilder.JoinsAndOperators
{
    /// <summary>
    /// Checks, if a Field Value does contain a Value existing in a provided List of Values.
    /// </summary>
    public class In : ComparsionOperator
    {
        /// <summary>
        /// The Values to check against.
        /// </summary>
        protected string[] values;

        /// <summary>
        /// Checks, if a Field Value does contain a Value existing in a provided List of Values.
        /// </summary>
        /// <param name="fieldName">The Name of the Field to check.</param>
        /// <param name="values">The Values to check against.</param>
        /// <param name="valueType">An (optional) Value Type to use when performing Checks. Default is Text.</param>
        public In(string fieldName, string[] values, CamlQueryBuilderRoot.ValueType valueType = CamlQueryBuilderRoot.ValueType.Text)
        {
            this.fieldName = fieldName;
            this.values = values;
            this.valueType = valueType;
        }

        /// <summary>
        /// Creates a CamlQuery-String using the current Configuration.
        /// </summary>
        /// <returns>A new CamlQuery-String.</returns>
        public override string ToCamlQueryString()
        {
            return $"<In><FieldRef Name='{fieldName}'/><Values>{string.Join("", values.Select(x => "<Value Type='" + valueType + "'>" + x + "</Value>"))}</Values></In>";
        }
    }
}
