using CamlQueryBuilder.Interfaces;
using System.Text;
using System.Xml;

namespace CamlQueryBuilder
{
    /// <summary>
    /// A CamlQuery-Builder to create simple Querys without using magic Strings.
    /// 
    /// Usage:
    /// - Instantiate the Root-Element by creating an Instance of this Class.
    /// - Add ICamlQueryElement-Instances. e.g. And, Or or Eq.
    /// - (Optional) Add SortingOrder(s) by calling AddOrderBy().
    /// - (Optional) Set RecursiveAll-Flag by calling SetRecursiveAll().
    /// - (Optional) Limit the Amount of returned Results by calling SetRowLimit().
    /// - Create the CamlQuery-String by calling ToCamlQueryString() on your Instance afterwards.
    /// </summary>
    public class CamlQueryBuilderRoot : ICamlQueryElement
    {
        /// <summary>
        /// Defines the Sorting Order when adding an OrderBy-Operator.
        /// </summary>
        public enum SortingOrder { Ascending, Descending }

        /// <summary>
        /// The Value Types used for Comparsion Operators.
        /// A List of existing Types can be found at:
        /// https://docs.microsoft.com/en-us/previous-versions/office/sharepoint-server/ms428806(v=office.15)?redirectedfrom=MSDN
        /// </summary>
        public enum ValueType { Invalid, Integer, Text, Note, DateTime, Counter, Choice, Lookup, Boolean, Number, Currency, URL, Computed, Threading, Guid, MultiChoice, GridChoice, Calculated, File, Attachments, User, Recurrence, CrossProjectLink, ModStat, Error, ContentTypeId, PageSeparator, ThreadIndex, WorkflowStatus, AllDayEvent, WorkflowEventType, MaxItems }

        /// <summary>
        /// The Root Element containing all CamlQuery Elements.
        /// </summary>
        private ICamlQueryElement rootElement;

        /// <summary>
        /// The OrderBy FieldNames used to sort the CamlQuery-Results.
        /// </summary>
        private List<string> orderByFieldNames;

        /// <summary>
        /// The OrderBy SortDirections used to sort the CamlQuery-Results.
        /// </summary>
        private List<SortingOrder> orderBySortingOrders;

        /// <summary>
        /// The GroupBy FieldNames used to group the CamlQuery-Results.
        /// </summary>
        private List<string> groupByFieldNames;

        /// <summary>
        /// The ViewFields used to filter the Columns returned by the CamlQuery.
        /// </summary>
        private List<string> viewFieldNames;

        /// <summary>
        /// An optional Limit of Results to return. May be set to a Number equal or below 0 to indicate no Limit.
        /// </summary>
        private int rowLimit;

        /// <summary>
        /// Flag indicating, if the RecursiveAll-Scope Option will be used in the Query.
        /// </summary>
        private bool recursiveAll;

        /// <summary>
        /// Flag indicating, if additional System-Fields will be returned by the Query.
        /// </summary>
        private bool queryOptionIncludeMandatoryColumns;

        /// <summary>
        /// Flag indicating, that all DateTimes should be returned in UTC.
        /// </summary>
        private bool queryOptionDateInUTC;

        /// <summary>
        /// Flag indicating, if the Output-String should be formatted.
        /// </summary>
        private bool formatOutput;

        /// <summary>
        /// Creates a new Instance.
        /// </summary>
        /// <param name="rootElement">The CamlQuery Element to use when generating the Query-String.</param>
        public CamlQueryBuilderRoot(ICamlQueryElement rootElement)
        {
            this.rootElement = rootElement;
            orderByFieldNames = new List<string>();
            orderBySortingOrders = new List<SortingOrder>();
            groupByFieldNames = new List<string>();
            viewFieldNames = new List<string>();
            rowLimit = -1;
        }

