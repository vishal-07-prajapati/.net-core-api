namespace API.Utilities
{
    public static class CommonExtension
    {
        #region Generic Extensions
        /// <summary>
        /// this method returns bool to check if list has any items with null handle
        /// </summary>
        /// <typeparam name="T">Class</typeparam>
        /// <param name="list">list of the objects</param>
        /// <returns>bool</returns>
        public static bool AnyItems<T>(this List<T> list, Func<T, bool> predicate) => list != null && list.Any(predicate);
        #endregion
    }
}
