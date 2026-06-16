namespace Contellation.Custom.Controls
{
    public class FilterItemDate : FilterBase
    {
        public List<FilterItemDate> Children { get; set; }

        private bool? isChecked;
        /// <summary>
        /// State of checkbox
        /// </summary>
        public bool? IsChecked
        {
            get { return isChecked; }
            set { SetIsChecked(value, true, true); }
        }

        private bool? initialState;
        /// <summary>
        /// Initial state
        /// </summary>
        public bool? Initialize
        {
            set
            {
                initialState = value;
                isChecked = value;
            }
        }

        public FilterItem Item { get; set; }
        public FilterItemDate Parent { get; set; }
        public List<FilterItemDate> Tree { get; set; }

        private void SetIsChecked(bool? value, bool updateChildren, bool updateParent)
        {
            if (value == isChecked) return;

            isChecked = value;

            IsChanged = initialState != isChecked;

            // filter Item linked to the day, it propagates the states changes.
            // Only the days have a reference to an item in the list used to generate the tree.
            if (Item != null)
            {
                Item.IsChanged = IsChanged;
                Item.Initialize = IsChecked == true;
            }

            // (Select All) item
            if (Level == 0) { Tree?.Skip(1).ToList().ForEach(c => { c.SetIsChecked(value, true, true); }); }

            // state.HasValue : !null
            if (updateChildren && isChecked.HasValue && Level > 0) { Children?.ForEach(c => { c.SetIsChecked(value, true, false); }); }

            if (updateParent) { Parent?.VerifyCheckedState(); }

            OnPropertyChanged(nameof(IsChecked));
        }

        private void VerifyCheckedState()
        {
            bool? b = null;

            for (var i = 0; i < Children.Count; ++i)
            {
                var item = Children[i];
                var current = item.IsChecked;

                if (i == 0)
                {
                    b = current;
                }
                else if (b != current)
                {
                    b = null;
                    break;
                }
            }

            SetIsChecked(b, false, true);
        }
    }
}