        /// <summary>
        /// Adds an OrderBy-Element to the CamlQuery.
        /// </summary>
        /// <param name="fieldName">The Field to use for Sorting.</param>
        /// <param name="sortAscending">Sorting-Direction Value.</param>
        /// <returns>The Builder-Instance.</returns>
        public CamlQueryBuilderRoot OrderBy(string fieldName, SortingOrder sortAscending)
        {
            //Validations
            ValidateFieldName(fieldName);

            //Save Values
            orderByFieldNames.Add(fieldName);
            orderBySortingOrders.Add(sortAscending);

            //Done
            return this;
        }

        /// <summary>
        /// Adds an GroupBy-Element to the CamlQuery.
        /// </summary>
        /// <param name="fieldName">The Field to use for Grouping.</param>
        /// <returns>The Builder-Instance.</returns>
        public CamlQueryBuilderRoot GroupBy(string fieldName)
        {
            //Validations
            ValidateFieldName(fieldName);

            //Save Value
            groupByFieldNames.Add(fieldName);

            //Done
            return this;
        }

        /// <summary>
        /// Adds a ViewFields-Element to the CamlQuery.
        /// </summary>
        /// <param name="fieldName">The Field to add.</param>
        /// <returns>The Builder-Instance.</returns>
        public CamlQueryBuilderRoot AddViewField(string fieldName)
        {
            //Validations
            ValidateFieldName(fieldName);

            //Save Value
            viewFieldNames.Add(fieldName);

            //Done
            return this;
        }

        /// <summary>
        /// Sets the RowLimit for the CamlQuery, limiting the maximum Amount of Results to return.
        /// </summary>
        /// <param name="rowLimit">The RowLimit.</param>
        /// <returns>The Builder-Instance.</returns>
        public CamlQueryBuilderRoot SetRowLimit(int rowLimit)
        {
            //Validations
            if (rowLimit <= 0)
            {
                throw new ArgumentException($"rowLimit may not be smaller or equal to 0, but was {rowLimit}!");
            }

            //Save Value
            this.rowLimit = rowLimit;

            //Done
            return this;
        }

        /// <summary>
        /// Call, when the RecursiveAll-Scope Option should be used in the Query.
        /// Setting this Option will return all matching Items in a List, including Subfolders, which are otherwise ignored.
        /// </summary>
        /// <returns>The Builder-Instance.</returns>
        public CamlQueryBuilderRoot SetRecursiveAll()
        {
            //Save Value
            recursiveAll = true;

            //Done
            return this;
        }

        /// <summary>
        /// Call, when the IncludeMandatoryColumns Option should be used in the Query.
        /// Setting this Option will return additional System-Fields otherwise hidden.
        /// </summary>
        /// <returns>The Builder-Instance.</returns>
        public CamlQueryBuilderRoot QueryOptionIncludeMandatoryColumns()
        {
            //Save Value
            queryOptionIncludeMandatoryColumns = true;

            //Done
            return this;
        }

        /// <summary>
        /// Call, when the DateInUTC Option should be used in the Query.
        /// Setting this Option makes SP return all DateTimes in UTC instead of trying to transforming it into lokal Time.
        /// </summary>
        /// <returns>The Builder-Instance.</returns>
        public CamlQueryBuilderRoot QueryOptionDateInUTC()
        {
            //Save Value
            queryOptionDateInUTC = true;

            //Done
            return this;
        }

        /// <summary>
        /// Call, when the Output-String should be formatted to be 'human-readable'.
        /// </summary>
        /// <returns>The Builder-Instance.</returns>
        public CamlQueryBuilderRoot FormatOutput()
        {
            //Save Value
            formatOutput = true;

            //Done
            return this;
        }

