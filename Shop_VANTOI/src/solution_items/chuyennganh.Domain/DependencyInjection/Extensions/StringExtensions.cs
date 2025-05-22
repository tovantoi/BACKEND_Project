namespace chuyennganh.Domain.DependencyInjection.Extensions
{
    /// <summary>
    /// Extensions for string
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgText"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatMsg(this string msgText, params object[] args)
        {
            return string.Format(msgText, args);
        }
    }
}