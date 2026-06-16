using Contellation.Custom.Controls.Datas.DataGrids.Generic;

namespace Contellation.Custom.Controls
{
    public abstract class FilterBase : NotifyProperty
    {
        /// <summary>
        ///     Raw value of the item (not displayed, see Label property)
        /// </summary>
        public object Content { get; set; }

        /// <summary>
        ///     Content length
        /// </summary>
        public int ContentLength { get; set; }

        /// <summary>
        ///     Field type
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public Type FieldType { get; set; }

        /// <summary>
        ///     State change flag
        /// </summary>
        public bool IsChanged { get; set; }

        /// <summary>
        ///     Content displayed
        /// </summary>
        public object Label { get; set; }

        /// <summary>
        ///     Hierarchical level
        /// </summary>
        public int Level { get; set; }
    }
}