        /// <summary>
        /// Creates a CamlQuery-String using the current Configuration.
        /// </summary>
        /// <returns>A new CamlQuery-String.</returns>
        public string ToCamlQueryString()
        {
            //Config RecursiveAll-Flag
            string recursiveAllElement = recursiveAll ? " Scope='RecursiveAll'" : "";

            //Config OrderBys
            string orderByFields = "";

            for (int i = 0; i < orderByFieldNames.Count; i++)
            {
                orderByFields += $"<FieldRef Name='{orderByFieldNames[i]}' Ascending='{(orderBySortingOrders[i] == SortingOrder.Ascending ? "TRUE" : "FALSE")}' />";
            }

            string orderByElement = orderByFieldNames.Count > 0 ? $"<OrderBy>{orderByFields}</OrderBy>" : "";

            //Config GroupBys
            string groupByFields = "";

            for (int i = 0; i < groupByFieldNames.Count; i++)
            {
                groupByFields += $"<FieldRef Name='{groupByFieldNames[i]}' />";
            }

            string groupByElement = groupByFieldNames.Count > 0 ? $"<GroupBy>{groupByFields}</GroupBy>" : "";

            //Config ViewFields
            string viewFields = "";

            for (int i = 0; i < viewFieldNames.Count; i++)
            {
                viewFields += $"<FieldRef Name='{viewFieldNames[i]}' />";
            }

            string viewFieldElement = viewFieldNames.Count > 0 ? $"<ViewFields>{viewFields}</ViewFields>" : "";

            //Config RowLimit
            string rowLimitElement = rowLimit > 0 ? $"<RowLimit>{rowLimit}</RowLimit>" : "";

            //Config additional QueryOptions
            string queryOptionsElement = "";

            if (queryOptionIncludeMandatoryColumns ||
                queryOptionDateInUTC)
            {
                queryOptionsElement = "<QueryOptions>";

                if (queryOptionIncludeMandatoryColumns)
                {
                    queryOptionsElement += "<IncludeMandatoryColumns>TRUE</IncludeMandatoryColumns>";
                }

                if (queryOptionDateInUTC)
                {
                    queryOptionsElement += "<DateInUtc>TRUE</DateInUtc>";
                }

                queryOptionsElement += "</QueryOptions>";
            }

            //Create CamlQuery String
            string camlQuery = $"<View{recursiveAllElement}><Query>{orderByElement}{groupByElement}{viewFieldElement}<Where>{rootElement.ToCamlQueryString()}</Where></Query>{rowLimitElement}{queryOptionsElement}</View>";

            //Format
            if (formatOutput)
            {
                camlQuery = FormatCamlQuery(camlQuery);
            }

            //Done
            return camlQuery;
        }

        /// <summary>
        /// Validates the provided FieldName.
        /// </summary>
        /// <param name="fieldName">The FieldName to validate.</param>
        private void ValidateFieldName(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                throw new ArgumentException("fieldName may not be null or empty!");
            }
        }

        /// <summary>
        /// Formats the provided CamlQuery, returning it as a new String with Newlines and Indentations.
        /// Bases upon:
        /// https://stackoverflow.com/questions/1123718/format-xml-string-to-print-friendly-xml-string
        /// </summary>
        /// <param name="camlQuery">The unformatted CamlQuery.</param>
        /// <returns>The formatted CamlQuery.</returns>
        private string FormatCamlQuery(string camlQuery)
        {
            //Init
            string formattedCamlQuery;

            //Init Streams, needed to parse the camlQuery-String into a Document and format it
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.Unicode))
                {
                    //Parse camlQuery-String into an XML-Document
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(camlQuery);

                    //Write String back from Document to MemoryStream, indented this Time
                    xmlTextWriter.Formatting = Formatting.Indented;
                    xmlDocument.WriteContentTo(xmlTextWriter);
                    xmlTextWriter.Flush();
                    memoryStream.Flush();
                    memoryStream.Position = 0;

                    //Read formatted Text from Memory Stream
                    using (StreamReader streamReader = new StreamReader(memoryStream))
                    {
                        formattedCamlQuery = streamReader.ReadToEnd();
                    }
                }
            }

            //Done
            return formattedCamlQuery;
        }
    }
}
