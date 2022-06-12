namespace CamlQueryBuilder.JoinsAndOperators
{
    /// <summary>
    /// Checks, if a Fields Value is null.
    /// </summary>
    public class IsNull : ComparsionOperator
    {
        /// <summary>
        /// Checks, if a Fields Value is null.
        /// </summary>
        /// <param name="fieldName">The Name of the Field to check.</param>
        public IsNull(string fieldName)
        {
            this.fieldName = fieldName;
        }

        /// <summary>
        /// Creates a CamlQuery-String using the current Configuration.
        /// </summary>
        /// <returns>A new CamlQuery-String.</returns>
        public override string ToCamlQueryString()
        {
            return $"<IsNull><FieldRef Name='{fieldName}'/></IsNull>";
        }
    }
}
