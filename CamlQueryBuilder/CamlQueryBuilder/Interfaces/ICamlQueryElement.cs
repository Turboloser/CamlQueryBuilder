
namespace CamlQueryBuilder.Interfaces
{
    /// <summary>
    /// Interface for all CamlQuery-Elements.
    /// </summary>
    public interface ICamlQueryElement
    {
        /// <summary>
        /// Creates a CamlQuery-String using the current Configuration.
        /// </summary>
        /// <returns>A new CamlQuery-String.</returns>
        string ToCamlQueryString();
    }
}
