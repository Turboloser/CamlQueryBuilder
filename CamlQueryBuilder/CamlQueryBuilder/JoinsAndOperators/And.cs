using CamlQueryBuilder.Interfaces;

namespace CamlQueryBuilder.JoinsAndOperators
{
    /// <summary>
    /// Generates a new AND-CamlQuery-Element, linking an Array of Elements together using AND-Statements.
    /// </summary>
    public class And : ICamlQueryElement
    {
        /// <summary>
        /// The first Element to link.
        /// </summary>
        private ICamlQueryElement a;

        /// <summary>
        /// The second Element to link. May be null to indicate that no Element should be linked.
        /// </summary>
        private ICamlQueryElement b;

        /// <summary>
        /// Links two or more Elements together using an AND-Statement.
        /// </summary>
        /// <param name="a">The first Element to link.</param>
        /// <param name="b">Additional optional Elements to link.</param>
        public And(ICamlQueryElement a, params ICamlQueryElement[] b)
        {
            //Validations
            if (a == null)
            {
                throw new ArgumentException("a may not be null!");
            }

            //Merge all Data together
            List<ICamlQueryElement> elements = new List<ICamlQueryElement>(b);
            elements.Insert(0, a);

            //Pass to Initialization
            Init(elements);
        }

        /// <summary>
        /// Links an Array of Elements together using an AND-Statement.
        /// </summary>
        /// <param name="elements">The List of Elements. Must at least containt 1 Element.</param>
        public And(IEnumerable<ICamlQueryElement> elements)
        {
            //Validations
            if (elements == null || elements.Count() < 1)
            {
                throw new ArgumentException("elements may not be null or empty!");
            }

            //Pass to Initialization
            Init(elements.ToList());
        }

        /// <summary>
        /// Creates a CamlQuery-String using the current Configuration.
        /// </summary>
        /// <returns>A new CamlQuery-String.</returns>
        public string ToCamlQueryString()
        {
            if (b != null)
            {
                return $"<And>{a.ToCamlQueryString()}{b.ToCamlQueryString()}</And>";
            }
            else
            {
                return a.ToCamlQueryString();
            }
        }

        /// <summary>
        /// Initializes the CamlQuery-Element, using the provided Data.
        /// </summary>
        /// <param name="elements">The List of Elements. Must at least containt 1 Element.</param>
        private void Init(List<ICamlQueryElement> elements)
        {
            if (elements.Count <= 2)
            {
                //One or two Elements provided? Just save them
                a = elements.Count >= 1 ? elements[0] : throw new ArgumentException("elements may not be empty!");
                b = elements.Count == 2 ? elements[1] : null;
            }
            else
            {
                //More than two Elements provided? Chain-Link the remaining ones using the same Technique
                a = elements[0];
                b = new And(elements.Skip(1).ToArray());
            }
        }
    }
}
