using System.Diagnostics;
using System.Runtime.Serialization;
using Contellation.Custom.Controls.Datas.DataGrids;
using Contellation.Custom.Controls.Datas.DataGrids.Generic;

namespace Contellation.Custom.Controls
{
    [DataContract]
    public sealed class FilterCommon : NotifyProperty
    {
        private bool isFiltered;

        public HashSet<object> PreviouslyFilteredItems { get; set; } = new HashSet<object>(EqualityComparer<object>.Default);

        [DataMember(Name = nameof(FilteredItems))]
        public List<object> FilteredItems
        {
            get
            {
                return FieldType?.BaseType == typeof(Enum) ? PreviouslyFilteredItems.ToList().ConvertAll(f => (object)f.ToString()) : PreviouslyFilteredItems?.ToList();
            }

            set => PreviouslyFilteredItems = value.ToHashSet();
        }

        [DataMember(Name = nameof(FieldName))]
        public string FieldName { get; set; }

        public Button FilterButton { get; set; }
        public Loc Translate { get; set; }

        // Use a string to store the type name for serialization
        [DataMember(Name = nameof(FieldType))]
        private string FieldTypeString { get; set; }

        // Property to get and set the actual Type
        public Type FieldType
        {
            get
            {
                try
                {
                    return Type.GetType(FieldTypeString);
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it as needed
                    Debug.WriteLine($"Error deserializing type: {ex.Message}");
                    return null; // or a default type, e.g., typeof(object)
                }
            }
            set => FieldTypeString = value?.AssemblyQualifiedName;
        }
        public bool IsFiltered
        {
            get { return isFiltered; }
            set
            {
                isFiltered = value;
                OnPropertyChanged(nameof(IsFiltered));
            }
        }

        /// <summary>
        /// Add the filter to the predicate dictionary
        /// </summary>
        public void AddFilter(Dictionary<string, Predicate<object>> criteria)
        {
            if (IsFiltered) return;

            // add to list of predicates
            criteria.Add(FieldName, Predicate);

            IsFiltered = true;
            return;

            // predicate of filter
            bool Predicate(object o)
            {
                var value = FieldType == typeof(DateTime) ? ((DateTime?)o.GetPropertyValue(FieldName))?.Date : o.GetPropertyValue(FieldName);

                return !PreviouslyFilteredItems.Contains(value);
            }
        }
    }
}
