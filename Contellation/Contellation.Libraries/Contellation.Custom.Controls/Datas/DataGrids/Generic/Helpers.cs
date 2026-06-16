using System.Diagnostics;

namespace Contellation.Custom.Controls.Datas.DataGrids.Generic
{
    public static class Helpers
    {
        #region Public Methods

        /// <summary>
        ///     Print elapsed time
        /// </summary>
        /// <param name="label"></param>
        /// <param name="start"></param>
        public static void Elapsed(string label, DateTime start)
        {
            var span = DateTime.Now - start;
            Debug.WriteLine($"{label,-20}{span:mm\\:ss\\.ff}");
        }

        #endregion Public Methods
    }
}
