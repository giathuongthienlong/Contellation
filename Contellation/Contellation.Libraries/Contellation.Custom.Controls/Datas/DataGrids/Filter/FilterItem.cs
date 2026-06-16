namespace Contellation.Custom.Controls
{
    public class FilterItem : FilterBase
    {
        private bool isChecked;
        /// <summary>
        /// State of checkbox
        /// </summary>
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                IsChanged = value != initialState;
                OnPropertyChanged(nameof(IsChecked));
            }
        }

        private bool initialState;
        /// <summary>
        /// Initial state
        /// </summary>
        public bool Initialize
        {
            set
            {
                initialState = value;
                isChecked = value;
            }
        }
    }
}
