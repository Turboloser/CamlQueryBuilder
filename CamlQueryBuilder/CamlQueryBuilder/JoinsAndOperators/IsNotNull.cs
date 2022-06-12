namespace CamlQueryBuilder.JoinsAndOperators
{
    /// <summary>
    /// Checks, if a Fields Value is not null.
    /// </summary>
    public class IsNotNull : ComparsionOperator
    {
        /// <summary>
        /// Checks, if a Fields Value is not null.
        /// </summary>
        /// <param name="fieldName">The Name of the Field to check.</param>
        public IsNotNull(string fieldName)
        {
            this.fieldName = fieldName;
        }

        /// <summary>
        /// Creates a CamlQuery-String using the current Configuration.
        /// </summary>
        /// <returns>A new CamlQuery-String.</returns>
        public override string ToCamlQueryString()
        {
            return $"<IsNotNull><FieldRef Name='{fieldName}'/></IsNotNull>";
        }
    }
}
